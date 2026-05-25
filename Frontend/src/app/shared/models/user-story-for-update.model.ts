import { UserStoryStage } from '../enums/user-story-stage.enum';

export interface UserStoryForUpdate {
  id: string;
  title: string;
  description: string;
  stage?: UserStoryStage;
}
