import { Component } from '@angular/core';
import { ProjectWizardPersonasStep } from './project-wizard-personas-step/project-wizard-personas-step';
import { ProjectWizardProjectStep } from './project-wizard-project-step/project-wizard-project-step';
import { ProjectWizardScenariosStep } from './project-wizard-scenarios-step/project-wizard-scenarios-step';
import { ProjectWizardUserStoriesStep } from './project-wizard-user-stories-step/project-wizard-user-stories-step';

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

  nextStep(): void {
    if (this.isLastStep) {
      return;
    }

    this.currentStepIndex++;
    this.maxAllowedStepIndex = Math.max(this.maxAllowedStepIndex, this.currentStepIndex);
  }

  previousStep(): void {
    if (this.currentStepIndex === 0) {
      return;
    }

    this.currentStepIndex--;
  }

  finish(): void {
    // later: collect wizard data and call API
    console.log('Create project');
  }
}
