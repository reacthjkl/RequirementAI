import { Component } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ProjectRefineButton } from '../../shared/components/project-refine-button/project-refine-button';
import { UserStoryStage } from '../../shared/enums/user-story-stage.enum';
import { ENTITY_COLLECTION_ICONS, ENTITY_ICONS } from '../../shared/icons/entity-icons';
import { Project } from '../../shared/models/project.model';
import { PersonaService } from '../../shared/services/persona';
import { ProjectService } from '../../shared/services/project';
import { ScenarioService } from '../../shared/services/scenario';
import { UserStoryService } from '../../shared/services/user-story';

@Component({
  selector: 'app-project-overview',
  imports: [RouterModule, FontAwesomeModule, ProjectRefineButton],
  templateUrl: './project-overview.html',
  styleUrl: './project-overview.scss',
})
export class ProjectOverview {
  public project: Project | null = null;
  public loading = true;

  public personaCount = 0;
  public scenarioCount = 0;
  public userStoryCount = 0;
  public closedUserStoryCount = 0;

  public readonly entityCollectionIcons = ENTITY_COLLECTION_ICONS;
  public readonly entityIcons = ENTITY_ICONS;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly projectService: ProjectService,
    private readonly personaService: PersonaService,
    private readonly scenarioService: ScenarioService,
    private readonly userStoryService: UserStoryService,
  ) {}

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
}
