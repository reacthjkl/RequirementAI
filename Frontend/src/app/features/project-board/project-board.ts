import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { UserStoryStage } from '../../shared/enums/user-story-stage.enum';
import { ENTITY_ICONS } from '../../shared/icons/entity-icons';
import { UserStory } from '../../shared/models/user-story.model';
import { UserStoryService } from '../../shared/services/user-story';

interface BoardColumn {
  key: UserStoryStage;
  label: string;
  userStories: UserStory[];
}

@Component({
  selector: 'app-project-board',
  imports: [FontAwesomeModule],
  templateUrl: './project-board.html',
  styleUrl: './project-board.scss',
})
export class ProjectBoard {
  public readonly entityIcons = ENTITY_ICONS;
  public readonly columns: BoardColumn[] = [
    { key: UserStoryStage.New, label: 'New', userStories: [] },
    { key: UserStoryStage.Active, label: 'Active', userStories: [] },
    { key: UserStoryStage.Testing, label: 'Testing', userStories: [] },
    { key: UserStoryStage.Closed, label: 'Closed', userStories: [] },
  ];

  public projectId: string | null = null;
  public loading = true;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly userStoryService: UserStoryService,
  ) {}

  public async ngOnInit(): Promise<void> {
    this.projectId = this.route.snapshot.paramMap.get('projectId');

    if (!this.projectId) {
      this.loading = false;
      return;
    }

    const userStories = await this.userStoryService.getByProjectId(this.projectId);

    for (const column of this.columns) {
      column.userStories = userStories.filter((x) => x.stage === column.key);
    }

    this.loading = false;
  }
}
