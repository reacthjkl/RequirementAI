import { ChangeDetectorRef, Component, Input, Optional } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faCheck, faTimes } from '@fortawesome/free-solid-svg-icons';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Notification } from '../../core/services/notification.service';
import { PersonaFormFields } from '../../shared/components/persona-form-fields/persona-form-fields';
import { QualityScorePanel } from '../../shared/components/quality-score-panel/quality-score-panel';
import { ENTITY_ICONS } from '../../shared/icons/entity-icons';
import { Persona } from '../../shared/models/persona.model';
import { PersonaQualityScore } from '../../shared/models/quality-score.model';
import { PersonaService } from '../../shared/services/persona';
import { QualityScoreService } from '../../shared/services/quality-score';

@Component({
  selector: 'app-project-persona-editor',
  imports: [
    ReactiveFormsModule,
    RouterModule,
    FontAwesomeModule,
    PersonaFormFields,
    QualityScorePanel,
  ],
  templateUrl: './project-persona-editor.html',
})
export class ProjectPersonaEditor {
  @Input() public projectId: string | null = null;
  @Input() public personaId: string | null = null;

  public readonly form;

  public persona: Persona | null = null;
  public latestQualityScore: PersonaQualityScore | null = null;
  public activeTab: 'persona' | 'qualityScore' = 'persona';
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
    private readonly personaService: PersonaService,
    private readonly qualityScoreService: QualityScoreService,
    private readonly notification: Notification,
  ) {
    this.form = this.fb.nonNullable.group({
      id: [''],
      name: ['', Validators.required],
      description: ['', Validators.required],
      contextOfUse: ['', Validators.required],
      goals: ['', Validators.required],
      frustrations: ['', Validators.required],
    });
  }

  public async ngOnInit(): Promise<void> {
    this.projectId ??= this.route.snapshot.paramMap.get('projectId');
    this.personaId ??= this.route.snapshot.paramMap.get('personaId');

    if (!this.projectId) {
      this.finishLoading();
      return;
    }

    if (!this.personaId) {
      this.finishLoading();
      return;
    }

    const [persona] = await Promise.all([
      this.personaService.getById(this.personaId),
      this.loadLatestQualityScore(this.personaId),
    ]);
    this.persona = persona;

    if (this.persona) {
      this.form.patchValue({
        id: this.persona.id,
        name: this.persona.name,
        description: this.persona.description,
        contextOfUse: this.persona.contextOfUse,
        goals: this.persona.goals,
        frustrations: this.persona.frustrations,
      });
      this.form.markAsPristine();
    }

    this.finishLoading();
  }

  public get isEditing(): boolean {
    return !!this.personaId;
  }

  public get title(): string {
    return this.isEditing ? 'Edit persona' : 'Add persona';
  }

  public get isModal(): boolean {
    return !!this.activeModal;
  }

  public selectTab(tab: 'persona' | 'qualityScore'): void {
    if (tab === 'qualityScore' && !this.isEditing) {
      return;
    }

    this.activeTab = tab;
  }

  public async save(): Promise<void> {
    this.form.markAllAsTouched();

    if (!this.projectId || this.form.invalid || this.saving) {
      return;
    }

    this.saving = true;

    try {
      const formValue = this.trimFormValue();

      if (this.personaId) {
        await this.personaService.update({
          id: this.personaId,
          ...formValue,
        });
        this.notification.success('Persona updated');
      } else {
        await this.personaService.create({
          ...formValue,
          projectId: this.projectId,
        });
        this.notification.success('Persona created');
      }

      await this.closeAfterSave();
    } catch {
      this.notification.fail('Could not save persona');
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

    await this.router.navigate(['/projects', this.projectId, 'personas']);
  }

  private async closeAfterSave(): Promise<void> {
    if (this.activeModal) {
      this.activeModal.close(true);
      return;
    }

    await this.router.navigate(['/projects', this.projectId, 'personas']);
  }

  private finishLoading(): void {
    setTimeout(() => {
      this.loading = false;
      this.cdr.markForCheck();
    });
  }

  private async loadLatestQualityScore(personaId: string): Promise<void> {
    this.qualityScoreLoading = true;

    try {
      this.latestQualityScore = await this.qualityScoreService.getLatestByPersonaId(personaId);
    } catch {
      this.latestQualityScore = null;
      this.notification.fail('Could not load persona quality score');
    } finally {
      this.qualityScoreLoading = false;
    }
  }

  private trimFormValue() {
    const value = this.form.getRawValue();

    return {
      name: value.name.trim(),
      description: value.description.trim(),
      contextOfUse: value.contextOfUse.trim(),
      goals: value.goals.trim(),
      frustrations: value.frustrations.trim(),
    };
  }
}
