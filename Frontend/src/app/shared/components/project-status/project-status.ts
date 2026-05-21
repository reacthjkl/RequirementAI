import { Component, Input } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faCheckDouble,
  faCircleCheck,
  faCircleExclamation,
  faSpinner,
  IconDefinition,
} from '@fortawesome/free-solid-svg-icons';
import { ProjectStatus as ProjectStatusValue } from '../../enums/project-status.enum';

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
  @Input({ required: true }) status!: ProjectStatusValue | string;

  readonly projectStatus = ProjectStatusValue;

  get viewModel(): ProjectStatusViewModel {
    if (this.matchesStatus(ProjectStatusValue.Incomplete, 'incomplete')) {
      return {
        icon: faCircleExclamation,
        label: 'Incomplete',
        className: 'text-bg-warning',
      };
    }

    if (this.matchesStatus(ProjectStatusValue.ReadyForRefinement, 'readyforrefinement')) {
      return {
        icon: faCircleCheck,
        label: 'Ready',
        className: 'text-bg-primary',
      };
    }

    if (this.matchesStatus(ProjectStatusValue.RefinementInProgress, 'refinementinprogress')) {
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

  private matchesStatus(enumValue: ProjectStatusValue, stringValue: string): boolean {
    return this.status === enumValue || String(this.status).toLowerCase() === stringValue;
  }
}
