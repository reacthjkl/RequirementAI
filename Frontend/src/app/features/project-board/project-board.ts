import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ENTITY_ICONS } from '../../shared/icons/entity-icons';
import { UserStory } from '../../shared/models/user-story.model';
import { UserStoryService } from '../../shared/services/user-story';

interface BoardColumn {
  key: string;
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
    { key: 'new', label: 'New', userStories: [] },
    { key: 'active', label: 'Active', userStories: [] },
    { key: 'testing', label: 'Testing', userStories: [] },
    { key: 'closed', label: 'Closed', userStories: [] },
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

    this.columns[0].userStories = userStories;
    this.loading = false;
  }
}
