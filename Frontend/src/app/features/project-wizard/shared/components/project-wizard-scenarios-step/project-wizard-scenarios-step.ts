import { Component, effect, Input, OnChanges, SimpleChanges } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Persona } from '../../../../../shared/models/persona.model';
import { Scenario } from '../../../../../shared/models/scenario.model';
import { ScenarioService } from '../../../../../shared/services/scenario';
import { StepNavigationResult } from '../../models/project-wizard-step.model';
import { ProjectWizardState } from '../../services/project-wizard-state';

type ScenarioForm = FormGroup<{
  id: FormControl<string>;
  title: FormControl<string>;
  content: FormControl<string>;
}>;

@Component({
  selector: 'app-project-wizard-scenarios-step',
  imports: [ReactiveFormsModule],
  templateUrl: './project-wizard-scenarios-step.html',
  styleUrl: './project-wizard-scenarios-step.scss',
})
export class ProjectWizardScenariosStep implements OnChanges {
  @Input() public initialPersonaIndex = 0;

  public readonly form;
  public currentPersonaIndex = 0;
  private syncedKey = '';

  constructor(
    private readonly fb: FormBuilder,
    private readonly scenarioService: ScenarioService,
    public readonly wizardState: ProjectWizardState,
  ) {
    this.form = this.fb.nonNullable.group({
      scenarios: this.fb.array<ScenarioForm>([]),
    });

    effect(() => {
      const key = this.createSyncKey();

      if (this.form.dirty && this.syncedKey !== '') {
        return;
      }

      if (key === this.syncedKey) {
        return;
      }

      this.syncCurrentPersonaForm(key);
    });
  }

  public ngOnChanges(changes: SimpleChanges): void {
    if (!changes['initialPersonaIndex']) {
      return;
    }

    this.goToInitialPersona();
  }

  public get personas(): Persona[] {
    return this.wizardState.personas();
  }

  public get currentPersona(): Persona | null {
    return this.personas[this.currentPersonaIndex] ?? null;
  }

  public get scenarioForms(): FormArray<ScenarioForm> {
    return this.form.controls.scenarios;
  }

  public get currentPersonaScenarios(): Scenario[] {
    const persona = this.currentPersona;

    if (!persona) {
      return [];
    }

    return this.wizardState.scenarios().filter((scenario) => scenario.personaId === persona.id);
  }

  public addScenario(): void {
    this.scenarioForms.push(this.createScenarioForm());
    this.form.markAsDirty();
  }

  public removeScenario(index: number): void {
    this.scenarioForms.removeAt(index);
    this.form.markAsDirty();
  }

  public async canGoNext(): Promise<StepNavigationResult> {
    const saved = await this.saveCurrentPersonaScenarios();

    if (!saved) {
      return 'stay';
    }

    if (this.currentPersonaIndex < this.personas.length - 1) {
      this.currentPersonaIndex++;
      this.syncCurrentPersonaForm();
      return 'handled-internally';
    }

    return 'next-main-step';
  }

  public goToPersona(index: number): void {
    if (index === this.currentPersonaIndex || this.form.dirty) {
      return;
    }

    this.currentPersonaIndex = index;
    this.syncCurrentPersonaForm();
  }

  public isInvalid(index: number, controlName: keyof ScenarioForm['controls']): boolean {
    const control = this.scenarioForms.at(index).controls[controlName];
    return control.touched && control.invalid;
  }

  private async saveCurrentPersonaScenarios(): Promise<boolean> {
    this.form.markAllAsTouched();

    const persona = this.currentPersona;

    if (!persona) {
      return true;
    }

    if (this.scenarioForms.length === 0 || this.form.invalid) {
      return false;
    }

    if (this.form.pristine) {
      return true;
    }

    const formScenarios = this.scenarioForms.getRawValue();
    const allScenarios = this.wizardState.scenarios();
    const existingScenarios = allScenarios.filter((scenario) => scenario.personaId === persona.id);
    const formScenarioIds = new Set(formScenarios.map((scenario) => scenario.id).filter(Boolean));
    const removedScenarios = existingScenarios.filter(
      (scenario) => !formScenarioIds.has(scenario.id),
    );

    await Promise.all(removedScenarios.map((scenario) => this.scenarioService.delete(scenario.id)));

    const savedScenarios: Scenario[] = [];

    for (const scenario of formScenarios) {
      if (scenario.id) {
        await this.scenarioService.update(scenario);
        savedScenarios.push({
          ...existingScenarios.find((existingScenario) => existingScenario.id === scenario.id),
          ...scenario,
          personaId: persona.id,
          createdAt:
            existingScenarios.find((existingScenario) => existingScenario.id === scenario.id)
              ?.createdAt ?? new Date().toISOString(),
        });
        continue;
      }

      const savedScenario = await this.scenarioService.create({
        title: scenario.title,
        content: scenario.content,
        personaId: persona.id,
      });

      if (!savedScenario) {
        return false;
      }

      savedScenarios.push({
        ...savedScenario,
        personaId: persona.id,
      });
    }

    this.wizardState.setScenarios([
      ...allScenarios.filter((scenario) => scenario.personaId !== persona.id),
      ...savedScenarios,
    ]);
    this.form.markAsPristine();
    this.syncedKey = this.createSyncKey();

    return true;
  }

  private createSyncKey(): string {
    const persona = this.currentPersona;
    const personaIds = this.personas.map((item) => item.id).join('|');
    const scenarioIds = this.currentPersonaScenarios.map((scenario) => scenario.id).join('|');

    return `${persona?.id ?? ''}:${personaIds}:${scenarioIds}`;
  }

  private syncCurrentPersonaForm(key = this.createSyncKey()): void {
    this.syncedKey = key;
    this.rebuildForm(this.currentPersonaScenarios);
  }

  private goToInitialPersona(): void {
    if (this.form.dirty) {
      return;
    }

    const lastPersonaIndex = Math.max(this.personas.length - 1, 0);
    this.currentPersonaIndex = Math.min(Math.max(this.initialPersonaIndex, 0), lastPersonaIndex);
    this.syncCurrentPersonaForm();
  }

  private rebuildForm(scenarios: Scenario[]): void {
    this.scenarioForms.clear();

    for (const scenario of scenarios) {
      this.scenarioForms.push(this.createScenarioForm(scenario));
    }

    if (this.currentPersona && this.scenarioForms.length === 0) {
      this.scenarioForms.push(this.createScenarioForm());
    }

    this.form.markAsPristine();
  }

  private createScenarioForm(scenario?: Scenario): ScenarioForm {
    return this.fb.nonNullable.group({
      id: [scenario?.id ?? ''],
      title: [scenario?.title ?? '', Validators.required],
      content: [scenario?.content ?? '', Validators.required],
    });
  }
}
