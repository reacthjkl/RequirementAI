import { LowestScoreItem } from './lowest-score-item.model';
import { ProjectScoreTrendPoint } from './project-score-trend-point.model';

export interface ProjectQualityOverview {
  totalProjectScore: number;
  averagePersonaScore: number;
  averageScenarioScore: number;
  averageUserStoryScore: number;
  lowestPersona: LowestScoreItem | null;
  lowestScenario: LowestScoreItem | null;
  lowestUserStory: LowestScoreItem | null;
  scoreTrend: ProjectScoreTrendPoint[];
}
