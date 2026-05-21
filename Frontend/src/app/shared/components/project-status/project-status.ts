import { Component, Input } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faCheckDouble,
  faCircleCheck,
  faCircleExclamation,
  faSpinner,
  IconDefinition,
} from '@fortawesome/free-solid-svg-icons';
import { ProjectStatus } from '../../enums/project-status.enum';

interface ProjectStatusViewModel {
  icon: IconDefinition;
  label: string;
  className: string;
}

@Component({
  selector: 'app-project-status',
  imports: [FontAwesomeModule],
  templateUrl: './project-status.html',
  styleUrl: './project-status.scss',
})
export class ProjectStatusComponent {
  @Input({ required: true }) status!: ProjectStatus | string;

  readonly projectStatus = ProjectStatus;

  get viewModel(): ProjectStatusViewModel {
    if (this.status == ProjectStatus.Incomplete) {
      return {
        icon: faCircleExclamation,
        label: 'Incomplete',
        className: 'text-bg-warning',
      };
    }

    if (this.status == ProjectStatus.ReadyForRefinement) {
      return {
        icon: faCircleCheck,
        label: 'Ready',
        className: 'text-bg-primary',
      };
    }

    if (this.status == ProjectStatus.RefinementInProgress) {
      return {
        icon: faSpinner,
        label: 'In refinement',
        className: 'text-bg-info',
      };
    }

    return {
      icon: faCheckDouble,
      label: 'Refined',
      className: 'text-bg-secondary',
    };
  }
}
