import { Location } from '@angular/common';
import { Component, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProjectService } from '../../shared/services/project';
import { ProjectWizardPersonasStep } from './shared/components/project-wizard-personas-step/project-wizard-personas-step';
import { ProjectWizardProjectStep } from './shared/components/project-wizard-project-step/project-wizard-project-step';
import { ProjectWizardScenariosStep } from './shared/components/project-wizard-scenarios-step/project-wizard-scenarios-step';
import { ProjectWizardUserStoriesStep } from './shared/components/project-wizard-user-stories-step/project-wizard-user-stories-step';
import { ProjectWizardState } from './shared/services/project-wizard-state';

@Component({
  selector: 'app-project-wizard',
  imports: [
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

  steps = [
    { key: 'project', label: 'Project' },
    { key: 'personas', label: 'Personas' },
    { key: 'scenarios', label: 'Scenarios' },
    { key: 'userStories', label: 'User Stories' },
  ] as const;

  currentStepIndex = 0;
  maxAllowedStepIndex = 0;

  constructor(
    private readonly location: Location,
    private readonly route: ActivatedRoute,
    private readonly wizardState: ProjectWizardState,
    private readonly projectSvc: ProjectService,
  ) {}

  async ngOnInit() {
    const projectId = this.route.snapshot.paramMap.get('projectId');

    if (!projectId) {
      return;
    }

    const project = await this.projectSvc.getById(projectId);
    if (!project) return;

    this.wizardState.setProject(project);
  }

  get currentStep() {
    return this.steps[this.currentStepIndex];
  }

  get progress(): number {
    return ((this.currentStepIndex + 1) / this.steps.length) * 100;
  }

  get isLastStep(): boolean {
    return this.currentStepIndex === this.steps.length - 1;
  }

  goToStep(index: number): void {
    if (index > this.maxAllowedStepIndex) {
      return;
    }

    this.currentStepIndex = index;
  }

  async nextStep(): Promise<void> {
    if (this.isLastStep) {
      return;
    }

    if (this.currentStep.key === 'project') {
      const project = await this.projectStep?.persistChanges();

      if (!project) {
        return;
      }

      this.location.replaceState(`/projects/wizard/${project.id}`);
    }

    this.moveNext();
  }

  previousStep(): void {
    if (this.currentStepIndex === 0) {
      return;
    }

    this.currentStepIndex--;
  }

  finish(): void {
    console.log('Create project');
  }

  private moveNext(): void {
    this.currentStepIndex++;

    this.maxAllowedStepIndex = Math.max(this.maxAllowedStepIndex, this.currentStepIndex);
  }
}
