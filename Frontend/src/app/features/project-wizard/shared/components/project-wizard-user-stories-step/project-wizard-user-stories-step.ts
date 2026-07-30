import { Component, effect, Input, OnChanges, SimpleChanges } from '@angular/core';
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
import { UserStoryFormFields } from '../../../../../shared/components/user-story-form-fields/user-story-form-fields';
import { ENTITY_ICONS } from '../../../../../shared/icons/entity-icons';
import { Scenario } from '../../../../../shared/models/scenario.model';
import { UserStory } from '../../../../../shared/models/user-story.model';
import { UserStoryService } from '../../../../../shared/services/user-story';
import { StepNavigationResult } from '../../models/project-wizard-step.model';
import { ProjectWizardState } from '../../services/project-wizard-state';

type UserStoryForm = FormGroup<{
  id: FormControl<string>;
  title: FormControl<string>;
  description: FormControl<string>;
}>;

@Component({
  selector: 'app-project-wizard-user-stories-step',
  imports: [ReactiveFormsModule, FontAwesomeModule, UserStoryFormFields],
  templateUrl: './project-wizard-user-stories-step.html',
  styleUrl: './project-wizard-user-stories-step.scss',
})
export class ProjectWizardUserStoriesStep implements OnChanges {
  @Input() public initialScenarioIndex = 0;

  public readonly form;
  public readonly faCheck = faCheck;
  public readonly faPen = faPen;
  public readonly faTrash = faTrash;
  public readonly entityIcons = ENTITY_ICONS;
  public currentScenarioIndex = 0;
  public editingUserStoryIndex: number | null = null;
  private syncedKey = '';

  constructor(
    private readonly fb: FormBuilder,
    private readonly modalService: NgbModal,
    private readonly userStoryService: UserStoryService,
    public readonly wizardState: ProjectWizardState,
  ) {
    this.form = this.fb.nonNullable.group({
      userStories: this.fb.array<UserStoryForm>([]),
    });

    effect(() => {
      const key = this.createSyncKey();

      if (this.form.dirty && this.syncedKey !== '') {
        return;
      }

      if (key === this.syncedKey) {
        return;
      }

      this.syncCurrentScenarioForm(key);
    });
  }

  public ngOnChanges(changes: SimpleChanges): void {
    if (!changes['initialScenarioIndex']) {
      return;
    }

    this.goToInitialScenario();
  }

  public get scenarios(): Scenario[] {
    return this.wizardState.scenarios();
  }

  public get currentScenario(): Scenario | null {
    return this.scenarios[this.currentScenarioIndex] ?? null;
  }

  public get isLastScenario(): boolean {
    return this.currentScenarioIndex >= this.scenarios.length - 1;
  }

  public get userStoryForms(): FormArray<UserStoryForm> {
    return this.form.controls.userStories;
  }

  public get currentScenarioUserStories(): UserStory[] {
    const scenario = this.currentScenario;

    if (!scenario) {
      return [];
    }

    return this.wizardState
      .userStories()
      .filter((userStory) => userStory.scenarioId === scenario.id);
  }

  public addUserStory(): void {
    if (!this.closeCurrentUserStoryForm()) {
      return;
    }

    this.userStoryForms.push(this.createUserStoryForm());
    this.editingUserStoryIndex = this.userStoryForms.length - 1;
    this.form.markAsDirty();
  }

  public editUserStory(index: number): void {
    if (index === this.editingUserStoryIndex) {
      return;
    }

    if (!this.closeCurrentUserStoryForm()) {
      return;
    }

    this.editingUserStoryIndex = index;
  }

  public doneEditingUserStory(): void {
    this.closeCurrentUserStoryForm();
  }

  public async removeUserStory(index: number): Promise<void> {
    const confirmed = await this.confirmRemoveUserStory(index);

    if (!confirmed) {
      return;
    }

    this.userStoryForms.removeAt(index);
    this.updateEditingIndexAfterRemove(index);
    this.form.markAsDirty();
  }

  public async canGoNext(): Promise<StepNavigationResult> {
    const saved = await this.saveCurrentScenarioUserStories();

    if (!saved) {
      return 'stay';
    }

    if (this.currentScenarioIndex < this.scenarios.length - 1) {
      this.currentScenarioIndex++;
      this.syncCurrentScenarioForm();
      return 'handled-internally';
    }

    return 'next-main-step';
  }

  public goToScenario(index: number): void {
    if (
      index === this.currentScenarioIndex ||
      this.form.dirty ||
      !this.closeCurrentUserStoryForm()
    ) {
      return;
    }

    this.currentScenarioIndex = index;
    this.syncCurrentScenarioForm();
  }

  public isInvalid(index: number, controlName: keyof UserStoryForm['controls']): boolean {
    const control = this.userStoryForms.at(index).controls[controlName];
    return control.touched && control.invalid;
  }

