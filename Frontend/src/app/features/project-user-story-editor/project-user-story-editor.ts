import {
  AfterViewChecked,
  ChangeDetectorRef,
  Component,
  ElementRef,
  Input,
  OnDestroy,
  Optional,
  ViewChild,
} from '@angular/core';
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
import { faCheck, faPlus } from '@fortawesome/free-solid-svg-icons';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import type { Api } from 'datatables.net-bs5';
import { Notification } from '../../core/services/notification.service';
import {
  MetaDropdown,
  MetaDropdownOption,
  MetaDropdownValue,
} from '../../shared/components/meta-dropdown/meta-dropdown';
import { QualityScorePanel } from '../../shared/components/quality-score-panel/quality-score-panel';
import {
  UserStoryFormFields,
  UserStoryFormGroup,
} from '../../shared/components/user-story-form-fields/user-story-form-fields';
import { USER_STORY_STAGE_META } from '../../shared/constants/user-story-stage-meta';
import { UserStoryStage } from '../../shared/enums/user-story-stage.enum';
import { ENTITY_ICONS } from '../../shared/icons/entity-icons';
import { AcceptanceCriteria } from '../../shared/models/acceptance-criteria.model';
import { EdgeCase } from '../../shared/models/edge-case.model';
import { Persona } from '../../shared/models/persona.model';
import { UserStoryQualityScore } from '../../shared/models/quality-score.model';
import { Scenario } from '../../shared/models/scenario.model';
import { UserStory } from '../../shared/models/user-story.model';
import { AcceptanceCriteriaService } from '../../shared/services/acceptance-criteria';
import { EdgeCaseService } from '../../shared/services/edge-case';
import { PersonaService } from '../../shared/services/persona';
import { QualityScoreService } from '../../shared/services/quality-score';
import { ScenarioService } from '../../shared/services/scenario';
import { UserStoryService } from '../../shared/services/user-story';
import {
  UserStoryAcceptanceCriteriaModal,
  UserStoryAcceptanceCriteriaModalResult,
} from './user-story-acceptance-criteria-modal';
import { UserStoryContextModal } from './user-story-context-modal';
import { UserStoryEdgeCaseModal, UserStoryEdgeCaseModalResult } from './user-story-edge-case-modal';

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
  imports: [
    ReactiveFormsModule,
    FontAwesomeModule,
    MetaDropdown,
    QualityScorePanel,
    UserStoryFormFields,
  ],
  templateUrl: './project-user-story-editor.html',
  styleUrl: './project-user-story-editor.scss',
})
export class ProjectUserStoryEditor implements AfterViewChecked, OnDestroy {
  @Input() public projectId: string | null = null;
  @Input() public userStoryId: string | null = null;
  @ViewChild('acceptanceCriteriaTable')
  private acceptanceCriteriaTable?: ElementRef<HTMLTableElement>;
  @ViewChild('edgeCasesTable') private edgeCasesTable?: ElementRef<HTMLTableElement>;

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
  public latestQualityScore: UserStoryQualityScore | null = null;
  public activeTab: 'userStory' | 'qualityScore' = 'userStory';
  public scenarioOptions: ScenarioOption[] = [];
  public loading = true;
  public qualityScoreLoading = false;
  public saving = false;
  public loadFailed = false;

  public readonly entityIcons = ENTITY_ICONS;
  public readonly faCheck = faCheck;
  public readonly faPlus = faPlus;
  public readonly stageDropdownOptions: MetaDropdownOption[] = USER_STORY_STAGE_META.map(
    (stage) => ({
      value: stage.value,
      label: stage.label,
      colorClass: stage.colorClass,
    }),
  );

  private initialAcceptanceCriteria: AcceptanceCriteria[] = [];
  private initialEdgeCases: EdgeCase[] = [];
  private acceptanceCriteriaDataTable: Api | null = null;
  private acceptanceCriteriaDataTableGeneration = 0;
  private acceptanceCriteriaDataTableInitializing = false;
  private edgeCasesDataTable: Api | null = null;
  private edgeCasesDataTableGeneration = 0;
  private edgeCasesDataTableInitializing = false;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    @Optional() private readonly activeModal: NgbActiveModal | null,
    private readonly cdr: ChangeDetectorRef,
    private readonly fb: FormBuilder,
    private readonly modalService: NgbModal,
    private readonly personaService: PersonaService,
    private readonly qualityScoreService: QualityScoreService,
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
        this.userStoryId ? this.loadLatestQualityScore(this.userStoryId) : Promise.resolve(),
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

