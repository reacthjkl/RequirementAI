import { Injectable } from '@angular/core';
import { Api } from '../../core/services/api';
import { ApiController } from '../enums/api-controller.enum';
import { ApiResponse } from '../models/api-response.model';
import { ScenarioForCreation } from '../models/scenario-for-creation.model';
import { ScenarioForUpdate } from '../models/scenario-for-update.model';
import { Scenario } from '../models/scenario.model';

@Injectable({
  providedIn: 'root',
})
export class ScenarioService {
  constructor(private api: Api) {}

  public async getById(id: string): Promise<Scenario | null> {
    const response: ApiResponse<Scenario | null> = await this.api.get(ApiController.Scenario, id);

    return response.data;
  }

  public async getByPersonaId(personaId: string): Promise<Scenario[]> {
    const response: ApiResponse<Scenario[]> = await this.api.get(
      ApiController.Scenario,
      `by-persona/${personaId}`,
    );

    return response.data ?? [];
  }

  public async create(scenario: ScenarioForCreation): Promise<Scenario | null> {
    const response: ApiResponse<Scenario> = await this.api.post<ScenarioForCreation, Scenario>(
      ApiController.Scenario,
      '',
      scenario,
    );

    return response.data;
  }

  public async update(scenario: ScenarioForUpdate): Promise<void> {
    await this.api.put(ApiController.Scenario, '', scenario);
  }

  public async delete(id: string): Promise<void> {
    await this.api.delete(ApiController.Scenario, id);
  }
}
