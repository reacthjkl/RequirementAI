import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faEllipsisVertical, faPlus } from '@fortawesome/free-solid-svg-icons';
import { NgbDropdownModule, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Notification } from '../../core/services/notification.service';
import { ConfirmationModal } from '../../shared/components/confirmation-modal/confirmation-modal';
import {
  USER_STORY_STAGE_META,
  UserStoryStageMeta,
} from '../../shared/constants/user-story-stage-meta';
import { UserStoryStage } from '../../shared/enums/user-story-stage.enum';
import { Persona } from '../../shared/models/persona.model';
import { Project } from '../../shared/models/project.model';
import { Scenario } from '../../shared/models/scenario.model';
import { UserStory } from '../../shared/models/user-story.model';
import { PersonaService } from '../../shared/services/persona';
import { ProjectService } from '../../shared/services/project';
import { ScenarioService } from '../../shared/services/scenario';
import { UserStoryService } from '../../shared/services/user-story';
import { ProjectUserStoryEditor } from '../project-user-story-editor/project-user-story-editor';

interface BoardUserStory {
  userStory: UserStory;
  personaName: string;
}

interface BoardColumn {
  key: UserStoryStage;
  label: string;
  stageMeta: UserStoryStageMeta;
  userStories: BoardUserStory[];
}

@Component({
  selector: 'app-project-board',
  imports: [FontAwesomeModule, NgbDropdownModule],
  templateUrl: './project-board.html',
  styleUrl: './project-board.scss',
})
export class ProjectBoard {
  public readonly faEllipsisVertical = faEllipsisVertical;
  public readonly faPlus = faPlus;
  public readonly columns: BoardColumn[] = [
    {
      key: UserStoryStage.New,
      label: 'New',
      stageMeta: USER_STORY_STAGE_META[0],
      userStories: [],
    },
    {
      key: UserStoryStage.Active,
      label: 'Active',
      stageMeta: USER_STORY_STAGE_META[1],
      userStories: [],
    },
    {
      key: UserStoryStage.Testing,
      label: 'Testing',
      stageMeta: USER_STORY_STAGE_META[2],
      userStories: [],
    },
    {
      key: UserStoryStage.Closed,
      label: 'Closed',
      stageMeta: USER_STORY_STAGE_META[3],
      userStories: [],
    },
  ];

