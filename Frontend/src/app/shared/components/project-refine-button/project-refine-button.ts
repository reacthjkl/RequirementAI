import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faCheckDouble,
  faCircleExclamation,
  faClock,
  faMagicWandSparkles,
  faSpinner,
  IconDefinition,
} from '@fortawesome/free-solid-svg-icons';
import { NgbModal, NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { Notification } from '../../../core/services/notification.service';
import { ProjectStatus } from '../../enums/project-status.enum';
import { RefinementStatus } from '../../enums/refinement-status.enum';
import { Project } from '../../models/project.model';
import { ProjectService } from '../../services/project';
import { ProjectRefineModal } from '../project-refine-modal/project-refine-modal';

interface RefinementStatusBadge {
  className: string;
  icon: IconDefinition;
  label: string;
  spinning?: boolean;
}

@Component({
  selector: 'app-project-refine-button',
  imports: [FontAwesomeModule, NgbTooltipModule],
  templateUrl: './project-refine-button.html',
  styleUrl: './project-refine-button.scss',
})
export class ProjectRefineButton {
  @Input({ required: true }) public project!: Project;
  @Output() public readonly refinementStarted = new EventEmitter<Project>();

  public refining = false;
  public readonly faMagicWandSparkles = faMagicWandSparkles;

  constructor(
    private readonly modalService: NgbModal,
    private readonly notification: Notification,
    private readonly projectService: ProjectService,
  ) {}

  public get canRefine(): boolean {
    return (
      this.project.status === ProjectStatus.Complete &&
      !this.isRefinementActive &&
      !this.refining
    );
  }

  public get isRefinementActive(): boolean {
    return (
      this.project.refinementStatus === RefinementStatus.Pending ||
      this.project.refinementStatus === RefinementStatus.InProcess
    );
  }

  public get buttonLabel(): string {
    if (this.refining) {
      return 'Refining...';
    }

    return 'Refine';
  }

  public get disabledTooltip(): string | null {
    if (this.project.status === ProjectStatus.Incomplete) {
      return 'Project is incomplete.';
    }

    return null;
  }

  public get badge(): RefinementStatusBadge | null {
    switch (this.project.refinementStatus) {
      case RefinementStatus.Pending:
        return {
          icon: faClock,
          label: 'Refinement Pending',
          className: 'text-bg-primary',
        };
      case RefinementStatus.InProcess:
        return {
          icon: faSpinner,
          label: 'Refinement in Progress',
          className: 'text-bg-info',
          spinning: true,
        };
      case RefinementStatus.Failed:
        return {
          icon: faCircleExclamation,
          label: 'Refinement Failed',
          className: 'text-bg-danger',
        };
      case RefinementStatus.Completed:
        return {
          icon: faCheckDouble,
          label: 'Refinement Completed',
          className: 'text-bg-success',
        };
      default:
        return null;
    }
  }

  public async openRefineModal(): Promise<void> {
    if (!this.canRefine) {
      return;
    }

    const modalRef = this.modalService.open(ProjectRefineModal, {
      centered: true,
      scrollable: true,
    });

    try {
      const customInstructions = (await modalRef.result) as string | null;
      await this.refine(customInstructions);
    } catch {
      return;
    }
  }

  private async refine(customInstructions: string | null): Promise<void> {
    this.refining = true;

    try {
      await this.projectService.refine(this.project.id, customInstructions);
      const project = {
        ...this.project,
        refinementStatus: RefinementStatus.Pending,
      };
      this.refinementStarted.emit(project);
      this.notification.success('Project refinement started');
    } catch {
      this.notification.fail('Could not start project refinement');
    } finally {
      this.refining = false;
    }
  }
}
