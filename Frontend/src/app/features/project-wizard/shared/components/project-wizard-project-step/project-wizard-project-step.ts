import { Location } from '@angular/common';
import { Component, effect } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProjectService } from '../../../../../shared/services/project';
import { StepNavigationResult } from '../../models/project-wizard-step.model';
import { ProjectWizardState } from '../../services/project-wizard-state';

@Component({
  selector: 'app-project-wizard-project-step',
  imports: [ReactiveFormsModule],
  templateUrl: './project-wizard-project-step.html',
  styleUrl: './project-wizard-project-step.scss',
})
export class ProjectWizardProjectStep {
  readonly form;

  submitAttempted = false;
  private patchedProjectId: string | null = null;

  constructor(
    private readonly location: Location,
    private readonly fb: FormBuilder,
    private readonly projectService: ProjectService,
    private readonly wizardState: ProjectWizardState,
  ) {
    this.form = this.fb.nonNullable.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
    });

    effect(() => {
      const currentProject = this.wizardState.project();

      if (!currentProject || currentProject.id === this.patchedProjectId || this.form.dirty) {
        return;
      }

      this.patchedProjectId = currentProject.id;
      this.form.patchValue({
        name: currentProject.name,
        description: currentProject.description,
      });
    });
  }

  async canGoNext(): Promise<StepNavigationResult> {
    this.form.markAllAsTouched();

    if (this.form.invalid) {
      return 'stay';
    }

    if (this.form.pristine && this.wizardState.project()) {
      return 'next-main-step';
    }

    const formValue = this.form.getRawValue();
    const currentProject = this.wizardState.project();

    if (currentProject) {
      await this.projectService.update({
        id: currentProject.id,
        ...formValue,
      });

      this.wizardState.setProject({
        ...currentProject,
        ...formValue,
      });
      this.form.markAsPristine();

      return 'next-main-step';
    }

    const project = await this.projectService.create(formValue);
    if (!project) return 'stay';

    this.wizardState.setProject(project);
    this.location.replaceState(`/projects/wizard/${project.id}`);
    this.form.markAsPristine();

    return 'next-main-step';
  }

  isInvalid(controlName: string): boolean {
    const control = this.form.get(controlName);
    return !!control && control.touched && control.invalid;
  }
}
