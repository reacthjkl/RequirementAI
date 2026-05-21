import { Injectable } from '@angular/core';
import { Api } from '../../core/services/api';
import { ApiController } from '../enums/api-controller.enum';
import { ApiResponse } from '../models/api-response.model';
import { UserStoryForCreation } from '../models/user-story-for-creation.model';
import { UserStoryForUpdate } from '../models/user-story-for-update.model';
import { UserStory } from '../models/user-story.model';

@Injectable({
  providedIn: 'root',
})
export class UserStoryService {
  constructor(private api: Api) {}

  public async getById(id: string): Promise<UserStory | null> {
    const response: ApiResponse<UserStory | null> = await this.api.get(ApiController.UserStory, id);

    return response.data;
  }

  public async getByScenarioId(scenarioId: string): Promise<UserStory[]> {
    const response: ApiResponse<UserStory[]> = await this.api.get(
      ApiController.UserStory,
      `by-scenario/${scenarioId}`,
    );

    return response.data ?? [];
  }

  public async getByPersonaId(personaId: string): Promise<UserStory[]> {
    const response: ApiResponse<UserStory[]> = await this.api.get(
      ApiController.UserStory,
      `by-persona/${personaId}`,
    );

    return response.data ?? [];
  }

  public async getByProjectId(projectId: string): Promise<UserStory[]> {
    const response: ApiResponse<UserStory[]> = await this.api.get(
      ApiController.UserStory,
      `by-project/${projectId}`,
    );

    return response.data ?? [];
  }

  public async create(userStory: UserStoryForCreation): Promise<UserStory | null> {
    const response: ApiResponse<UserStory> = await this.api.post<UserStoryForCreation, UserStory>(
      ApiController.UserStory,
      '',
      userStory,
    );

    return response.data;
  }

  public async update(userStory: UserStoryForUpdate): Promise<void> {
    await this.api.put(ApiController.UserStory, '', userStory);
  }

  public async delete(id: string): Promise<void> {
    await this.api.delete(ApiController.UserStory, id);
  }
}
