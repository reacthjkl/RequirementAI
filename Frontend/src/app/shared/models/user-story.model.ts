import { UserStoryStage } from '../enums/user-story-stage.enum';

export interface UserStory {
  id: string;
  title: string;
  description: string;
  createdAt: string;
  scenarioId: string;
  stage: UserStoryStage;
}