  private async saveCurrentScenarioUserStories(): Promise<boolean> {
    this.form.markAllAsTouched();

    const scenario = this.currentScenario;

    if (!scenario) {
      return true;
    }

    if (this.userStoryForms.length === 0 || this.form.invalid) {
      this.editingUserStoryIndex = this.findFirstInvalidUserStoryIndex();
      return false;
    }

    if (this.form.pristine) {
      return true;
    }

    const formUserStories = this.userStoryForms.getRawValue();
    const allUserStories = this.wizardState.userStories();
    const existingUserStories = allUserStories.filter(
      (userStory) => userStory.scenarioId === scenario.id,
    );
    const formUserStoryIds = new Set(
      formUserStories.map((userStory) => userStory.id).filter(Boolean),
    );
    const removedUserStories = existingUserStories.filter(
      (userStory) => !formUserStoryIds.has(userStory.id),
    );

    await Promise.all(
      removedUserStories.map((userStory) => this.userStoryService.delete(userStory.id)),
    );

    const savedUserStories: UserStory[] = [];

    for (const userStory of formUserStories) {
      if (userStory.id) {
        const existingUserStory = existingUserStories.find((x) => x.id === userStory.id);

        if (!existingUserStory) {
          return false;
        }

        await this.userStoryService.update(userStory);

        savedUserStories.push({
          ...existingUserStory,
          ...userStory,
          scenarioId: scenario.id,
          stage: existingUserStory.stage,
          createdAt: existingUserStory.createdAt,
        });

        continue;
      }

      const savedUserStory = await this.userStoryService.create({
        title: userStory.title,
        description: userStory.description,
        scenarioId: scenario.id,
      });

      if (!savedUserStory) {
        return false;
      }

      savedUserStories.push({
        ...savedUserStory,
        scenarioId: scenario.id,
      });
    }

    this.wizardState.setUserStories([
      ...allUserStories.filter((userStory) => userStory.scenarioId !== scenario.id),
      ...savedUserStories,
    ]);
    this.form.markAsPristine();
    this.editingUserStoryIndex = null;
    this.syncedKey = this.createSyncKey();

    return true;
  }

  private createSyncKey(): string {
    const scenario = this.currentScenario;
    const scenarioIds = this.scenarios.map((item) => item.id).join('|');
    const userStoryIds = this.currentScenarioUserStories.map((userStory) => userStory.id).join('|');

    return `${scenario?.id ?? ''}:${scenarioIds}:${userStoryIds}`;
  }

  private syncCurrentScenarioForm(key = this.createSyncKey()): void {
    this.syncedKey = key;
    this.rebuildForm(this.currentScenarioUserStories);
  }

  private goToInitialScenario(): void {
    if (this.form.dirty) {
      return;
    }

    const lastScenarioIndex = Math.max(this.scenarios.length - 1, 0);
    this.currentScenarioIndex = Math.min(Math.max(this.initialScenarioIndex, 0), lastScenarioIndex);
    this.syncCurrentScenarioForm();
  }

  private rebuildForm(userStories: UserStory[]): void {
    this.userStoryForms.clear();

    for (const userStory of userStories) {
      this.userStoryForms.push(this.createUserStoryForm(userStory));
    }

    this.editingUserStoryIndex = null;
    this.form.markAsPristine();
  }

  private createUserStoryForm(userStory?: UserStory): UserStoryForm {
    return this.fb.nonNullable.group({
      id: [userStory?.id ?? ''],
      title: [userStory?.title ?? '', Validators.required],
      description: [userStory?.description ?? '', Validators.required],
    });
  }

  private closeCurrentUserStoryForm(): boolean {
    if (this.editingUserStoryIndex === null) {
      return true;
    }

    const userStoryForm = this.userStoryForms.at(this.editingUserStoryIndex);
    userStoryForm.markAllAsTouched();

    if (userStoryForm.invalid) {
      return false;
    }

    this.editingUserStoryIndex = null;
    return true;
  }

  private async confirmRemoveUserStory(index: number): Promise<boolean> {
    const modalRef = this.modalService.open(ConfirmationModal, { centered: true });
    const userStoryTitle =
      this.userStoryForms.at(index).controls.title.value || `User story ${index + 1}`;

    modalRef.componentInstance.title = 'Remove user story';
    modalRef.componentInstance.message = `Remove "${userStoryTitle}"? This change will be saved when you continue.`;
    modalRef.componentInstance.confirmText = 'Remove';
    modalRef.componentInstance.confirmButtonClass = 'btn-danger';

    try {
      return await modalRef.result;
    } catch {
      return false;
    }
  }

  private updateEditingIndexAfterRemove(removedIndex: number): void {
    if (this.editingUserStoryIndex === null) {
      return;
    }

    if (this.editingUserStoryIndex === removedIndex) {
      this.editingUserStoryIndex = null;
      return;
    }

    if (this.editingUserStoryIndex > removedIndex) {
      this.editingUserStoryIndex--;
    }
  }

  private findFirstInvalidUserStoryIndex(): number | null {
    const invalidIndex = this.userStoryForms.controls.findIndex(
      (userStoryForm) => userStoryForm.invalid,
    );

    return invalidIndex >= 0 ? invalidIndex : null;
  }
}
