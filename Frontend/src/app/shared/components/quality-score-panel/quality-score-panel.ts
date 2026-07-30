import { Component, Input } from '@angular/core';
import { QualityScorePieChart } from '../quality-score-pie-chart/quality-score-pie-chart';
import {
  PersonaQualityScore,
  QualityScore,
  QualityScoreContext,
  ScenarioQualityScore,
  UserStoryQualityScore,
} from '../../models/quality-score.model';

interface QualityMetric {
  label: string;
  score: number;
}

@Component({
  selector: 'app-quality-score-panel',
  imports: [QualityScorePieChart],
  templateUrl: './quality-score-panel.html',
})
export class QualityScorePanel {
  @Input({ required: true }) public context: QualityScoreContext = 'persona';
  @Input() public score: QualityScore | null = null;
  @Input() public loading = false;

  public get title(): string {
    switch (this.context) {
      case 'persona':
        return 'Latest persona quality score';
      case 'scenario':
        return 'Latest scenario quality score';
      case 'userStory':
        return 'Latest User Story quality score';
    }
  }

  public get description(): string {
    switch (this.context) {
      case 'persona':
        return 'How complete, believable, and useful this persona is for requirement work.';
      case 'scenario':
        return 'How clearly this scenario describes context, trigger, flow, and persona fit.';
      case 'userStory':
        return 'How ready this User Story is for implementation and validation.';
    }
  }

  public get metrics(): QualityMetric[] {
    if (!this.score) {
      return [];
    }

    switch (this.context) {
      case 'persona':
        return this.personaMetrics(this.score as PersonaQualityScore);
      case 'scenario':
        return this.scenarioMetrics(this.score as ScenarioQualityScore);
      case 'userStory':
        return this.userStoryMetrics(this.score as UserStoryQualityScore);
    }
  }

  public formatDate(value: string): string {
    const date = new Date(value);

    if (Number.isNaN(date.getTime())) {
      return value;
    }

    return new Intl.DateTimeFormat(undefined, {
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
      month: 'short',
      year: 'numeric',
    }).format(date);
  }

  private personaMetrics(score: PersonaQualityScore): QualityMetric[] {
    return [
      { label: 'Clarity', score: score.clarityScore },
      { label: 'Realism', score: score.realismScore },
      { label: 'Goal clarity', score: score.goalClarityScore },
      { label: 'Pain points', score: score.painPointsScore },
      { label: 'Relevance', score: score.relevanceScore },
      { label: 'Differentiation', score: score.differentiationScore },
    ];
  }

  private scenarioMetrics(score: ScenarioQualityScore): QualityMetric[] {
    return [
      { label: 'Clarity', score: score.clarityScore },
      { label: 'Context', score: score.contextScore },
      { label: 'Trigger', score: score.triggerScore },
      { label: 'Flow completeness', score: score.flowCompletenessScore },
      { label: 'Edge cases', score: score.edgeCasesScore },
      { label: 'Persona fit', score: score.personaFitScore },
    ];
  }

  private userStoryMetrics(score: UserStoryQualityScore): QualityMetric[] {
    return [
      { label: 'Clarity', score: score.clarityScore },
      { label: 'Completeness', score: score.completenessScore },
      { label: 'Testability', score: score.testabilityScore },
      { label: 'Acceptance criteria', score: score.acceptanceCriteriaScore },
      { label: 'Scope', score: score.scopeScore },
      { label: 'Business value', score: score.businessValueScore },
      { label: 'Ambiguity', score: score.ambiguityScore },
    ];
  }
}
