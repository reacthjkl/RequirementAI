import { LowestScoreItem } from './lowest-score-item.model';
import { ProjectScoreTrendPoint } from './project-score-trend-point.model';

export interface ProjectQualityCriterionAverage {
  artifactType: 'Persona' | 'Scenario' | 'UserStory';
  criterionName: string;
  averageScore: number;
}

export interface ProjectQualityOverview {
  totalProjectScore: number;
  averagePersonaScore: number;
  averageScenarioScore: number;
  averageUserStoryScore: number;
  lowestPersona: LowestScoreItem | null;
  lowestScenario: LowestScoreItem | null;
  lowestUserStory: LowestScoreItem | null;
  criterionAverages: ProjectQualityCriterionAverage[];
  scoreTrend: ProjectScoreTrendPoint[];
}
