import { ChangeDetectorRef, Component, Input, Optional } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faCheck, faPlus, faTimes, faTrash } from '@fortawesome/free-solid-svg-icons';
import { NgbActiveModal, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { Notification } from '../../core/services/notification.service';
import { USER_STORY_STAGE_META } from '../../shared/constants/user-story-stage-meta';
import { UserStoryStage } from '../../shared/enums/user-story-stage.enum';
import {
  UserStoryFormFields,
  UserStoryFormGroup,
} from '../../shared/components/user-story-form-fields/user-story-form-fields';
import { ENTITY_ICONS } from '../../shared/icons/entity-icons';
import { AcceptanceCriteria } from '../../shared/models/acceptance-criteria.model';
import { EdgeCase } from '../../shared/models/edge-case.model';
import { Persona } from '../../shared/models/persona.model';
import { Scenario } from '../../shared/models/scenario.model';
import { UserStory } from '../../shared/models/user-story.model';
import { AcceptanceCriteriaService } from '../../shared/services/acceptance-criteria';
import { EdgeCaseService } from '../../shared/services/edge-case';
import { PersonaService } from '../../shared/services/persona';
import { ScenarioService } from '../../shared/services/scenario';
import { UserStoryService } from '../../shared/services/user-story';

type AcceptanceCriteriaForm = FormGroup<{
  id: FormControl<string>;
  wording: FormControl<string>;
}>;

type EdgeCaseForm = FormGroup<{
  id: FormControl<string>;
  preconditions: FormControl<string>;
  triggerAction: FormControl<string>;
  expectedBehavior: FormControl<string>;
}>;

interface ScenarioOption {
  persona: Persona;
  scenario: Scenario;
}

@Component({
  selector: 'app-project-user-story-editor',
  imports: [ReactiveFormsModule, FontAwesomeModule, NgbDropdownModule, UserStoryFormFields],
  templateUrl: './project-user-story-editor.html',
  styleUrl: './project-user-story-editor.scss',
})
export class ProjectUserStoryEditor {
  @Input() public projectId: string | null = null;
  @Input() public userStoryId: string | null = null;

  public readonly form;
  public readonly personaControl = new FormControl('', {
    nonNullable: true,
    validators: [Validators.required],
  });
  public readonly scenarioControl = new FormControl('', {
    nonNullable: true,
    validators: [Validators.required],
  });
  public readonly stageControl = new FormControl(UserStoryStage.New, {
    nonNullable: true,
    validators: [Validators.required],
  });

  public userStory: UserStory | null = null;
  public scenarioOptions: ScenarioOption[] = [];
  public loading = true;
  public saving = false;
  public loadFailed = false;
  public showScenarioDetails = false;

  public readonly entityIcons = ENTITY_ICONS;
  public readonly faCheck = faCheck;
  public readonly faPlus = faPlus;
  public readonly faTimes = faTimes;
  public readonly faTrash = faTrash;
  public readonly stageOptions = USER_STORY_STAGE_META;

  private initialAcceptanceCriteria: AcceptanceCriteria[] = [];
  private initialEdgeCases: EdgeCase[] = [];

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    @Optional() private readonly activeModal: NgbActiveModal | null,
    private readonly cdr: ChangeDetectorRef,
    private readonly fb: FormBuilder,
    private readonly personaService: PersonaService,
    private readonly scenarioService: ScenarioService,
    private readonly userStoryService: UserStoryService,
    private readonly acceptanceCriteriaService: AcceptanceCriteriaService,
    private readonly edgeCaseService: EdgeCaseService,
    private readonly notification: Notification,
  ) {
    this.form = this.fb.nonNullable.group({
      id: [''],
      title: ['', Validators.required],
      description: ['', Validators.required],
      acceptanceCriteria: this.fb.array<AcceptanceCriteriaForm>([]),
      edgeCases: this.fb.array<EdgeCaseForm>([]),
    });
  }

  public async ngOnInit(): Promise<void> {
    this.projectId ??= this.route.snapshot.paramMap.get('projectId');
    this.userStoryId ??= this.route.snapshot.paramMap.get('userStoryId');

    if (!this.projectId) {
      this.loading = false;
      return;
    }

    try {
      const [scenarioOptions, userStory] = await Promise.all([
        this.loadScenarioOptions(this.projectId),
        this.userStoryId ? this.userStoryService.getById(this.userStoryId) : Promise.resolve(null),
      ]);

      this.scenarioOptions = scenarioOptions;
      this.userStory = userStory;

      if (userStory) {
        this.patchUserStory(userStory);
        await this.loadChildRecords(userStory.id);
      } else {
        const initialScenarioId = this.route.snapshot.queryParamMap.get('scenarioId');
        const fallbackScenarioId = this.scenarioOptions[0]?.scenario.id ?? '';
        this.scenarioControl.setValue(initialScenarioId ?? fallbackScenarioId);
        this.syncPersonaFromScenario();
      }
    } catch {
      this.loadFailed = true;
      this.notification.fail('Could not load User Story');
    } finally {
      this.form.markAsPristine();
      this.personaControl.markAsPristine();
      this.scenarioControl.markAsPristine();
      this.stageControl.markAsPristine();
      this.finishLoading();
    }
  }

  public get isEditing(): boolean {
    return !!this.userStoryId;
  }

  public get title(): string {
    return this.isEditing ? 'Edit User Story' : 'Add User Story';
  }

  public get acceptanceCriteriaForms(): FormArray<AcceptanceCriteriaForm> {
    return this.form.controls.acceptanceCriteria;
  }

  public get edgeCaseForms(): FormArray<EdgeCaseForm> {
    return this.form.controls.edgeCases;
  }

  public get userStoryFieldsForm(): UserStoryFormGroup {
    return this.form as unknown as UserStoryFormGroup;
  }

  public get personaOptions(): Persona[] {
    const personas = new Map<string, Persona>();

    for (const option of this.scenarioOptions) {
      personas.set(option.persona.id, option.persona);
    }

    return [...personas.values()];
  }

  public get filteredScenarioOptions(): ScenarioOption[] {
    return this.scenarioOptions.filter((option) => option.persona.id === this.personaControl.value);
  }

  public get currentScenarioOption(): ScenarioOption | undefined {
    return this.scenarioOptions.find((option) => option.scenario.id === this.scenarioControl.value);
  }

  public onPersonaChange(): void {
    const firstScenario = this.filteredScenarioOptions[0];
    this.scenarioControl.setValue(firstScenario?.scenario.id ?? '');
    this.scenarioControl.markAsDirty();
    this.showScenarioDetails = false;
  }

  public toggleScenarioDetails(): void {
    this.showScenarioDetails = !this.showScenarioDetails;
  }

  public selectStage(stage: UserStoryStage): void {
    this.stageControl.setValue(stage);
    this.stageControl.markAsDirty();
    this.stageControl.markAsTouched();
  }

  public addAcceptanceCriteria(): void {
    this.acceptanceCriteriaForms.push(this.createAcceptanceCriteriaForm());
    this.form.markAsDirty();
  }

  public removeAcceptanceCriteria(index: number): void {
    this.acceptanceCriteriaForms.removeAt(index);
    this.form.markAsDirty();
  }

  public addEdgeCase(): void {
    this.edgeCaseForms.push(this.createEdgeCaseForm());
    this.form.markAsDirty();
  }

  public removeEdgeCase(index: number): void {
    this.edgeCaseForms.removeAt(index);
    this.form.markAsDirty();
  }

  public async save(): Promise<void> {
    this.form.markAllAsTouched();
    this.personaControl.markAsTouched();
    this.scenarioControl.markAsTouched();
    this.stageControl.markAsTouched();

    if (
      !this.projectId ||
      this.form.invalid ||
      this.personaControl.invalid ||
      this.scenarioControl.invalid ||
      this.stageControl.invalid ||
      this.saving
    ) {
      return;
    }

    this.saving = true;

    try {
      const userStory = await this.saveUserStory();

      if (!userStory) {
        this.notification.fail('Could not save User Story');
        return;
      }

      await Promise.all([
        this.saveAcceptanceCriteria(userStory.id),
        this.saveEdgeCases(userStory.id),
      ]);
      this.notification.success(this.isEditing ? 'User Story updated' : 'User Story created');
      this.closeAfterSave();
    } catch {
      this.notification.fail('Could not save User Story');
    } finally {
      this.saving = false;
    }
  }

  public async cancel(): Promise<void> {
    if (this.activeModal) {
      this.activeModal.dismiss();
      return;
    }

    if (!this.projectId) {
      return;
    }

    await this.router.navigate(['/projects', this.projectId, 'board']);
  }

  public isScenarioInvalid(): boolean {
    return this.scenarioControl.touched && this.scenarioControl.invalid;
  }

  public isPersonaInvalid(): boolean {
    return this.personaControl.touched && this.personaControl.invalid;
  }

  public isStageInvalid(): boolean {
    return this.stageControl.touched && this.stageControl.invalid;
  }

  public get selectedStageMeta() {
    return this.stageOptions.find((stage) => stage.value === this.stageControl.value);
  }

  public isAcceptanceCriteriaInvalid(index: number): boolean {
    const control = this.acceptanceCriteriaForms.at(index).controls.wording;
    return control.touched && control.invalid;
  }

  public isEdgeCaseInvalid(index: number, controlName: keyof EdgeCaseForm['controls']): boolean {
    const control = this.edgeCaseForms.at(index).controls[controlName];
    return control.touched && control.invalid;
  }

  private async loadScenarioOptions(projectId: string): Promise<ScenarioOption[]> {
    const personas = await this.personaService.getByProjectId(projectId);
    const scenarioGroups = await Promise.all(
      personas.map(async (persona) => {
        const scenarios = await this.scenarioService.getByPersonaId(persona.id);

        return scenarios.map((scenario) => ({
          persona,
          scenario,
        }));
      }),
    );

    return scenarioGroups.flat();
  }

  private patchUserStory(userStory: UserStory): void {
    this.form.patchValue({
      id: userStory.id,
      title: userStory.title,
      description: userStory.description,
    });
    this.scenarioControl.setValue(userStory.scenarioId);
    this.stageControl.setValue(userStory.stage);

    const scenarioOption = this.scenarioOptions.find(
      (option) => option.scenario.id === userStory.scenarioId,
    );
    this.personaControl.setValue(scenarioOption?.persona.id ?? '');
  }

  private async loadChildRecords(userStoryId: string): Promise<void> {
    try {
      const [acceptanceCriteria, edgeCases] = await Promise.all([
        this.acceptanceCriteriaService.getByUserStoryId(userStoryId),
        this.edgeCaseService.getByUserStoryId(userStoryId),
      ]);

      this.initialAcceptanceCriteria = acceptanceCriteria;
      this.initialEdgeCases = edgeCases;
      this.rebuildAcceptanceCriteria(acceptanceCriteria);
      this.rebuildEdgeCases(edgeCases);
    } catch {
      this.notification.fail('Could not load Acceptance Criteria or Edge Cases');
    }
  }

  private syncPersonaFromScenario(): void {
    const scenarioOption = this.currentScenarioOption;
    this.personaControl.setValue(scenarioOption?.persona.id ?? this.personaOptions[0]?.id ?? '');

    if (!this.scenarioControl.value && this.filteredScenarioOptions.length > 0) {
      this.scenarioControl.setValue(this.filteredScenarioOptions[0].scenario.id);
    }
  }

  private closeAfterSave(): void {
    if (this.activeModal) {
      this.activeModal.close(true);
      return;
    }

    void this.router.navigate(['/projects', this.projectId, 'board']);
  }

  private finishLoading(): void {
    setTimeout(() => {
      this.loading = false;
      this.cdr.markForCheck();
    });
  }

  private async saveUserStory(): Promise<UserStory | null> {
    const value = this.form.getRawValue();
    const trimmed = {
      title: value.title.trim(),
      description: value.description.trim(),
    };

    if (this.userStoryId && this.userStory) {
      await this.userStoryService.update({
        id: this.userStoryId,
        title: trimmed.title,
        description: trimmed.description,
        stage: this.stageControl.value,
      });

      return {
        ...this.userStory,
        ...trimmed,
        stage: this.stageControl.value,
      };
    }

    return await this.userStoryService.create({
      ...trimmed,
      scenarioId: this.scenarioControl.value,
    });
  }

  private async saveAcceptanceCriteria(userStoryId: string): Promise<void> {
    const formItems = this.acceptanceCriteriaForms.getRawValue();
    const formIds = new Set(formItems.map((item) => item.id).filter(Boolean));
    const removed = this.initialAcceptanceCriteria.filter((item) => !formIds.has(item.id));

    await Promise.all(removed.map((item) => this.acceptanceCriteriaService.delete(item.id)));

    for (const item of formItems) {
      const wording = item.wording.trim();

      if (item.id) {
        await this.acceptanceCriteriaService.update({
          id: item.id,
          wording,
        });
      } else {
        await this.acceptanceCriteriaService.create({
          wording,
          userStoryId,
        });
      }
    }
  }

  private async saveEdgeCases(userStoryId: string): Promise<void> {
    const formItems = this.edgeCaseForms.getRawValue();
    const formIds = new Set(formItems.map((item) => item.id).filter(Boolean));
    const removed = this.initialEdgeCases.filter((item) => !formIds.has(item.id));

    await Promise.all(removed.map((item) => this.edgeCaseService.delete(item.id)));

    for (const item of formItems) {
      const update = {
        preconditions: item.preconditions.trim(),
        triggerAction: item.triggerAction.trim(),
        expectedBehavior: item.expectedBehavior.trim(),
      };

      if (item.id) {
        await this.edgeCaseService.update({
          id: item.id,
          ...update,
        });
      } else {
        await this.edgeCaseService.create({
          ...update,
          userStoryId,
        });
      }
    }
  }

  private rebuildAcceptanceCriteria(items: AcceptanceCriteria[]): void {
    this.acceptanceCriteriaForms.clear();

    for (const item of items) {
      this.acceptanceCriteriaForms.push(this.createAcceptanceCriteriaForm(item));
    }
  }

  private rebuildEdgeCases(items: EdgeCase[]): void {
    this.edgeCaseForms.clear();

    for (const item of items) {
      this.edgeCaseForms.push(this.createEdgeCaseForm(item));
    }
  }

  private createAcceptanceCriteriaForm(item?: AcceptanceCriteria): AcceptanceCriteriaForm {
    return this.fb.nonNullable.group({
      id: [item?.id ?? ''],
      wording: [item?.wording ?? '', Validators.required],
    });
  }

  private createEdgeCaseForm(item?: EdgeCase): EdgeCaseForm {
    return this.fb.nonNullable.group({
      id: [item?.id ?? ''],
      preconditions: [item?.preconditions ?? '', Validators.required],
      triggerAction: [item?.triggerAction ?? '', Validators.required],
      expectedBehavior: [item?.expectedBehavior ?? '', Validators.required],
    });
  }
}
