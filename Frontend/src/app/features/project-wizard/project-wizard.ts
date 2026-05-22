import { Component, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProjectService } from '../../shared/services/project';
import { ProjectWizardNavigation } from './shared/components/project-wizard-navigation/project-wizard-navigation';
import { ProjectWizardPersonasStep } from './shared/components/project-wizard-personas-step/project-wizard-personas-step';
import { ProjectWizardProjectStep } from './shared/components/project-wizard-project-step/project-wizard-project-step';
import { ProjectWizardScenariosStep } from './shared/components/project-wizard-scenarios-step/project-wizard-scenarios-step';
import { ProjectWizardUserStoriesStep } from './shared/components/project-wizard-user-stories-step/project-wizard-user-stories-step';
import { ProjectWizardStep, StepNavigationResult } from './shared/models/project-wizard-step.model';
import { ProjectWizardLoader } from './shared/services/project-wizard-loader';
import { ProjectWizardState } from './shared/services/project-wizard-state';

@Component({
  selector: 'app-project-wizard',
  imports: [
    ProjectWizardNavigation,
    ProjectWizardProjectStep,
    ProjectWizardPersonasStep,
    ProjectWizardScenariosStep,
    ProjectWizardUserStoriesStep,
  ],
  templateUrl: './project-wizard.html',
  styleUrl: './project-wizard.scss',
})
export class ProjectWizard {
  @ViewChild(ProjectWizardProjectStep)
  private projectStep?: ProjectWizardProjectStep;

  @ViewChild(ProjectWizardPersonasStep)
  private personasStep?: ProjectWizardPersonasStep;

  @ViewChild(ProjectWizardScenariosStep)
  private scenariosStep?: ProjectWizardScenariosStep;

  @ViewChild(ProjectWizardUserStoriesStep)
  private userStoriesStep?: ProjectWizardUserStoriesStep;

  steps = [
    { key: 'project', label: 'Project' },
    { key: 'personas', label: 'Personas' },
    { key: 'scenarios', label: 'Scenarios' },
    { key: 'userStories', label: 'User Stories' },
  ] as const;

  currentStepIndex = 0;
  maxAllowedStepIndex = 0;
  initialPersonaIndex = 0;
  initialScenarioIndex = 0;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly wizardState: ProjectWizardState,
    private readonly wizardLoader: ProjectWizardLoader,
    private readonly projectService: ProjectService,
  ) {}

  async ngOnInit() {
    const projectId = this.route.snapshot.paramMap.get('projectId');

    if (!projectId) {
      this.wizardState.clear();
      return;
    }

    await this.wizardLoader.load(projectId);
    this.applyInitialNavigation();
  }

  get currentStep() {
    return this.steps[this.currentStepIndex];
  }

  get progress(): number {
    return ((this.currentStepIndex + 1) / this.steps.length) * 100;
  }

  get isLastStep(): boolean {
    if (this.currentStepIndex !== this.steps.length - 1) {
      return false;
    }

    if (this.currentStep.key !== 'userStories') {
      return true;
    }

    return this.userStoriesStep?.isLastScenario ?? this.wizardState.scenarios().length <= 1;
  }

  async goToStep(index: number): Promise<void> {
    if (index > this.maxAllowedStepIndex) {
      return;
    }

    if (index > this.currentStepIndex) {
      const result = await this.askCurrentStepToLeave();

      if (result !== 'next-main-step') {
        return;
      }
    }

    this.currentStepIndex = index;
  }

  async nextStep(): Promise<void> {
    if (this.isLastStep) {
      return;
    }

    const result = await this.askCurrentStepToLeave();

    if (result === 'stay' || result === 'handled-internally') {
      return;
    }

    this.moveNext();
  }

  previousStep(): void {
    if (this.currentStepIndex === 0) {
      return;
    }

    this.currentStepIndex--;
  }

  async finish(): Promise<void> {
    const result = await this.askCurrentStepToLeave();

    if (result === 'stay' || result === 'handled-internally') {
      return;
    }

    const projectId = this.wizardState.getProjectId();

    if (!projectId) {
      return;
    }

    await this.router.navigate(['/projects', projectId, 'board']);
  }

  private moveNext(): void {
    this.currentStepIndex++;

    this.maxAllowedStepIndex = Math.max(this.maxAllowedStepIndex, this.currentStepIndex);
  }

  private applyInitialNavigation(): void {
    const step = this.route.snapshot.queryParamMap.get('step');
    const personaIndex = Number(this.route.snapshot.queryParamMap.get('personaIndex') ?? 0);
    const scenarioIndex = Number(this.route.snapshot.queryParamMap.get('scenarioIndex') ?? 0);
    const stepIndex = this.steps.findIndex((item) => item.key === step);

    if (stepIndex < 0) {
      return;
    }

    this.initialPersonaIndex = Number.isNaN(personaIndex) ? 0 : personaIndex;
    this.initialScenarioIndex = Number.isNaN(scenarioIndex) ? 0 : scenarioIndex;
    this.currentStepIndex = stepIndex;
    this.maxAllowedStepIndex = Math.max(this.maxAllowedStepIndex, stepIndex);
  }

  private async askCurrentStepToLeave(): Promise<StepNavigationResult> {
    const currentStepComponent = this.currentStepComponent;

    if (!currentStepComponent) {
      return 'next-main-step';
    }

    return await currentStepComponent.canGoNext();
  }

  private get currentStepComponent(): ProjectWizardStep | null {
    switch (this.currentStep.key) {
      case 'project':
        return this.projectStep ?? null;
      case 'personas':
        return this.personasStep ?? null;
      case 'scenarios':
        return this.scenariosStep ?? null;
      case 'userStories':
        return this.userStoriesStep ?? null;
    }
  }
}
