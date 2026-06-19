import { Injectable } from '@angular/core';
import { Api } from '../../core/services/api';
import { ApiController } from '../enums/api-controller.enum';
import { ApiResponse } from '../models/api-response.model';
import {
  PersonaQualityScore,
  ScenarioQualityScore,
  UserStoryQualityScore,
} from '../models/quality-score.model';

@Injectable({
  providedIn: 'root',
})
export class QualityScoreService {
  constructor(private api: Api) {}

  public async getLatestByPersonaId(personaId: string): Promise<PersonaQualityScore | null> {
    const response: ApiResponse<PersonaQualityScore | null> = await this.api.get(
      ApiController.QualityScore,
      `latest/by-persona/${personaId}`,
    );

    return response.data;
  }

  public async getLatestByScenarioId(scenarioId: string): Promise<ScenarioQualityScore | null> {
    const response: ApiResponse<ScenarioQualityScore | null> = await this.api.get(
      ApiController.QualityScore,
      `latest/by-scenario/${scenarioId}`,
    );

    return response.data;
  }

  public async getLatestByUserStoryId(
    userStoryId: string,
  ): Promise<UserStoryQualityScore | null> {
    const response: ApiResponse<UserStoryQualityScore | null> = await this.api.get(
      ApiController.QualityScore,
      `latest/by-user-story/${userStoryId}`,
    );

    return response.data;
  }
}
