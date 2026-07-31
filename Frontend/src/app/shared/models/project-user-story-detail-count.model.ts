export interface ProjectUserStoryDetailCount {
  projectId: string;
  userStoryCount: number;
  totalAcceptanceCriteria: number;
  totalEdgeCases: number;
  averageAcceptanceCriteriaPerUserStory: number;
  averageEdgeCasesPerUserStory: number;
}
