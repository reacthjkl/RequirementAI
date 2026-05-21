import { Component, effect } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Persona } from '../../../../../shared/models/persona.model';
import { PersonaService } from '../../../../../shared/services/persona';
import { StepNavigationResult } from '../../models/project-wizard-step.model';
import { ProjectWizardState } from '../../services/project-wizard-state';

type PersonaForm = FormGroup<{
  id: FormControl<string>;
  name: FormControl<string>;
  description: FormControl<string>;
  contextOfUse: FormControl<string>;
  goals: FormControl<string>;
  frustrations: FormControl<string>;
}>;

@Component({
  selector: 'app-project-wizard-personas-step',
  imports: [ReactiveFormsModule],
  templateUrl: './project-wizard-personas-step.html',
  styleUrl: './project-wizard-personas-step.scss',
})
export class ProjectWizardPersonasStep {
  readonly form;
  private syncedPersonaIds = '';

  constructor(
    private readonly fb: FormBuilder,
    private readonly personaService: PersonaService,
    private readonly wizardState: ProjectWizardState,
  ) {
    this.form = this.fb.nonNullable.group({
      personas: this.fb.array<PersonaForm>([]),
    });

    effect(() => {
      const personas = this.wizardState.personas();
      const personaIds = personas.map((persona) => persona.id).join('|');

      if (this.form.dirty && this.syncedPersonaIds !== '') {
        return;
      }

      if (personaIds === this.syncedPersonaIds) {
        return;
      }

      this.syncedPersonaIds = personaIds;
      this.rebuildForm(personas);
    });
  }

  get personaForms(): FormArray<PersonaForm> {
    return this.form.controls.personas;
  }

  addPersona(): void {
    this.personaForms.push(this.createPersonaForm());
    this.form.markAsDirty();
  }

  removePersona(index: number): void {
    this.personaForms.removeAt(index);
    this.form.markAsDirty();
  }

  async canGoNext(): Promise<StepNavigationResult> {
    this.form.markAllAsTouched();

    if (this.personaForms.length === 0 || this.form.invalid) {
      return 'stay';
    }

    const projectId = this.wizardState.getProjectId();

    if (!projectId) {
      return 'stay';
    }

    if (this.form.pristine) {
      return 'next-main-step';
    }

    const formPersonas = this.personaForms.getRawValue();
    const currentPersonas = this.wizardState.personas();
    const formPersonaIds = new Set(formPersonas.map((persona) => persona.id).filter(Boolean));
    const removedPersonas = currentPersonas.filter((persona) => !formPersonaIds.has(persona.id));

    await Promise.all(removedPersonas.map((persona) => this.personaService.delete(persona.id)));

    const savedPersonas: Persona[] = [];

    for (const persona of formPersonas) {
      if (persona.id) {
        await this.personaService.update(persona);
        savedPersonas.push({
          ...currentPersonas.find((currentPersona) => currentPersona.id === persona.id),
          ...persona,
          projectId,
          createdAt:
            currentPersonas.find((currentPersona) => currentPersona.id === persona.id)?.createdAt ??
            new Date().toISOString(),
        });
        continue;
      }

      const savedPersona = await this.personaService.create({
        name: persona.name,
        description: persona.description,
        contextOfUse: persona.contextOfUse,
        goals: persona.goals,
        frustrations: persona.frustrations,
        projectId,
      });

      if (!savedPersona) {
        return 'stay';
      }

      savedPersonas.push(savedPersona);
    }

    this.wizardState.setPersonas(savedPersonas);
    this.form.markAsPristine();

    return 'next-main-step';
  }

  isInvalid(index: number, controlName: keyof PersonaForm['controls']): boolean {
    const control = this.personaForms.at(index).controls[controlName];
    return control.touched && control.invalid;
  }

  private rebuildForm(personas: Persona[]): void {
    this.personaForms.clear();

    for (const persona of personas) {
      this.personaForms.push(this.createPersonaForm(persona));
    }

    if (this.personaForms.length === 0) {
      this.personaForms.push(this.createPersonaForm());
    }

    this.form.markAsPristine();
  }

  private createPersonaForm(persona?: Persona): PersonaForm {
    return this.fb.nonNullable.group({
      id: [persona?.id ?? ''],
      name: [persona?.name ?? '', Validators.required],
      description: [persona?.description ?? '', Validators.required],
      contextOfUse: [persona?.contextOfUse ?? '', Validators.required],
      goals: [persona?.goals ?? '', Validators.required],
      frustrations: [persona?.frustrations ?? '', Validators.required],
    });
  }
}
