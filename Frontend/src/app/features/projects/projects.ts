import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faGrip, faList, faPlus } from '@fortawesome/free-solid-svg-icons';
import { ProjectStatusComponent } from '../../shared/components/project-status/project-status';
import { ProjectStatus } from '../../shared/enums/project-status.enum';
import { Project } from '../../shared/models/project.model';
import { PersonaService } from '../../shared/services/persona';
import { ProjectService } from '../../shared/services/project';
import { ScenarioService } from '../../shared/services/scenario';
import { UserStoryService } from '../../shared/services/user-story';

type ProjectViewMode = 'list' | 'cards';

@Component({
  selector: 'app-projects',
  imports: [FontAwesomeModule, RouterModule, ProjectStatusComponent],
  templateUrl: './projects.html',
})
export class Projects {
  public projects: Project[] = [];

  readonly viewModeStorageKey: string = 'projects-view-mode';
  public viewMode: ProjectViewMode =
    (localStorage.getItem(this.viewModeStorageKey) as ProjectViewMode) ?? 'list';

  //icons
  faGrip = faGrip;
  faList = faList;
  faPlus = faPlus;

  constructor(
    private projectSvc: ProjectService,
    private personaSvc: PersonaService,
    private scenarioSvc: ScenarioService,
    private userStorySvc: UserStoryService,
    private router: Router,
  ) {}

  async ngOnInit() {
    this.projects = await this.projectSvc.get();
  }

  setViewMode(viewMode: ProjectViewMode): void {
    this.viewMode = viewMode;
    localStorage.setItem(this.viewModeStorageKey, this.viewMode);
  }

  async openProject(project: Project): Promise<void> {
    if (!this.isIncompleteStatus(project.status)) {
      await this.router.navigate(['/projects', project.id, 'board']);
      return;
    }

    const incompleteTarget = await this.resolveIncompleteTarget(project.id);

    if (!incompleteTarget) {
      await this.router.navigate(['/projects', project.id, 'board']);
      return;
    }

    await this.router.navigate(['/projects', 'wizard', project.id], {
      queryParams: incompleteTarget,
    });
  }

  private async resolveIncompleteTarget(projectId: string) {
    const personas = await this.personaSvc.getByProjectId(projectId);

    if (personas.length === 0) {
      return { step: 'personas' };
    }

    const scenarioGroups = await Promise.all(
      personas.map((persona) => this.scenarioSvc.getByPersonaId(persona.id)),
    );
    const scenarios = scenarioGroups.flat();
    const missingScenarioPersonaIndex = scenarioGroups.findIndex((group) => group.length === 0);

    if (missingScenarioPersonaIndex >= 0) {
      return { step: 'scenarios', personaIndex: missingScenarioPersonaIndex };
    }

    const userStoryGroups = await Promise.all(
      scenarios.map((scenario) => this.userStorySvc.getByScenarioId(scenario.id)),
    );
    const missingUserStoryScenarioIndex = userStoryGroups.findIndex((group) => group.length === 0);

    if (missingUserStoryScenarioIndex >= 0) {
      return { step: 'userStories', scenarioIndex: missingUserStoryScenarioIndex };
    }

    return null;
  }

  private isIncompleteStatus(status: ProjectStatus): boolean {
    return status === ProjectStatus.Incomplete;
  }
}
