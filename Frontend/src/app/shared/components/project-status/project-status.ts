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
  spinning?: boolean;
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
        label: 'Refinement in Progress',
        className: 'text-bg-info',
        spinning: true,
      };
    }

    if (this.refinementStatus === RefinementStatus.Pending) {
      return {
        icon: faClock,
        label: 'Refinement Pending',
        className: 'text-bg-primary',
      };
    }

    if (this.refinementStatus === RefinementStatus.Failed) {
      return {
        icon: faCircleExclamation,
        label: 'Refinement Failed',
        className: 'text-bg-danger',
      };
    }

    if (this.refinementStatus === RefinementStatus.Completed) {
      return {
        icon: faCheckDouble,
        label: 'Refinement Completed',
        className: 'text-bg-success',
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
