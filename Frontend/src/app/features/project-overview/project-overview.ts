import { Component, ElementRef, OnDestroy, ViewChild } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import type { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faChartLine,
  faCircleCheck,
  faCircleExclamation,
  faClock,
  faHashtag,
  faListCheck,
  faSpinner,
  faTriangleExclamation,
} from '@fortawesome/free-solid-svg-icons';
import type { Chart as ChartInstance } from 'chart.js';
import { Notification } from '../../core/services/notification.service';
import { ProjectRefineButton } from '../../shared/components/project-refine-button/project-refine-button';
import { JobStatus } from '../../shared/enums/job-status.enum';
import { UserStoryStage } from '../../shared/enums/user-story-stage.enum';
import { ENTITY_COLLECTION_ICONS, ENTITY_ICONS } from '../../shared/icons/entity-icons';
import { LowestScoreItem } from '../../shared/models/lowest-score-item.model';
import {
  ProjectQualityCriterionAverage,
  ProjectQualityOverview,
} from '../../shared/models/project-quality-overview.model';
import { ProjectScoreTrendPoint } from '../../shared/models/project-score-trend-point.model';
import { ProjectUserStoryDetailCount } from '../../shared/models/project-user-story-detail-count.model';
import {
  ArtifactWordCount,
  ProjectWordCount,
} from '../../shared/models/project-word-count.model';
import { Project } from '../../shared/models/project.model';
import { PersonaService } from '../../shared/services/persona';
import { ProjectService } from '../../shared/services/project';
import { ScenarioService } from '../../shared/services/scenario';
import { UserStoryService } from '../../shared/services/user-story';

type QualityCriterionArtifactType = ProjectQualityCriterionAverage['artifactType'];

interface QualityCriterionAverageGroup {
  artifactType: QualityCriterionArtifactType;
  averages: ProjectQualityCriterionAverage[];
  icon: IconDefinition;
  label: string;
  subtitle: string;
}

interface WordCountArtifactGroup {
  artifacts: ArtifactWordCount[];
  averageWords: number;
  icon: IconDefinition;
  label: string;
  totalWords: number;
}

@Component({
  selector: 'app-project-overview',
  imports: [RouterModule, FontAwesomeModule, ProjectRefineButton],
  templateUrl: './project-overview.html',
  styleUrl: './project-overview.scss',
})
export class ProjectOverview implements OnDestroy {
  @ViewChild('scoreTrendCanvas')
  private set scoreTrendCanvas(canvas: ElementRef<HTMLCanvasElement> | undefined) {
    this.scoreTrendCanvasRef = canvas;
    void this.renderScoreTrendChart();
  }

  public project: Project | null = null;
  public qualityOverview: ProjectQualityOverview | null = null;
  public userStoryDetailCounts: ProjectUserStoryDetailCount | null = null;
  public wordCounts: ProjectWordCount | null = null;
  public loading = true;
  public qualityLoading = false;
  public userStoryDetailCountsLoading = false;
  public wordCountsLoading = false;
  public analyzing = false;
  public analysisJobStatus = JobStatus.None;
  public analysisJobErrorMessage: string | null = null;

  public personaCount = 0;
  public scenarioCount = 0;
  public userStoryCount = 0;
  public closedUserStoryCount = 0;

  public readonly entityCollectionIcons = ENTITY_COLLECTION_ICONS;
  public readonly entityIcons = ENTITY_ICONS;
  public readonly jobStatus = JobStatus;
  public readonly faCircleCheck = faCircleCheck;
  public readonly faCircleExclamation = faCircleExclamation;
  public readonly faClock = faClock;
  public readonly faChartLine = faChartLine;
  public readonly faHashtag = faHashtag;
  public readonly faListCheck = faListCheck;
  public readonly faSpinner = faSpinner;
  public readonly faTriangleExclamation = faTriangleExclamation;