  public projectId: string | null = null;
  public project: Project | null = null;
  public loading = true;
  public deletingUserStoryId: string | null = null;
  public draggedUserStory: UserStory | null = null;
  public dragOverColumn: UserStoryStage | null = null;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly modalService: NgbModal,
    private readonly notification: Notification,
    private readonly projectService: ProjectService,
    private readonly userStoryService: UserStoryService,
    private readonly scenarioService: ScenarioService,
    private readonly personaService: PersonaService,
  ) {}

  public async ngOnInit(): Promise<void> {
    this.projectId = this.route.snapshot.paramMap.get('projectId');

    if (!this.projectId) {
      this.loading = false;
      return;
    }

    const [project] = await Promise.all([
      this.projectService.getById(this.projectId),
      this.loadBoard(),
    ]);
    this.project = project;
  }

  public async openCreateUserStory(): Promise<void> {
    await this.openUserStoryEditor();
  }

  public async openEditUserStory(userStory: UserStory, event?: MouseEvent): Promise<void> {
    await this.openUserStoryEditor(userStory.id, event);
  }

  public async deleteUserStory(userStory: UserStory, event: MouseEvent): Promise<void> {
    event.stopPropagation();

    if (this.deletingUserStoryId) {
      return;
    }

    const confirmed = await this.confirmDeleteUserStory(userStory);

    if (!confirmed) {
      return;
    }

    this.deletingUserStoryId = userStory.id;

    try {
      await this.userStoryService.delete(userStory.id);
      this.removeUserStoryFromColumns(userStory.id);
      this.notification.success('User Story deleted');
    } catch {
      this.notification.fail('Could not delete User Story');
    } finally {
      this.deletingUserStoryId = null;
    }
  }

  public handleCardKeydown(event: KeyboardEvent, userStory: UserStory): void {
    if (event.key !== 'Enter' && event.key !== ' ') {
      return;
    }

    event.preventDefault();
    void this.openEditUserStory(userStory);
  }

  public startDragging(event: DragEvent, userStory: UserStory): void {
    this.draggedUserStory = userStory;
    event.dataTransfer?.setData('text/plain', userStory.id);
    event.dataTransfer?.setDragImage(event.currentTarget as Element, 16, 16);
  }

  public stopDragging(): void {
    this.draggedUserStory = null;
    this.dragOverColumn = null;
  }

  public allowDrop(event: DragEvent, column: BoardColumn): void {
    event.preventDefault();
    this.dragOverColumn = column.key;
  }

  public clearDragOver(column: BoardColumn): void {
    if (this.dragOverColumn === column.key) {
      this.dragOverColumn = null;
    }
  }

  public async dropUserStory(event: DragEvent, column: BoardColumn): Promise<void> {
    event.preventDefault();

    const userStory = this.draggedUserStory;
    this.stopDragging();

    if (!userStory || userStory.stage === column.key) {
      return;
    }

    await this.moveUserStoryToStage(userStory, column.key);
  }

  private async openUserStoryEditor(userStoryId?: string, event?: MouseEvent): Promise<void> {
    if (!this.projectId) {
      return;
    }

    const trigger = event?.currentTarget;
    const modalRef = this.modalService.open(ProjectUserStoryEditor, {
      centered: true,
      scrollable: true,
      size: 'xl',
    });
    modalRef.componentInstance.projectId = this.projectId;
    modalRef.componentInstance.userStoryId = userStoryId ?? null;

    try {
      const saved = await modalRef.result;

      if (saved) {
        await this.loadBoard();
      }
    } catch {
      return;
    } finally {
      if (trigger instanceof HTMLElement) {
        trigger.blur();
      }
    }
  }

  private async confirmDeleteUserStory(userStory: UserStory): Promise<boolean> {
    const modalRef = this.modalService.open(ConfirmationModal, { centered: true });
    modalRef.componentInstance.title = 'Delete User Story';
    modalRef.componentInstance.message = `Delete "${userStory.title}"? This action cannot be undone.`;
    modalRef.componentInstance.confirmText = 'Delete user story';
    modalRef.componentInstance.confirmButtonClass = 'btn-danger';

    try {
      return !!(await modalRef.result);
    } catch {
      return false;
    }
  }

  private async loadBoard(): Promise<void> {
    if (!this.projectId) {
      return;
    }

    const userStories = await this.userStoryService.getByProjectId(this.projectId);
    const boardUserStories = await this.createBoardUserStories(userStories);

    for (const column of this.columns) {
      column.userStories = boardUserStories.filter((item) => item.userStory.stage === column.key);
    }

    this.loading = false;
  }

  private removeUserStoryFromColumns(userStoryId: string): void {
    for (const column of this.columns) {
      column.userStories = column.userStories.filter((item) => item.userStory.id !== userStoryId);
    }
  }

  private async moveUserStoryToStage(userStory: UserStory, stage: UserStoryStage): Promise<void> {
    const previousStage = userStory.stage;
    userStory.stage = stage;
    this.placeUserStoryInColumns(userStory);

    try {
      await this.userStoryService.update({
        id: userStory.id,
        title: userStory.title,
        description: userStory.description,
        stage,
      });
    } catch {
      userStory.stage = previousStage;
      this.placeUserStoryInColumns(userStory);
      this.notification.fail('Could not update User Story stage');
    }
  }

  private placeUserStoryInColumns(userStory: UserStory): void {
    let movedItem: BoardUserStory | undefined;

    for (const column of this.columns) {
      const existingIndex = column.userStories.findIndex(
        (item) => item.userStory.id === userStory.id,
      );

      if (existingIndex >= 0) {
        movedItem = column.userStories.splice(existingIndex, 1)[0];
        break;
      }
    }

    if (!movedItem) {
      return;
    }

    const targetColumn = this.columns.find((column) => column.key === userStory.stage);
    targetColumn?.userStories.push(movedItem);
  }

  private async createBoardUserStories(userStories: UserStory[]): Promise<BoardUserStory[]> {
    const scenarioIds = [...new Set(userStories.map((userStory) => userStory.scenarioId))];
    const scenarios = await Promise.all(scenarioIds.map((id) => this.scenarioService.getById(id)));
    const scenarioMap = new Map<string, Scenario>(
      scenarios
        .filter((scenario): scenario is Scenario => !!scenario)
        .map((scenario) => [scenario.id, scenario]),
    );

    const personaIds = [
      ...new Set([...scenarioMap.values()].map((scenario) => scenario.personaId)),
    ];
    const personas = await Promise.all(personaIds.map((id) => this.personaService.getById(id)));
    const personaMap = new Map<string, Persona>(
      personas
        .filter((persona): persona is Persona => !!persona)
        .map((persona) => [persona.id, persona]),
    );

    return userStories.map((userStory) => {
      const scenario = scenarioMap.get(userStory.scenarioId);
      const persona = scenario ? personaMap.get(scenario.personaId) : undefined;

      return {
        userStory,
        personaName: persona?.name ?? 'Unknown persona',
      };
    });
  }
}