  public ngAfterViewChecked(): void {
    this.initializeDataTable(
      'acceptanceCriteria',
      this.acceptanceCriteriaTable,
      'No Acceptance Criteria added yet.',
    );
    this.initializeDataTable('edgeCases', this.edgeCasesTable, 'No Edge Cases added yet.');
  }

  public ngOnDestroy(): void {
    this.destroyDataTable('acceptanceCriteria');
    this.destroyDataTable('edgeCases');
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

  public get personaDropdownOptions(): MetaDropdownOption[] {
    return this.personaOptions.map((persona) => ({
      value: persona.id,
      label: persona.name,
    }));
  }

  public get filteredScenarioOptions(): ScenarioOption[] {
    return this.scenarioOptions.filter((option) => option.persona.id === this.personaControl.value);
  }

  public get scenarioDropdownOptions(): MetaDropdownOption[] {
    return this.filteredScenarioOptions.map((option) => ({
      value: option.scenario.id,
      label: option.scenario.title,
    }));
  }

  public get currentPersona(): Persona | undefined {
    return this.personaOptions.find((persona) => persona.id === this.personaControl.value);
  }

  public get currentScenarioOption(): ScenarioOption | undefined {
    return this.scenarioOptions.find((option) => option.scenario.id === this.scenarioControl.value);
  }

  public selectPersona(personaId: string): void {
    this.personaControl.setValue(personaId);
    this.personaControl.markAsDirty();
    this.personaControl.markAsTouched();
    this.selectFirstScenarioForPersona();
  }

  public selectScenario(scenarioId: string): void {
    this.scenarioControl.setValue(scenarioId);
    this.scenarioControl.markAsDirty();
    this.scenarioControl.markAsTouched();
  }

  private selectFirstScenarioForPersona(): void {
    const firstScenario = this.filteredScenarioOptions[0];
    this.scenarioControl.setValue(firstScenario?.scenario.id ?? '');
    this.scenarioControl.markAsDirty();
  }

  public selectStage(stage: UserStoryStage): void {
    this.stageControl.setValue(stage);
    this.stageControl.markAsDirty();
    this.stageControl.markAsTouched();
  }

  public selectStageValue(value: MetaDropdownValue): void {
    this.selectStage(value as UserStoryStage);
  }

  public selectPersonaValue(value: MetaDropdownValue): void {
    this.selectPersona(String(value));
  }

  public selectScenarioValue(value: MetaDropdownValue): void {
    this.selectScenario(String(value));
  }

  public selectTab(tab: 'userStory' | 'qualityScore'): void {
    if (tab === 'qualityScore' && !this.isEditing) {
      return;
    }

    if (tab === this.activeTab) {
      return;
    }

    if (tab === 'qualityScore') {
      this.destroyDataTable('acceptanceCriteria');
      this.destroyDataTable('edgeCases');
    }

    this.activeTab = tab;

    if (tab === 'userStory') {
      this.refreshDataTable('acceptanceCriteria');
      this.refreshDataTable('edgeCases');
    }
  }

  public async addAcceptanceCriteria(): Promise<void> {
    await this.openAcceptanceCriteriaModal(null);
  }

  public removeAcceptanceCriteria(index: number): void {
    this.destroyDataTable('acceptanceCriteria');
    this.acceptanceCriteriaForms.removeAt(index);
    this.form.markAsDirty();
    this.refreshDataTable('acceptanceCriteria');
  }

  public async editAcceptanceCriteria(index: number): Promise<void> {
    await this.openAcceptanceCriteriaModal(index);
  }

  public async addEdgeCase(): Promise<void> {
    await this.openEdgeCaseModal(null);
  }

  public removeEdgeCase(index: number): void {
    this.destroyDataTable('edgeCases');
    this.edgeCaseForms.removeAt(index);
    this.form.markAsDirty();
    this.refreshDataTable('edgeCases');
  }

  public async editEdgeCase(index: number): Promise<void> {
    await this.openEdgeCaseModal(index);
  }

  public openPersonaDetails(): void {
    const persona = this.currentScenarioOption?.persona;

    if (!persona) {
      return;
    }

    const modalRef = this.modalService.open(UserStoryContextModal, { centered: true });
    modalRef.componentInstance.title = persona.name;
    modalRef.componentInstance.icon = this.entityIcons.persona;
    modalRef.componentInstance.fields = [
      { label: 'Description', value: persona.description },
      { label: 'Context of use', value: persona.contextOfUse },
      { label: 'Goals', value: persona.goals },
      { label: 'Frustrations', value: persona.frustrations },
    ];
  }

  public openScenarioDetails(): void {
    const option = this.currentScenarioOption;

    if (!option) {
      return;
    }

    const modalRef = this.modalService.open(UserStoryContextModal, { centered: true });
    modalRef.componentInstance.title = option.scenario.title;
    modalRef.componentInstance.icon = this.entityIcons.scenario;
    modalRef.componentInstance.fields = [
      { label: 'Persona', value: option.persona.name },
      { label: 'Content', value: option.scenario.content },
    ];
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

  public acceptanceCriteriaText(index: number): string {
    return this.acceptanceCriteriaForms.at(index).controls.wording.value.trim();
  }

  public edgeCasePreconditions(index: number): string {
    return this.edgeCaseForms.at(index).controls.preconditions.value.trim();
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

  private async loadLatestQualityScore(userStoryId: string): Promise<void> {
    this.qualityScoreLoading = true;

    try {
      this.latestQualityScore =
        await this.qualityScoreService.getLatestByUserStoryId(userStoryId);
    } catch {
      this.latestQualityScore = null;
      this.notification.fail('Could not load User Story quality score');
    } finally {
      this.qualityScoreLoading = false;
    }
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
    this.destroyDataTable('acceptanceCriteria');
    this.acceptanceCriteriaForms.clear();

    for (const item of items) {
      this.acceptanceCriteriaForms.push(this.createAcceptanceCriteriaForm(item));
    }

    this.refreshDataTable('acceptanceCriteria');
  }

  private rebuildEdgeCases(items: EdgeCase[]): void {
    this.destroyDataTable('edgeCases');
    this.edgeCaseForms.clear();

    for (const item of items) {
      this.edgeCaseForms.push(this.createEdgeCaseForm(item));
    }

    this.refreshDataTable('edgeCases');
  }

  private createAcceptanceCriteriaForm(item?: Partial<AcceptanceCriteria>): AcceptanceCriteriaForm {
    return this.fb.nonNullable.group({
      id: [item?.id ?? ''],
      wording: [item?.wording ?? '', Validators.required],
    });
  }

  private createEdgeCaseForm(item?: Partial<EdgeCase>): EdgeCaseForm {
    return this.fb.nonNullable.group({
      id: [item?.id ?? ''],
      preconditions: [item?.preconditions ?? '', Validators.required],
      triggerAction: [item?.triggerAction ?? '', Validators.required],
      expectedBehavior: [item?.expectedBehavior ?? '', Validators.required],
    });
  }

  private async openEdgeCaseModal(index: number | null): Promise<void> {
    const modalRef = this.modalService.open(UserStoryEdgeCaseModal, { centered: true, size: 'lg' });
    const edgeCaseForm = index === null ? null : this.edgeCaseForms.at(index);

    modalRef.componentInstance.title = index === null ? 'Add edge case' : 'Edit edge case';
    modalRef.componentInstance.canDelete = index !== null;
    modalRef.componentInstance.value = edgeCaseForm
      ? {
          preconditions: edgeCaseForm.controls.preconditions.value,
          triggerAction: edgeCaseForm.controls.triggerAction.value,
          expectedBehavior: edgeCaseForm.controls.expectedBehavior.value,
        }
      : {
          preconditions: '',
          triggerAction: '',
          expectedBehavior: '',
        };

    try {
      const result = (await modalRef.result) as UserStoryEdgeCaseModalResult;

      if (result.action === 'delete') {
        if (index !== null) {
          this.removeEdgeCase(index);
        }

        return;
      }

      this.destroyDataTable('edgeCases');

      if (index === null) {
        this.edgeCaseForms.push(this.createEdgeCaseForm(result.value));
      } else {
        edgeCaseForm?.patchValue(result.value);
        edgeCaseForm?.markAsDirty();
      }

      this.form.markAsDirty();
      this.refreshDataTable('edgeCases');
    } catch {
      return;
    }
  }

  private async openAcceptanceCriteriaModal(index: number | null): Promise<void> {
    const modalRef = this.modalService.open(UserStoryAcceptanceCriteriaModal, { centered: true });
    const criteriaForm = index === null ? null : this.acceptanceCriteriaForms.at(index);

    modalRef.componentInstance.title =
      index === null ? 'Add acceptance criteria' : 'Edit acceptance criteria';
    modalRef.componentInstance.canDelete = index !== null;
    modalRef.componentInstance.value = criteriaForm
      ? {
          wording: criteriaForm.controls.wording.value,
        }
      : {
          wording: '',
        };

    try {
      const result = (await modalRef.result) as UserStoryAcceptanceCriteriaModalResult;

      if (result.action === 'delete') {
        if (index !== null) {
          this.removeAcceptanceCriteria(index);
        }

        return;
      }

      this.destroyDataTable('acceptanceCriteria');

      if (index === null) {
        this.acceptanceCriteriaForms.push(this.createAcceptanceCriteriaForm(result.value));
      } else {
        criteriaForm?.patchValue(result.value);
        criteriaForm?.markAsDirty();
      }

      this.form.markAsDirty();
      this.refreshDataTable('acceptanceCriteria');
    } catch {
      return;
    }
  }

  private async initializeDataTable(
    tableName: 'acceptanceCriteria' | 'edgeCases',
    tableRef: ElementRef<HTMLTableElement> | undefined,
    emptyTableMessage: string,
  ): Promise<void> {
    const state = this.dataTableState(tableName);

    if (this.loading || state.table || state.initializing || !tableRef) {
      return;
    }

    const table = tableRef.nativeElement;
    const generation = state.generation;
    this.setDataTableInitializing(tableName, true);

    try {
      const { default: DataTable } = await import('datatables.net-bs5');
      const currentState = this.dataTableState(tableName);
      const currentTableRef =
        tableName === 'acceptanceCriteria' ? this.acceptanceCriteriaTable : this.edgeCasesTable;

      if (
        generation !== currentState.generation ||
        currentState.table ||
        !currentTableRef ||
        currentTableRef.nativeElement !== table
      ) {
        return;
      }

      this.setDataTable(
        tableName,
        new DataTable(table, {
          info: false,
          language: {
            emptyTable: emptyTableMessage,
          },
          ordering: false,
          paging: false,
          searching: false,
        }),
      );
    } finally {
      this.setDataTableInitializing(tableName, false);
    }
  }

  private destroyDataTable(tableName: 'acceptanceCriteria' | 'edgeCases'): void {
    const state = this.dataTableState(tableName);

    this.incrementDataTableGeneration(tableName);
    state.table?.destroy();
    this.setDataTable(tableName, null);
  }

  private refreshDataTable(tableName: 'acceptanceCriteria' | 'edgeCases'): void {
    setTimeout(() => {
      if (tableName === 'acceptanceCriteria') {
        this.initializeDataTable(
          'acceptanceCriteria',
          this.acceptanceCriteriaTable,
          'No Acceptance Criteria added yet.',
        );
      } else {
        this.initializeDataTable('edgeCases', this.edgeCasesTable, 'No Edge Cases added yet.');
      }

      this.cdr.markForCheck();
    });
  }

  private dataTableState(tableName: 'acceptanceCriteria' | 'edgeCases'): {
    generation: number;
    initializing: boolean;
    table: Api | null;
  } {
    if (tableName === 'acceptanceCriteria') {
      return {
        generation: this.acceptanceCriteriaDataTableGeneration,
        initializing: this.acceptanceCriteriaDataTableInitializing,
        table: this.acceptanceCriteriaDataTable,
      };
    }

    return {
      generation: this.edgeCasesDataTableGeneration,
      initializing: this.edgeCasesDataTableInitializing,
      table: this.edgeCasesDataTable,
    };
  }

  private incrementDataTableGeneration(tableName: 'acceptanceCriteria' | 'edgeCases'): void {
    if (tableName === 'acceptanceCriteria') {
      this.acceptanceCriteriaDataTableGeneration++;
      return;
    }

    this.edgeCasesDataTableGeneration++;
  }

  private setDataTable(tableName: 'acceptanceCriteria' | 'edgeCases', table: Api | null): void {
    if (tableName === 'acceptanceCriteria') {
      this.acceptanceCriteriaDataTable = table;
      return;
    }

    this.edgeCasesDataTable = table;
  }

  private setDataTableInitializing(
    tableName: 'acceptanceCriteria' | 'edgeCases',
    initializing: boolean,
  ): void {
    if (tableName === 'acceptanceCriteria') {
      this.acceptanceCriteriaDataTableInitializing = initializing;
      return;
    }

    this.edgeCasesDataTableInitializing = initializing;
  }
}
