import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ENTITY_COLLECTION_ICONS, ENTITY_ICONS } from '../../shared/icons/entity-icons';
import { Persona } from '../../shared/models/persona.model';
import { PersonaService } from '../../shared/services/persona';
import { ScenarioService } from '../../shared/services/scenario';
import { UserStoryService } from '../../shared/services/user-story';
import { ProjectPersonaEditor } from '../project-persona-editor/project-persona-editor';

interface PersonaOverviewItem {
  persona: Persona;
  scenarioCount: number;
  userStoryCount: number;
}

@Component({
  selector: 'app-project-personas',
  imports: [FontAwesomeModule],
  templateUrl: './project-personas.html',
  styleUrl: './project-personas.scss',
})
export class ProjectPersonas {
  public projectId: string | null = null;
  public loading = true;
  public personas: PersonaOverviewItem[] = [];

  public readonly entityCollectionIcons = ENTITY_COLLECTION_ICONS;
  public readonly entityIcons = ENTITY_ICONS;
  public readonly faPlus = faPlus;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly modalService: NgbModal,
    private readonly personaService: PersonaService,
    private readonly scenarioService: ScenarioService,
    private readonly userStoryService: UserStoryService,
  ) {}

  public async ngOnInit(): Promise<void> {
    this.projectId = this.route.snapshot.paramMap.get('projectId');

    if (!this.projectId) {
      this.loading = false;
      return;
    }

    await this.loadPersonas();
  }

  public async openCreatePersona(): Promise<void> {
    await this.openPersonaEditor();
  }

  public async openEditPersona(persona: Persona, event?: MouseEvent): Promise<void> {
    await this.openPersonaEditor(persona.id, event);
  }

  public userStoryLabel(count: number): string {
    return count === 1 ? 'User Story' : 'User Stories';
  }

  public scenarioLabel(count: number): string {
    return count === 1 ? 'Scenario' : 'Scenarios';
  }

  private async openPersonaEditor(personaId?: string, event?: MouseEvent): Promise<void> {
    if (!this.projectId) {
      return;
    }

    const trigger = event?.currentTarget;
    const modalRef = this.modalService.open(ProjectPersonaEditor, {
      centered: true,
      scrollable: true,
      size: 'xl',
    });
    modalRef.componentInstance.projectId = this.projectId;
    modalRef.componentInstance.personaId = personaId ?? null;

    try {
      const saved = await modalRef.result;

      if (saved) {
        await this.loadPersonas();
      }
    } catch {
      return;
    } finally {
      if (trigger instanceof HTMLElement) {
        trigger.blur();
      }
    }
  }

  private async loadPersonas(): Promise<void> {
    if (!this.projectId) {
      return;
    }

    this.loading = true;
    const personas = await this.personaService.getByProjectId(this.projectId);

    this.personas = await Promise.all(
      personas.map(async (persona) => {
        const [scenarios, userStories] = await Promise.all([
          this.scenarioService.getByPersonaId(persona.id),
          this.userStoryService.getByPersonaId(persona.id),
        ]);

        return {
          persona,
          scenarioCount: scenarios.length,
          userStoryCount: userStories.length,
        };
      }),
    );

    this.loading = false;
  }
}