  private readonly analysisPollIntervalMs = 5000;
  private readonly analysisJobStorageKeyPrefix = 'requirement-ai:quality-analysis-job';
  private analysisPollTimeoutId: ReturnType<typeof setTimeout> | null = null;
  private activeAnalysisJobId: string | null = null;
  private scoreTrendCanvasRef: ElementRef<HTMLCanvasElement> | undefined;
  private scoreTrendChart: ChartInstance | null = null;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly projectService: ProjectService,
    private readonly personaService: PersonaService,
    private readonly scenarioService: ScenarioService,
    private readonly userStoryService: UserStoryService,
    private readonly notification: Notification,
  ) {}

  public async ngOnInit(): Promise<void> {
    const projectId = this.route.snapshot.paramMap.get('projectId');

    if (!projectId) {
      this.loading = false;
      return;
    }

    try {
      const [project, personas, userStories] = await Promise.all([
        this.projectService.getById(projectId),
        this.personaService.getByProjectId(projectId),
        this.userStoryService.getByProjectId(projectId),
      ]);

      this.project = project;
      this.personaCount = personas.length;
      this.userStoryCount = userStories.length;
      this.closedUserStoryCount = userStories.filter(
        (story) => story.stage === UserStoryStage.Closed,
      ).length;

      const scenarioGroups = await Promise.all(
        personas.map((persona) => this.scenarioService.getByPersonaId(persona.id)),
      );
      this.scenarioCount = scenarioGroups.flat().length;

      await Promise.all([
        this.loadQualityOverview(projectId),
        this.loadUserStoryDetailCounts(projectId),
        this.loadWordCounts(projectId),
      ]);
      this.resumeStoredQualityAnalysisJob(projectId);
    } finally {
      this.loading = false;
    }
  }

  public ngOnDestroy(): void {
    this.clearAnalysisPolling();
    this.destroyScoreTrendChart();
  }

  public get progressPercent(): number {
    if (this.userStoryCount === 0) {
      return 0;
    }

    return Math.round((this.closedUserStoryCount / this.userStoryCount) * 100);
  }

  public get lowestScoreItems(): LowestScoreItem[] {
    return [
      this.qualityOverview?.lowestPersona,
      this.qualityOverview?.lowestScenario,
      this.qualityOverview?.lowestUserStory,
    ].filter((item): item is LowestScoreItem => !!item && item.score <= 5);
  }

  public get qualityScoreCards(): { label: string; score: number }[] {
    if (!this.qualityOverview) {
      return [];
    }

    return [
      { label: 'Total', score: this.qualityOverview.totalProjectScore },
      { label: 'Personas', score: this.qualityOverview.averagePersonaScore },
      { label: 'Scenarios', score: this.qualityOverview.averageScenarioScore },
      { label: 'User Stories', score: this.qualityOverview.averageUserStoryScore },
    ];
  }

  public get criterionAverageGroups(): QualityCriterionAverageGroup[] {
    const criterionAverages = this.qualityOverview?.criterionAverages ?? [];

    return [
      {
        artifactType: 'Persona',
        averages: this.criterionAveragesFor('Persona', criterionAverages),
        icon: this.entityCollectionIcons.personas,
        label: 'Personas',
        subtitle: 'Persona quality criteria',
      },
      {
        artifactType: 'Scenario',
        averages: this.criterionAveragesFor('Scenario', criterionAverages),
        icon: this.entityCollectionIcons.scenarios,
        label: 'Scenarios',
        subtitle: 'Scenario quality criteria',
      },
      {
        artifactType: 'UserStory',
        averages: this.criterionAveragesFor('UserStory', criterionAverages),
        icon: this.entityIcons.userStory,
        label: 'User Stories',
        subtitle: 'User story quality criteria',
      },
    ];
  }

  public get wordCountArtifactGroups(): WordCountArtifactGroup[] {
    if (!this.wordCounts) {
      return [];
    }

    return [
      this.wordCountArtifactGroup(
        'Personas',
        this.entityCollectionIcons.personas,
        this.wordCounts.averageWordsPerPersona,
        this.wordCounts.wordsPerPersona,
      ),
      this.wordCountArtifactGroup(
        'Scenarios',
        this.entityCollectionIcons.scenarios,
        this.wordCounts.averageWordsPerScenario,
        this.wordCounts.wordsPerScenario,
      ),
      this.wordCountArtifactGroup(
        'User Stories',
        this.entityIcons.userStory,
        this.wordCounts.averageWordsPerUserStory,
        this.wordCounts.wordsPerUserStory,
      ),
    ];
  }

  public async analyzeProject(): Promise<void> {
    if (!this.project || this.analyzing) {
      return;
    }

    this.analyzing = true;
    this.analysisJobStatus = JobStatus.Pending;
    this.analysisJobErrorMessage = null;
    this.clearAnalysisPolling();

    try {
      this.activeAnalysisJobId = await this.projectService.analyze(this.project.id);
      this.storeQualityAnalysisJobId(this.project.id, this.activeAnalysisJobId);
      void this.pollQualityAnalysisJob(this.activeAnalysisJobId);
    } catch {
      this.analyzing = false;
      this.activeAnalysisJobId = null;
      this.analysisJobStatus = JobStatus.Failed;
      this.notification.fail('Could not refresh project analysis');
    }
  }

  public get analysisStatusTitle(): string {
    switch (this.analysisJobStatus) {
      case JobStatus.Pending:
        return 'Analysis queued';
      case JobStatus.Running:
        return 'Analysis running';
      case JobStatus.Completed:
        return 'Analysis updated';
      case JobStatus.Failed:
        return 'Analysis failed';
      default:
        return '';
    }
  }

  public get analysisStatusMessage(): string {
    switch (this.analysisJobStatus) {
      case JobStatus.Pending:
        return 'A new quality analysis is queued. The current scores remain visible until the job finishes.';
      case JobStatus.Running:
        return 'A new quality analysis is being generated. The overview will update automatically when it is ready.';
      case JobStatus.Completed:
        return 'The latest quality analysis is now shown.';
      case JobStatus.Failed:
        return this.analysisJobErrorMessage || 'The quality analysis job failed.';
      default:
        return '';
    }
  }

  public get analysisButtonLabel(): string {
    if (this.analyzing) {
      return this.analysisJobStatus === JobStatus.Running
        ? 'Analysis running...'
        : 'Analysis queued...';
    }

    return 'Analyze Project';
  }

  public scorePercent(score: number): number {
    return Math.round((this.normalizedScore(score) / 10) * 100);
  }

  public scoreLabel(score: number): string {
    return `${this.normalizedScore(score).toFixed(2)}/10`;
  }

  public criterionLabel(criterionName: string): string {
    return criterionName
      .replace(/Score$/, '')
      .replace(/([a-z])([A-Z])/g, '$1 $2')
      .replace(/([A-Z]+)([A-Z][a-z])/g, '$1 $2');
  }

  public isLowScore(score: number): boolean {
    return this.normalizedScore(score) < 5;
  }

  public formatDate(value: string): string {
    const date = new Date(value);

    if (Number.isNaN(date.getTime())) {
      return value;
    }

    return new Intl.DateTimeFormat(undefined, {
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
      month: 'short',
      timeZone: Intl.DateTimeFormat().resolvedOptions().timeZone,
      year: 'numeric',
    }).format(date);
  }

  public formatCount(value: number, fractionDigits = 0): string {
    return new Intl.NumberFormat(undefined, {
      maximumFractionDigits: fractionDigits,
      minimumFractionDigits: fractionDigits,
    }).format(value);
  }

  private async loadQualityOverview(projectId: string): Promise<void> {
    this.qualityLoading = true;

    try {
      this.qualityOverview = await this.projectService.getQualityOverview(projectId);
      void this.renderScoreTrendChart();
    } catch {
      this.qualityOverview = null;
      this.destroyScoreTrendChart();
      this.notification.fail('Could not load project quality overview');
    } finally {
      this.qualityLoading = false;
    }
  }

  private async loadWordCounts(projectId: string): Promise<void> {
    this.wordCountsLoading = true;

    try {
      this.wordCounts = await this.projectService.getWordCounts(projectId);
    } catch {
      this.wordCounts = null;
      this.notification.fail('Could not load project word counts');
    } finally {
      this.wordCountsLoading = false;
    }
  }

  private async loadUserStoryDetailCounts(projectId: string): Promise<void> {
    this.userStoryDetailCountsLoading = true;

    try {
      this.userStoryDetailCounts =
        await this.projectService.getUserStoryDetailCounts(projectId);
    } catch {
      this.userStoryDetailCounts = null;
      this.notification.fail('Could not load user story detail counts');
    } finally {
      this.userStoryDetailCountsLoading = false;
    }
  }

  private async pollQualityAnalysisJob(jobId: string): Promise<void> {
    try {
      const response = await this.projectService.getQualityAnalysisJob(jobId);

      if (jobId !== this.activeAnalysisJobId) {
        return;
      }

      if (!response.successful) {
        this.clearActiveAnalysisJob();
        return;
      }

      const job = response.data;

      if (!job) {
        this.clearActiveAnalysisJob();
        return;
      }

      this.analysisJobStatus = this.normalizeJobStatus(job.status);
      this.analysisJobErrorMessage = job.errorMessage;

      switch (this.analysisJobStatus) {
        case JobStatus.Pending:
        case JobStatus.Running:
          this.scheduleAnalysisPoll(jobId);
          return;
        case JobStatus.Completed:
          this.clearActiveAnalysisJob(job.projectId, JobStatus.Completed);
          await this.loadQualityOverview(job.projectId);
          return;
        case JobStatus.Failed:
          this.clearActiveAnalysisJob(job.projectId, JobStatus.Failed, job.errorMessage);
          return;
        case JobStatus.None:
          this.scheduleAnalysisPoll(jobId);
          return;
      }
    } catch {
      if (jobId === this.activeAnalysisJobId) {
        this.scheduleAnalysisPoll(jobId);
      }
    }
  }

  private scheduleAnalysisPoll(jobId: string): void {
    this.clearAnalysisPolling();
    this.analysisPollTimeoutId = setTimeout(() => {
      void this.pollQualityAnalysisJob(jobId);
    }, this.analysisPollIntervalMs);
  }

  private clearAnalysisPolling(): void {
    if (this.analysisPollTimeoutId === null) {
      return;
    }

    clearTimeout(this.analysisPollTimeoutId);
    this.analysisPollTimeoutId = null;
  }

  private normalizeJobStatus(status: JobStatus): JobStatus {
    switch (status) {
      case JobStatus.Pending:
      case JobStatus.Running:
      case JobStatus.Completed:
      case JobStatus.Failed:
      case JobStatus.None:
        return status;
      default:
        return JobStatus.None;
    }
  }

  private clearActiveAnalysisJob(
    projectId = this.project?.id,
    status = JobStatus.None,
    errorMessage: string | null = null,
  ): void {
    this.analyzing = false;
    this.activeAnalysisJobId = null;
    this.analysisJobStatus = status;
    this.analysisJobErrorMessage = errorMessage;
    this.clearAnalysisPolling();

    if (projectId) {
      this.clearStoredQualityAnalysisJobId(projectId);
    }
  }

  private resumeStoredQualityAnalysisJob(projectId: string): void {
    const jobId = this.storedQualityAnalysisJobId(projectId);

    if (!jobId) {
      return;
    }

    this.analyzing = true;
    this.analysisJobStatus = JobStatus.Pending;
    this.analysisJobErrorMessage = null;
    this.activeAnalysisJobId = jobId;
    void this.pollQualityAnalysisJob(jobId);
  }

  private storedQualityAnalysisJobId(projectId: string): string | null {
    try {
      return window.localStorage.getItem(this.qualityAnalysisJobStorageKey(projectId));
    } catch {
      return null;
    }
  }

  private storeQualityAnalysisJobId(projectId: string, jobId: string): void {
    try {
      window.localStorage.setItem(this.qualityAnalysisJobStorageKey(projectId), jobId);
    } catch {
      return;
    }
  }

  private clearStoredQualityAnalysisJobId(projectId: string): void {
    try {
      window.localStorage.removeItem(this.qualityAnalysisJobStorageKey(projectId));
    } catch {
      return;
    }
  }

  private qualityAnalysisJobStorageKey(projectId: string): string {
    return `${this.analysisJobStorageKeyPrefix}:${projectId}`;
  }

  private criterionAveragesFor(
    artifactType: QualityCriterionArtifactType,
    criterionAverages: ProjectQualityCriterionAverage[],
  ): ProjectQualityCriterionAverage[] {
    return criterionAverages.filter((average) => average.artifactType === artifactType);
  }

  private wordCountArtifactGroup(
    label: string,
    icon: IconDefinition,
    averageWords: number,
    artifacts: ArtifactWordCount[],
  ): WordCountArtifactGroup {
    const sortedArtifacts = [...artifacts].sort(
      (left, right) => right.words - left.words || left.title.localeCompare(right.title),
    );

    return {
      artifacts: sortedArtifacts,
      averageWords,
      icon,
      label,
      totalWords: artifacts.reduce((total, artifact) => total + artifact.words, 0),
    };
  }

  private dateKey(value: string | Date): string {
    if (typeof value === 'string') {
      return value.split('T')[0];
    }

    const year = value.getFullYear();
    const month = String(value.getMonth() + 1).padStart(2, '0');
    const day = String(value.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

  private normalizedScore(score: number): number {
    return Math.max(0, Math.min(10, score));
  }

  private async renderScoreTrendChart(): Promise<void> {
    const canvas = this.scoreTrendCanvasRef?.nativeElement;
    const trend = this.qualityOverview?.scoreTrend ?? [];

    if (!canvas || trend.length === 0) {
      this.destroyScoreTrendChart();
      return;
    }

    const primary = this.cssVariable('--bs-primary', '#27ae60');
    const primaryRgb = this.cssVariable('--bs-primary-rgb', '39, 174, 96');
    const { default: Chart } = await import('chart.js/auto');

    this.destroyScoreTrendChart();

    this.scoreTrendChart = new Chart(canvas, {
      type: 'line',
      data: {
        labels: trend.map((point) => this.formatDate(point.date)),
        datasets: [
          {
            data: trend.map((point) => this.normalizedScore(point.score)),
            borderColor: primary,
            backgroundColor: `rgba(${primaryRgb}, 0.12)`,
            borderWidth: 2,
            fill: true,
            pointBackgroundColor: primary,
            pointBorderColor: primary,
            pointHoverRadius: 5,
            pointRadius: 3,
            tension: 0.35,
          },
        ],
      },
      options: {
        maintainAspectRatio: false,
        plugins: {
          legend: {
            display: false,
          },
          tooltip: {
            callbacks: {
              title: (items) => this.trendTooltipTitle(trend, items[0]?.dataIndex ?? 0),
              label: (item) => `Score ${this.scoreLabel(Number(item.raw))}`,
            },
          },
        },
        scales: {
          x: {
            grid: {
              display: false,
            },
          },
          y: {
            beginAtZero: true,
            max: 10,
            ticks: {
              precision: 0,
              stepSize: 2,
            },
          },
        },
      },
    });
  }

  private destroyScoreTrendChart(): void {
    this.scoreTrendChart?.destroy();
    this.scoreTrendChart = null;
  }

  private trendTooltipTitle(trend: ProjectScoreTrendPoint[], index: number): string {
    const point = trend[index];

    if (!point) {
      return '';
    }

    return point.label
      ? `${point.label} · ${this.formatDate(point.date)}`
      : this.formatDate(point.date);
  }

  private cssVariable(name: string, fallback: string): string {
    if (typeof document === 'undefined') {
      return fallback;
    }

    return getComputedStyle(document.documentElement).getPropertyValue(name).trim() || fallback;
  }
}
