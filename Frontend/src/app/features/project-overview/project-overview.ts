import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faCheck, faPen, faTimes } from '@fortawesome/free-solid-svg-icons';
import { Notification } from '../../core/services/notification.service';
import { UserStoryStage } from '../../shared/enums/user-story-stage.enum';
import { ENTITY_COLLECTION_ICONS, ENTITY_ICONS } from '../../shared/icons/entity-icons';
import { Project } from '../../shared/models/project.model';
import { PersonaService } from '../../shared/services/persona';
import { ProjectService } from '../../shared/services/project';
import { ScenarioService } from '../../shared/services/scenario';
import { UserStoryService } from '../../shared/services/user-story';

@Component({
  selector: 'app-project-overview',
  imports: [ReactiveFormsModule, FontAwesomeModule],
  templateUrl: './project-overview.html',
  styleUrl: './project-overview.scss',
})
export class ProjectOverview {
  public readonly form;

  public project: Project | null = null;
  public loading = true;
  public saving = false;
  public editing = false;

  public personaCount = 0;
  public scenarioCount = 0;
  public userStoryCount = 0;
  public closedUserStoryCount = 0;

  public readonly entityCollectionIcons = ENTITY_COLLECTION_ICONS;
  public readonly entityIcons = ENTITY_ICONS;
  public readonly faCheck = faCheck;
  public readonly faPen = faPen;
  public readonly faTimes = faTimes;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly fb: FormBuilder,
    private readonly projectService: ProjectService,
    private readonly personaService: PersonaService,
    private readonly scenarioService: ScenarioService,
    private readonly userStoryService: UserStoryService,
    private readonly notification: Notification,
  ) {
    this.form = this.fb.nonNullable.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
    });
  }

  public async ngOnInit(): Promise<void> {
    const projectId = this.route.snapshot.paramMap.get('projectId');

    if (!projectId) {
      this.loading = false;
      return;
    }

    try {
      const [project, personas, userStories] = await Promise.all([
        this.projectService.getById(projectId),
        this.personaService.getByProjectId(projectId),
        this.userStoryService.getByProjectId(projectId),
      ]);

      this.project = project;
      this.personaCount = personas.length;
      this.userStoryCount = userStories.length;
      this.closedUserStoryCount = userStories.filter(
        (story) => story.stage === UserStoryStage.Closed,
      ).length;

      const scenarioGroups = await Promise.all(
        personas.map((persona) => this.scenarioService.getByPersonaId(persona.id)),
      );
      this.scenarioCount = scenarioGroups.flat().length;

      if (project) {
        this.form.patchValue({
          name: project.name,
          description: project.description,
        });
        this.form.markAsPristine();
      }
    } finally {
      this.loading = false;
    }
  }

  public get progressPercent(): number {
    if (this.userStoryCount === 0) {
      return 0;
    }

    return Math.round((this.closedUserStoryCount / this.userStoryCount) * 100);
  }

  public startEditing(): void {
    if (!this.project) {
      return;
    }

    this.form.patchValue({
      name: this.project.name,
      description: this.project.description,
    });
    this.form.markAsPristine();
    this.editing = true;
  }

  public cancelEditing(): void {
    if (!this.project) {
      return;
    }

    this.form.patchValue({
      name: this.project.name,
      description: this.project.description,
    });
    this.form.markAsPristine();
    this.editing = false;
  }

  public async saveProject(): Promise<void> {
    this.form.markAllAsTouched();

    if (!this.project || this.form.invalid || this.saving) {
      return;
    }

    const formValue = this.form.getRawValue();
    const update = {
      id: this.project.id,
      name: formValue.name.trim(),
      description: formValue.description.trim(),
    };

    if (!update.name || !update.description) {
      this.form.controls.name.setValue(update.name);
      this.form.controls.description.setValue(update.description);
      return;
    }

    this.saving = true;

    try {
      await this.projectService.update(update);
      this.project = {
        ...this.project,
        ...update,
      };
      this.form.patchValue(update);
      this.form.markAsPristine();
      this.editing = false;
      this.notification.success('Project updated');
    } catch {
      this.notification.fail('Could not update project');
    } finally {
      this.saving = false;
    }
  }

  public isInvalid(controlName: 'name' | 'description'): boolean {
    const control = this.form.controls[controlName];
    return control.touched && control.invalid;
  }
}
