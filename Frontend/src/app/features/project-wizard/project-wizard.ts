import { Component, ViewChild } from '@angular/core';
import { ProjectWizardPersonasStep } from './shared/components/project-wizard-personas-step/project-wizard-personas-step';
import { ProjectWizardProjectStep } from './shared/components/project-wizard-project-step/project-wizard-project-step';
import { ProjectWizardScenariosStep } from './shared/components/project-wizard-scenarios-step/project-wizard-scenarios-step';
import { ProjectWizardUserStoriesStep } from './shared/components/project-wizard-user-stories-step/project-wizard-user-stories-step';

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
      const saved = await this.projectStep?.persistChanges();

      if (!saved) {
        return;
      }
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
