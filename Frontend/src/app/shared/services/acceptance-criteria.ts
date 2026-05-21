import { Injectable } from '@angular/core';
import { Api } from '../../core/services/api';
import { ApiController } from '../enums/api-controller.enum';
import { AcceptanceCriteriaForCreation } from '../models/acceptance-criteria-for-creation.model';
import { AcceptanceCriteriaForUpdate } from '../models/acceptance-criteria-for-update.model';
import { AcceptanceCriteria } from '../models/acceptance-criteria.model';
import { ApiResponse } from '../models/api-response.model';

@Injectable({
  providedIn: 'root',
})
export class AcceptanceCriteriaService {
  constructor(private api: Api) {}

  public async getById(id: string): Promise<AcceptanceCriteria | null> {
    const response: ApiResponse<AcceptanceCriteria> = await this.api.get(
      ApiController.AcceptanceCriteria,
      id,
    );

    return response.data;
  }

  public async getByUserStoryId(userStoryId: string): Promise<AcceptanceCriteria[]> {
    const response: ApiResponse<AcceptanceCriteria[]> = await this.api.get(
      ApiController.AcceptanceCriteria,
      `by-user-story/${userStoryId}`,
    );

    return response.data ?? [];
  }

  public async create(
    acceptanceCriteria: AcceptanceCriteriaForCreation,
  ): Promise<AcceptanceCriteria | null> {
    const response: ApiResponse<AcceptanceCriteria> = await this.api.post<
      AcceptanceCriteriaForCreation,
      AcceptanceCriteria
    >(
      ApiController.AcceptanceCriteria,
      '',
      acceptanceCriteria,
    );

    return response.data;
  }

  public async update(acceptanceCriteria: AcceptanceCriteriaForUpdate): Promise<void> {
    await this.api.put(ApiController.AcceptanceCriteria, '', acceptanceCriteria);
  }

  public async delete(id: string): Promise<void> {
    await this.api.delete(ApiController.AcceptanceCriteria, id);
  }
}
