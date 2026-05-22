import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import {
  ENTITY_COLLECTION_ICONS,
  ENTITY_ICONS,
} from '../../../../../shared/icons/entity-icons';

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
    project: ENTITY_ICONS.project,
    personas: ENTITY_COLLECTION_ICONS.personas,
    scenarios: ENTITY_COLLECTION_ICONS.scenarios,
    userStories: ENTITY_ICONS.userStory,
  };

  public getStepIcon(step: ProjectWizardNavigationStep): IconDefinition {
    return this.icons[step.key] ?? ENTITY_ICONS.project;
  }

  public isCompleted(index: number): boolean {
    return index < this.currentStepIndex;
  }

  public isConnectorPrimary(index: number): boolean {
    return index <= this.currentStepIndex;
  }
}
