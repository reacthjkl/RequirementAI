import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faBookOpen,
  faFolderOpen,
  faListCheck,
  faUsers,
  IconDefinition,
} from '@fortawesome/free-solid-svg-icons';

export interface ProjectWizardNavigationStep {
  key: string;
  label: string;
}

@Component({
  selector: 'app-project-wizard-navigation',
  imports: [FontAwesomeModule],
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

  readonly icons: Record<string, IconDefinition> = {
    project: faFolderOpen,
    personas: faUsers,
    scenarios: faListCheck,
    userStories: faBookOpen,
  };

  getStepIcon(step: ProjectWizardNavigationStep): IconDefinition {
    return this.icons[step.key] ?? faFolderOpen;
  }

  isCompleted(index: number): boolean {
    return index < this.currentStepIndex;
  }

  isConnectorPrimary(index: number): boolean {
    return index <= this.currentStepIndex;
  }
}
