export interface QualityScoreBase {
  id: string;
  createdAt: string;
  updatedAt: string;
  overallScore: number;
  strengths: string;
  weaknesses: string;
  suggestions: string;
}

export interface PersonaQualityScore extends QualityScoreBase {
  personaId: string;
  clarityScore: number;
  realismScore: number;
  goalClarityScore: number;
  painPointsScore: number;
  relevanceScore: number;
  differentiationScore: number;
}

export interface ScenarioQualityScore extends QualityScoreBase {
  scenarioId: string;
  clarityScore: number;
  contextScore: number;
  triggerScore: number;
  flowCompletenessScore: number;
  edgeCasesScore: number;
  personaFitScore: number;
}

export interface UserStoryQualityScore extends QualityScoreBase {
  userStoryId: string;
  clarityScore: number;
  completenessScore: number;
  testabilityScore: number;
  acceptanceCriteriaScore: number;
  scopeScore: number;
  businessValueScore: number;
  ambiguityScore: number;
}

export type QualityScore = PersonaQualityScore | ScenarioQualityScore | UserStoryQualityScore;
export type QualityScoreContext = 'persona' | 'scenario' | 'userStory';
