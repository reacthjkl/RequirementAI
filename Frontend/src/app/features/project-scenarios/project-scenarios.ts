import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ENTITY_ICONS } from '../../shared/icons/entity-icons';
import { Persona } from '../../shared/models/persona.model';
import { Scenario } from '../../shared/models/scenario.model';
import { PersonaService } from '../../shared/services/persona';
import { ScenarioService } from '../../shared/services/scenario';
import { UserStoryService } from '../../shared/services/user-story';
import { ProjectScenarioEditor } from '../project-scenario-editor/project-scenario-editor';

interface ScenarioOverviewItem {
  scenario: Scenario;
  persona: Persona;
  userStoryCount: number;
}

@Component({
  selector: 'app-project-scenarios',
  imports: [FontAwesomeModule],
  templateUrl: './project-scenarios.html',
  styleUrl: './project-scenarios.scss',
})
export class ProjectScenarios {
  public projectId: string | null = null;
  public loading = true;
  public scenarios: ScenarioOverviewItem[] = [];

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

    await this.loadScenarios();
  }

  public async openCreateScenario(): Promise<void> {
    await this.openScenarioEditor();
  }

  public async openEditScenario(scenario: Scenario, event?: MouseEvent): Promise<void> {
    await this.openScenarioEditor(scenario.id, event);
  }

  public userStoryLabel(count: number): string {
    return count === 1 ? 'User Story' : 'User Stories';
  }

  private async openScenarioEditor(scenarioId?: string, event?: MouseEvent): Promise<void> {
    if (!this.projectId) {
      return;
    }

    const trigger = event?.currentTarget;
    const modalRef = this.modalService.open(ProjectScenarioEditor, {
      centered: true,
      scrollable: true,
      size: 'xl',
    });
    modalRef.componentInstance.projectId = this.projectId;
    modalRef.componentInstance.scenarioId = scenarioId ?? null;

    try {
      const saved = await modalRef.result;

      if (saved) {
        await this.loadScenarios();
      }
    } catch {
      return;
    } finally {
      if (trigger instanceof HTMLElement) {
        trigger.blur();
      }
    }
  }

  private async loadScenarios(): Promise<void> {
    if (!this.projectId) {
      return;
    }

    this.loading = true;
    const personas = await this.personaService.getByProjectId(this.projectId);
    const scenarioGroups = await Promise.all(
      personas.map(async (persona) => {
        const scenarios = await this.scenarioService.getByPersonaId(persona.id);

        return Promise.all(
          scenarios.map(async (scenario) => ({
            scenario,
            persona,
            userStoryCount: (await this.userStoryService.getByScenarioId(scenario.id)).length,
          })),
        );
      }),
    );

    this.scenarios = scenarioGroups.flat();
    this.loading = false;
  }
}
