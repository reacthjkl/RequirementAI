import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Project } from '../../../../../shared/models/project.model';
import { ProjectService } from '../../../../../shared/services/project';
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

  constructor(
    private readonly fb: FormBuilder,
    private readonly projectService: ProjectService,
    private readonly wizardState: ProjectWizardState,
  ) {
    this.form = this.fb.nonNullable.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
    });

    const currentProject = this.wizardState.project();

    if (currentProject) {
      this.form.patchValue({
        name: currentProject.name,
        description: currentProject.description,
      });
    }
  }

  async persistChanges(): Promise<Project | null> {
    this.form.markAllAsTouched();

    if (this.form.invalid) {
      return null;
    }

    const project = await this.projectService.create(this.form.getRawValue());
    if (!project) return null;

    this.wizardState.setProject(project);

    return project;
  }

  isInvalid(controlName: string): boolean {
    const control = this.form.get(controlName);
    return !!control && control.touched && control.invalid;
  }
}
