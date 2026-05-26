import { Component, ElementRef, OnDestroy, ViewChild } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faRotateRight } from '@fortawesome/free-solid-svg-icons';
import type { Chart as ChartInstance } from 'chart.js';
import { Notification } from '../../core/services/notification.service';
import { ProjectRefineButton } from '../../shared/components/project-refine-button/project-refine-button';
import { UserStoryStage } from '../../shared/enums/user-story-stage.enum';
import { ENTITY_COLLECTION_ICONS, ENTITY_ICONS } from '../../shared/icons/entity-icons';
import { LowestScoreItem } from '../../shared/models/lowest-score-item.model';
import { ProjectQualityOverview } from '../../shared/models/project-quality-overview.model';
import { ProjectScoreTrendPoint } from '../../shared/models/project-score-trend-point.model';
import { Project } from '../../shared/models/project.model';
import { PersonaService } from '../../shared/services/persona';
import { ProjectService } from '../../shared/services/project';
import { ScenarioService } from '../../shared/services/scenario';
import { UserStoryService } from '../../shared/services/user-story';

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
  public loading = true;
  public qualityLoading = false;
  public analyzing = false;

  public personaCount = 0;
  public scenarioCount = 0;
  public userStoryCount = 0;
  public closedUserStoryCount = 0;

  public readonly entityCollectionIcons = ENTITY_COLLECTION_ICONS;
  public readonly entityIcons = ENTITY_ICONS;
  public readonly faRotateRight = faRotateRight;

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

      await this.loadQualityOverview(projectId);
    } finally {
      this.loading = false;
    }
  }

  public ngOnDestroy(): void {
    this.destroyScoreTrendChart();
  }

  public get progressPercent(): number {
    if (this.userStoryCount === 0) {
      return 0;
    }

    return Math.round((this.closedUserStoryCount / this.userStoryCount) * 100);
  }

  public get hasQualityInfoForToday(): boolean {
    if (!this.qualityOverview) {
      return false;
    }

    const today = this.dateKey(new Date());

    return [
      ...this.qualityOverview.scoreTrend.map((point) => point.date),
      this.qualityOverview.lowestPersona?.evaluatedAt,
      this.qualityOverview.lowestScenario?.evaluatedAt,
      this.qualityOverview.lowestUserStory?.evaluatedAt,
    ].some((date) => !!date && this.dateKey(date) === today);
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

  public async analyzeProject(): Promise<void> {
    if (!this.project || this.analyzing || this.hasQualityInfoForToday) {
      return;
    }

    this.analyzing = true;

    try {
      await this.projectService.analyze(this.project.id);
      await this.loadQualityOverview(this.project.id);
      this.notification.success('Project analysis refreshed');
    } catch {
      this.notification.fail('Could not refresh project analysis');
    } finally {
      this.analyzing = false;
    }
  }

  public scorePercent(score: number): number {
    return Math.round((this.normalizedScore(score) / 10) * 100);
  }

  public scoreLabel(score: number): string {
    return `${this.normalizedScore(score).toFixed(1).replace(/\.0$/, '')}/10`;
  }

  public formatDate(value: string): string {
    return new Intl.DateTimeFormat(undefined, {
      day: '2-digit',
      month: 'short',
      year: 'numeric',
    }).format(new Date(value));
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
        labels: trend.map((point) => point.label || this.formatDate(point.date)),
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
