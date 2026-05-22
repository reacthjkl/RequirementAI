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
  @Input({ required: true }) public steps: readonly ProjectWizardNavigationStep[] = [];
  @Input({ required: true }) public currentStepIndex = 0;
  @Input({ required: true }) public maxAllowedStepIndex = 0;
  @Input({ required: true }) public isLastStep = false;

  @Output() public stepSelected = new EventEmitter<number>();
  @Output() public previous = new EventEmitter<void>();
  @Output() public next = new EventEmitter<void>();
  @Output() public finished = new EventEmitter<void>();

  public readonly icons: Record<string, IconDefinition> = {
    project: faFolderOpen,
    personas: faUsers,
    scenarios: faListCheck,
    userStories: faBookOpen,
  };

  public getStepIcon(step: ProjectWizardNavigationStep): IconDefinition {
    return this.icons[step.key] ?? faFolderOpen;
  }

  public isCompleted(index: number): boolean {
    return index < this.currentStepIndex;
  }

  public isConnectorPrimary(index: number): boolean {
    return index <= this.currentStepIndex;
  }
}
