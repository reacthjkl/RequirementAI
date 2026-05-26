import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faFloppyDisk, faTrash } from '@fortawesome/free-solid-svg-icons';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Notification } from '../../core/services/notification.service';
import { ConfirmationModal } from '../../shared/components/confirmation-modal/confirmation-modal';
import { Project } from '../../shared/models/project.model';
import { ProjectService } from '../../shared/services/project';

@Component({
  selector: 'app-project-settings',
  imports: [ReactiveFormsModule, FontAwesomeModule],
  templateUrl: './project-settings.html',
  styleUrl: './project-settings.scss',
})
export class ProjectSettings {
  public readonly form;

  public project: Project | null = null;
  public loading = true;
  public saving = false;
  public deleting = false;

  public readonly faFloppyDisk = faFloppyDisk;
  public readonly faTrash = faTrash;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly fb: FormBuilder,
    private readonly modalService: NgbModal,
    private readonly projectService: ProjectService,
    private readonly notification: Notification,
  ) {
    this.form = this.fb.nonNullable.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
    });
  }

  public async ngOnInit(): Promise<void> {
    const projectId = this.route.snapshot.paramMap.get('projectId');

    if (!projectId) {
      this.loading = false;
      return;
    }

    try {
      this.project = await this.projectService.getById(projectId);

      if (this.project) {
        this.form.patchValue({
          name: this.project.name,
          description: this.project.description,
        });
        this.form.markAsPristine();
      }
    } finally {
      this.loading = false;
    }
  }

  public async saveProject(): Promise<void> {
    this.form.markAllAsTouched();

    if (!this.project || this.form.invalid || this.saving) {
      return;
    }

    const formValue = this.form.getRawValue();
    const update = {
      id: this.project.id,
      name: formValue.name.trim(),
      description: formValue.description.trim(),
    };

    if (!update.name || !update.description) {
      this.form.controls.name.setValue(update.name);
      this.form.controls.description.setValue(update.description);
      return;
    }

    this.saving = true;

    try {
      await this.projectService.update(update);
      this.project = {
        ...this.project,
        ...update,
      };
      this.form.patchValue({
        name: update.name,
        description: update.description,
      });
      this.form.markAsPristine();
      this.notification.success('Project settings saved');
    } catch {
      this.notification.fail('Could not save project settings');
    } finally {
      this.saving = false;
    }
  }

  public async deleteProject(): Promise<void> {
    if (!this.project || this.deleting) {
      return;
    }

    const confirmed = await this.confirmDeleteProject(this.project);

    if (!confirmed) {
      return;
    }

    this.deleting = true;

    try {
      await this.projectService.delete(this.project.id);
      this.notification.success('Project deleted');
      await this.router.navigate(['/projects']);
    } catch {
      this.notification.fail('Could not delete project');
    } finally {
      this.deleting = false;
    }
  }

  public isInvalid(controlName: 'name' | 'description'): boolean {
    const control = this.form.controls[controlName];
    return control.touched && control.invalid;
  }

  private async confirmDeleteProject(project: Project): Promise<boolean> {
    const modalRef = this.modalService.open(ConfirmationModal, { centered: true });

    modalRef.componentInstance.title = 'Delete project';
    modalRef.componentInstance.message = `Delete "${project.name}" and all related project data?`;
    modalRef.componentInstance.confirmText = 'Delete project';
    modalRef.componentInstance.confirmButtonClass = 'btn-danger';
    modalRef.componentInstance.requiredConfirmationText = project.name;
    modalRef.componentInstance.confirmationLabel = 'Project name';

    try {
      return await modalRef.result;
    } catch {
      return false;
    }
  }
}
