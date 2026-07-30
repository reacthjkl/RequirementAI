import { ChangeDetectorRef, Component, Input, Optional } from '@angular/core';
import { FormBuilder, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faCheck, faTimes } from '@fortawesome/free-solid-svg-icons';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Notification } from '../../core/services/notification.service';
import {
  MetaDropdown,
  MetaDropdownOption,
  MetaDropdownValue,
} from '../../shared/components/meta-dropdown/meta-dropdown';
import { QualityScorePanel } from '../../shared/components/quality-score-panel/quality-score-panel';
import { ScenarioFormFields } from '../../shared/components/scenario-form-fields/scenario-form-fields';
import { ENTITY_ICONS } from '../../shared/icons/entity-icons';
import { Persona } from '../../shared/models/persona.model';
import { ScenarioQualityScore } from '../../shared/models/quality-score.model';
import { Scenario } from '../../shared/models/scenario.model';
import { PersonaService } from '../../shared/services/persona';
import { QualityScoreService } from '../../shared/services/quality-score';
import { ScenarioService } from '../../shared/services/scenario';
import { UserStoryContextModal } from '../project-user-story-editor/user-story-context-modal';

@Component({
  selector: 'app-project-scenario-editor',
  imports: [
    ReactiveFormsModule,
    RouterModule,
    FontAwesomeModule,
    MetaDropdown,
    QualityScorePanel,
    ScenarioFormFields,
  ],
  templateUrl: './project-scenario-editor.html',
})
export class ProjectScenarioEditor {
  @Input() public projectId: string | null = null;
  @Input() public scenarioId: string | null = null;

  public readonly form;
  public readonly personaControl = new FormControl('', {
    nonNullable: true,
    validators: [Validators.required],
  });

  public scenario: Scenario | null = null;
  public latestQualityScore: ScenarioQualityScore | null = null;
  public activeTab: 'scenario' | 'qualityScore' = 'scenario';
  public personas: Persona[] = [];
  public loading = true;
  public qualityScoreLoading = false;
  public saving = false;

  public readonly entityIcons = ENTITY_ICONS;
  public readonly faCheck = faCheck;
  public readonly faTimes = faTimes;

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
    private readonly notification: Notification,
  ) {
    this.form = this.fb.nonNullable.group({
      id: [''],
      title: ['', Validators.required],
      content: ['', Validators.required],
    });
  }

  public async ngOnInit(): Promise<void> {
    this.projectId ??= this.route.snapshot.paramMap.get('projectId');
    this.scenarioId ??= this.route.snapshot.paramMap.get('scenarioId');

    if (!this.projectId) {
      this.finishLoading();
      return;
    }

    const [personas, scenario] = await Promise.all([
      this.personaService.getByProjectId(this.projectId),
      this.scenarioId ? this.scenarioService.getById(this.scenarioId) : Promise.resolve(null),
      this.scenarioId ? this.loadLatestQualityScore(this.scenarioId) : Promise.resolve(),
    ]);

    this.personas = personas;
    this.scenario = scenario;

    if (scenario) {
      this.form.patchValue({
        id: scenario.id,
        title: scenario.title,
        content: scenario.content,
      });
      this.personaControl.setValue(scenario.personaId);
    } else if (personas.length > 0) {
      this.personaControl.setValue(personas[0].id);
    }

    this.form.markAsPristine();
    this.personaControl.markAsPristine();
    this.finishLoading();
  }

  public get isEditing(): boolean {
    return !!this.scenarioId;
  }

  public get title(): string {
    return this.isEditing ? 'Edit scenario' : 'Add scenario';
  }

  public get isModal(): boolean {
    return !!this.activeModal;
  }

  public selectTab(tab: 'scenario' | 'qualityScore'): void {
    if (tab === 'qualityScore' && !this.isEditing) {
      return;
    }

    this.activeTab = tab;
  }

  public get currentPersona(): Persona | undefined {
    const personaId = this.personaControl.value;
    return this.personas.find((persona) => persona.id === personaId);
  }

  public get personaDropdownOptions(): MetaDropdownOption[] {
    return this.personas.map((persona) => ({
      value: persona.id,
      label: persona.name,
    }));
  }

  public selectPersona(personaId: string): void {
    this.personaControl.setValue(personaId);
    this.personaControl.markAsDirty();
    this.personaControl.markAsTouched();
  }

  public selectPersonaValue(value: MetaDropdownValue): void {
    this.selectPersona(String(value));
  }

  public openPersonaDetails(): void {
    const persona = this.currentPersona;

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

  public async save(): Promise<void> {
    this.form.markAllAsTouched();
    this.personaControl.markAsTouched();

    if (!this.projectId || this.form.invalid || this.personaControl.invalid || this.saving) {
      return;
    }

    this.saving = true;

    try {
      const formValue = this.trimFormValue();

      if (this.scenarioId) {
        await this.scenarioService.update({
          id: this.scenarioId,
          title: formValue.title,
          content: formValue.content,
        });
        this.notification.success('Scenario updated');
      } else {
        await this.scenarioService.create({
          title: formValue.title,
          content: formValue.content,
          personaId: formValue.personaId,
        });
        this.notification.success('Scenario created');
      }

      await this.closeAfterSave();
    } catch {
      this.notification.fail('Could not save scenario');
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

    await this.router.navigate(['/projects', this.projectId, 'scenarios']);
  }

  public isPersonaInvalid(): boolean {
    return this.personaControl.touched && this.personaControl.invalid;
  }

  private async closeAfterSave(): Promise<void> {
    if (this.activeModal) {
      this.activeModal.close(true);
      return;
    }

    await this.router.navigate(['/projects', this.projectId, 'scenarios']);
  }

  private finishLoading(): void {
    setTimeout(() => {
      this.loading = false;
      this.cdr.markForCheck();
    });
  }

  private async loadLatestQualityScore(scenarioId: string): Promise<void> {
    this.qualityScoreLoading = true;

    try {
      this.latestQualityScore = await this.qualityScoreService.getLatestByScenarioId(scenarioId);
    } catch {
      this.latestQualityScore = null;
      this.notification.fail('Could not load scenario quality score');
    } finally {
      this.qualityScoreLoading = false;
    }
  }

  private trimFormValue() {
    const value = this.form.getRawValue();

    return {
      personaId: this.personaControl.value,
      title: value.title.trim(),
      content: value.content.trim(),
    };
  }
}
