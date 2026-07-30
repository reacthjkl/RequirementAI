import { UserStoryStage } from '../enums/user-story-stage.enum';

export interface UserStoryStageMeta {
  colorClass: string;
  label: string;
  value: UserStoryStage;
}

export const USER_STORY_STAGE_META: UserStoryStageMeta[] = [
  { value: UserStoryStage.New, label: 'New', colorClass: 'stage-dot-new' },
  { value: UserStoryStage.Active, label: 'Active', colorClass: 'stage-dot-active' },
  { value: UserStoryStage.Testing, label: 'Testing', colorClass: 'stage-dot-testing' },
  { value: UserStoryStage.Closed, label: 'Closed', colorClass: 'stage-dot-closed' },
];
