import { Component, effect } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
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
  imports: [ReactiveFormsModule],
  templateUrl: './project-wizard-user-stories-step.html',
  styleUrl: './project-wizard-user-stories-step.scss',
})
export class ProjectWizardUserStoriesStep {
  readonly form;
  currentScenarioIndex = 0;
  private syncedKey = '';

  constructor(
    private readonly fb: FormBuilder,
    private readonly userStoryService: UserStoryService,
    readonly wizardState: ProjectWizardState,
  ) {
    this.form = this.fb.nonNullable.group({
      userStories: this.fb.array<UserStoryForm>([]),
    });

    effect(() => {
      const scenarios = this.wizardState.scenarios();
      const userStories = this.wizardState.userStories();
      const key = `${this.currentScenario?.id ?? ''}:${scenarios.length}:${userStories
        .map((userStory) => userStory.id)
        .join('|')}`;

      if (this.form.dirty && this.syncedKey !== '') {
        return;
      }

      if (key === this.syncedKey) {
        return;
      }

      this.syncedKey = key;
      this.rebuildForm(this.currentScenarioUserStories);
    });
  }

  get scenarios(): Scenario[] {
    return this.wizardState.scenarios();
  }

  get currentScenario(): Scenario | null {
    return this.scenarios[this.currentScenarioIndex] ?? null;
  }

  get userStoryForms(): FormArray<UserStoryForm> {
    return this.form.controls.userStories;
  }

  get currentScenarioUserStories(): UserStory[] {
    const scenario = this.currentScenario;

    if (!scenario) {
      return [];
    }

    return this.wizardState
      .userStories()
      .filter((userStory) => userStory.scenarioId === scenario.id);
  }

  addUserStory(): void {
    this.userStoryForms.push(this.createUserStoryForm());
    this.form.markAsDirty();
  }

  removeUserStory(index: number): void {
    this.userStoryForms.removeAt(index);
    this.form.markAsDirty();
  }

  async canGoNext(): Promise<StepNavigationResult> {
    const saved = await this.saveCurrentScenarioUserStories();

    if (!saved) {
      return 'stay';
    }

    if (this.currentScenarioIndex < this.scenarios.length - 1) {
      this.currentScenarioIndex++;
      this.rebuildForm(this.currentScenarioUserStories);
      return 'handled-internally';
    }

    return 'next-main-step';
  }

  goToScenario(index: number): void {
    if (index === this.currentScenarioIndex || this.form.dirty) {
      return;
    }

    this.currentScenarioIndex = index;
    this.rebuildForm(this.currentScenarioUserStories);
  }

  isInvalid(index: number, controlName: keyof UserStoryForm['controls']): boolean {
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
        await this.userStoryService.update(userStory);
        savedUserStories.push({
          ...existingUserStories.find(
            (existingUserStory) => existingUserStory.id === userStory.id,
          ),
          ...userStory,
          scenarioId: scenario.id,
          createdAt:
            existingUserStories.find(
              (existingUserStory) => existingUserStory.id === userStory.id,
            )?.createdAt ?? new Date().toISOString(),
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

      savedUserStories.push(savedUserStory);
    }

    this.wizardState.setUserStories([
      ...allUserStories.filter((userStory) => userStory.scenarioId !== scenario.id),
      ...savedUserStories,
    ]);
    this.form.markAsPristine();

    return true;
  }

  private rebuildForm(userStories: UserStory[]): void {
    this.userStoryForms.clear();

    for (const userStory of userStories) {
      this.userStoryForms.push(this.createUserStoryForm(userStory));
    }

    if (this.currentScenario && this.userStoryForms.length === 0) {
      this.userStoryForms.push(this.createUserStoryForm());
    }

    this.form.markAsPristine();
  }

  private createUserStoryForm(userStory?: UserStory): UserStoryForm {
    return this.fb.nonNullable.group({
      id: [userStory?.id ?? ''],
      title: [userStory?.title ?? '', Validators.required],
      description: [userStory?.description ?? '', Validators.required],
    });
  }
}
