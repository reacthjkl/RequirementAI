import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
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
  }

  async persistChanges(): Promise<boolean> {
    this.form.markAllAsTouched();

    if (this.form.invalid) {
      return false;
    }

    const project = await this.projectService.create(this.form.getRawValue());
    if (!project) return false;

    this.wizardState.setProject(project);

    return true;
  }

  isInvalid(controlName: string): boolean {
    const control = this.form.get(controlName);
    return !!control && control.touched && control.invalid;
  }
}
