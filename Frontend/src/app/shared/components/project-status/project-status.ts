import { Component, Input } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faCheckDouble,
  faCircleCheck,
  faCircleExclamation,
  faClock,
  faSpinner,
  IconDefinition,
} from '@fortawesome/free-solid-svg-icons';
import { ProjectStatus } from '../../enums/project-status.enum';
import { RefinementStatus } from './../../enums/refinement-status.enum';

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
  @Input({ required: true }) public status!: ProjectStatus;
  @Input({ required: true }) public refinementStatus!: RefinementStatus;

  public readonly projectStatus = ProjectStatus;

  public get viewModel(): ProjectStatusViewModel {
    if (this.refinementStatus === RefinementStatus.InProcess) {
      return {
        icon: faSpinner,
        label: 'In refinement',
        className: 'text-bg-info',
      };
    }

    if (this.refinementStatus === RefinementStatus.Pending) {
      return {
        icon: faClock,
        label: 'Refinement pending',
        className: 'text-bg-primary',
      };
    }

    if (this.refinementStatus === RefinementStatus.Failed) {
      return {
        icon: faCircleExclamation,
        label: 'Refinement failed',
        className: 'text-bg-danger',
      };
    }

    if (this.refinementStatus === RefinementStatus.Completed) {
      return {
        icon: faCheckDouble,
        label: 'Refined',
        className: 'text-bg-secondary',
      };
    }

    if (this.status === ProjectStatus.Incomplete) {
      return {
        icon: faCircleExclamation,
        label: 'Incomplete',
        className: 'text-bg-warning',
      };
    }

    return {
      icon: faCircleCheck,
      label: 'Ready',
      className: 'text-bg-success',
    };
  }
}
