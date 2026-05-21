import { Component, EventEmitter, Input, Output } from '@angular/core';

export interface ProjectWizardNavigationStep {
  key: string;
  label: string;
}

@Component({
  selector: 'app-project-wizard-navigation',
  imports: [],
  templateUrl: './project-wizard-navigation.html',
  styleUrl: './project-wizard-navigation.scss',
})
export class ProjectWizardNavigation {
  @Input({ required: true }) steps: readonly ProjectWizardNavigationStep[] = [];
  @Input({ required: true }) currentStepIndex = 0;
  @Input({ required: true }) maxAllowedStepIndex = 0;
  @Input({ required: true }) isLastStep = false;

  @Output() stepSelected = new EventEmitter<number>();
  @Output() previous = new EventEmitter<void>();
  @Output() next = new EventEmitter<void>();
  @Output() finished = new EventEmitter<void>();
}

