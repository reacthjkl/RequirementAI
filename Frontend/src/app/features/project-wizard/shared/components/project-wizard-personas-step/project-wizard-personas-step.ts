import { Component, effect } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faCheck, faPen, faTrash } from '@fortawesome/free-solid-svg-icons';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationModal } from '../../../../../shared/components/confirmation-modal/confirmation-modal';
import { ENTITY_ICONS } from '../../../../../shared/icons/entity-icons';
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
  imports: [ReactiveFormsModule, FontAwesomeModule],
  templateUrl: './project-wizard-personas-step.html',
  styleUrl: './project-wizard-personas-step.scss',
})
export class ProjectWizardPersonasStep {
  public readonly form;
  public readonly faCheck = faCheck;
  public readonly faPen = faPen;
  public readonly faTrash = faTrash;
  public readonly entityIcons = ENTITY_ICONS;
  public editingPersonaIndex: number | null = null;
  private syncedPersonaIds = '';

  constructor(
    private readonly fb: FormBuilder,
    private readonly modalService: NgbModal,
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

  public get personaForms(): FormArray<PersonaForm> {
    return this.form.controls.personas;
  }

  public addPersona(): void {
    if (!this.closeCurrentPersonaForm()) {
      return;
    }

    this.personaForms.push(this.createPersonaForm());
    this.editingPersonaIndex = this.personaForms.length - 1;
    this.form.markAsDirty();
  }

  public editPersona(index: number): void {
    if (index === this.editingPersonaIndex) {
      return;
    }

    if (!this.closeCurrentPersonaForm()) {
      return;
    }

    this.editingPersonaIndex = index;
  }

  public doneEditingPersona(): void {
    this.closeCurrentPersonaForm();
  }

  public async removePersona(index: number): Promise<void> {
    const confirmed = await this.confirmRemovePersona(index);

    if (!confirmed) {
      return;
    }

    this.personaForms.removeAt(index);
    this.updateEditingIndexAfterRemove(index);
    this.form.markAsDirty();
  }

  public async canGoNext(): Promise<StepNavigationResult> {
    this.form.markAllAsTouched();

    if (this.personaForms.length === 0 || this.form.invalid) {
      this.editingPersonaIndex = this.findFirstInvalidPersonaIndex();
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
    this.editingPersonaIndex = null;

    return 'next-main-step';
  }

  public isInvalid(index: number, controlName: keyof PersonaForm['controls']): boolean {
    const control = this.personaForms.at(index).controls[controlName];
    return control.touched && control.invalid;
  }

  private rebuildForm(personas: Persona[]): void {
    this.personaForms.clear();

    for (const persona of personas) {
      this.personaForms.push(this.createPersonaForm(persona));
    }

    this.editingPersonaIndex = this.personaForms.length === 0 ? null : null;
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

  private closeCurrentPersonaForm(): boolean {
    if (this.editingPersonaIndex === null) {
      return true;
    }

    const personaForm = this.personaForms.at(this.editingPersonaIndex);
    personaForm.markAllAsTouched();

    if (personaForm.invalid) {
      return false;
    }

    this.editingPersonaIndex = null;
    return true;
  }

  private async confirmRemovePersona(index: number): Promise<boolean> {
    const modalRef = this.modalService.open(ConfirmationModal, { centered: true });
    const personaName = this.personaForms.at(index).controls.name.value || `Persona ${index + 1}`;

    modalRef.componentInstance.title = 'Remove persona';
    modalRef.componentInstance.message = `Remove "${personaName}"? This change will be saved when you continue.`;
    modalRef.componentInstance.confirmText = 'Remove';
    modalRef.componentInstance.confirmButtonClass = 'btn-danger';

    try {
      return await modalRef.result;
    } catch {
      return false;
    }
  }

  private updateEditingIndexAfterRemove(removedIndex: number): void {
    if (this.editingPersonaIndex === null) {
      return;
    }

    if (this.editingPersonaIndex === removedIndex) {
      this.editingPersonaIndex = null;
      return;
    }

    if (this.editingPersonaIndex > removedIndex) {
      this.editingPersonaIndex--;
    }
  }

  private findFirstInvalidPersonaIndex(): number | null {
    const invalidIndex = this.personaForms.controls.findIndex((personaForm) => personaForm.invalid);

    return invalidIndex >= 0 ? invalidIndex : null;
  }
}
