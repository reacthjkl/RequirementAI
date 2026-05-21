import { Injectable } from '@angular/core';
import { PersonaService } from '../../../../shared/services/persona';
import { ProjectService } from '../../../../shared/services/project';
import { ScenarioService } from '../../../../shared/services/scenario';
import { UserStoryService } from '../../../../shared/services/user-story';
import { ProjectWizardState } from './project-wizard-state';

@Injectable({
  providedIn: 'root',
})
export class ProjectWizardLoader {
  constructor(
    private readonly projectService: ProjectService,
    private readonly personaService: PersonaService,
    private readonly scenarioService: ScenarioService,
    private readonly userStoryService: UserStoryService,
    private readonly wizardState: ProjectWizardState,
  ) {}

  async load(projectId: string): Promise<boolean> {
    const project = await this.projectService.getById(projectId);

    if (!project) {
      this.wizardState.clear();
      return false;
    }

    this.wizardState.setProject(project);

    const personas = await this.personaService.getByProjectId(project.id);
    this.wizardState.setPersonas(personas);

    const scenarioGroups = await Promise.all(
      personas.map((persona) => this.scenarioService.getByPersonaId(persona.id)),
    );
    this.wizardState.setScenarios(scenarioGroups.flat());

    const userStories = await this.userStoryService.getByProjectId(project.id);
    this.wizardState.setUserStories(userStories);

    return true;
  }
}

