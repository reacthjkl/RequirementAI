import { Injectable } from '@angular/core';
import { Api } from '../../core/services/api';
import { ApiController } from '../enums/api-controller.enum';
import { ApiResponse } from '../models/api-response.model';
import { EdgeCaseForCreation } from '../models/edge-case-for-creation.model';
import { EdgeCaseForUpdate } from '../models/edge-case-for-update.model';
import { EdgeCase } from '../models/edge-case.model';

@Injectable({
  providedIn: 'root',
})
export class EdgeCaseService {
  constructor(private api: Api) {}

  public async getById(id: string): Promise<EdgeCase | null> {
    const response: ApiResponse<EdgeCase | null> = await this.api.get(ApiController.EdgeCase, id);

    return response.data;
  }

  public async getByUserStoryId(userStoryId: string): Promise<EdgeCase[]> {
    const response: ApiResponse<EdgeCase[]> = await this.api.get(
      ApiController.EdgeCase,
      `by-user-story/${userStoryId}`,
    );

    return response.data ?? [];
  }

  public async create(edgeCase: EdgeCaseForCreation): Promise<EdgeCase | null> {
    const response: ApiResponse<EdgeCase> = await this.api.post<EdgeCaseForCreation, EdgeCase>(
      ApiController.EdgeCase,
      '',
      edgeCase,
    );

    return response.data;
  }

  public async update(edgeCase: EdgeCaseForUpdate): Promise<void> {
    await this.api.put(ApiController.EdgeCase, '', edgeCase);
  }

  public async delete(id: string): Promise<void> {
    await this.api.delete(ApiController.EdgeCase, id);
  }
}
