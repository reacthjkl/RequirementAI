import { Component } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Project } from '../../shared/models/project.model';
import { UserStory } from '../../shared/models/user-story.model';
import { ProjectService } from '../../shared/services/project';
import { UserStoryService } from '../../shared/services/user-story';

interface BoardColumn {
  key: string;
  label: string;
  userStories: UserStory[];
}

@Component({
  selector: 'app-project-board',
  imports: [RouterLink],
  templateUrl: './project-board.html',
  styleUrl: './project-board.scss',
})
export class ProjectBoard {
  readonly columns: BoardColumn[] = [
    { key: 'new', label: 'New', userStories: [] },
    { key: 'active', label: 'Active', userStories: [] },
    { key: 'testing', label: 'Testing', userStories: [] },
    { key: 'closed', label: 'Closed', userStories: [] },
  ];

  project: Project | null = null;
  projectId: string | null = null;
  loading = true;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly projectService: ProjectService,
    private readonly userStoryService: UserStoryService,
  ) {}

  async ngOnInit(): Promise<void> {
    this.projectId = this.route.snapshot.paramMap.get('projectId');

    if (!this.projectId) {
      this.loading = false;
      return;
    }

    const [project, userStories] = await Promise.all([
      this.projectService.getById(this.projectId),
      this.userStoryService.getByProjectId(this.projectId),
    ]);

    this.project = project;
    this.columns[0].userStories = userStories;
    this.loading = false;
  }
}

