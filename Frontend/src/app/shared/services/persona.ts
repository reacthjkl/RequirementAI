import { Injectable } from '@angular/core';
import { Api } from '../../core/services/api';
import { ApiController } from '../enums/api-controller.enum';
import { ApiResponse } from '../models/api-response.model';
import { PersonaForCreation } from '../models/persona-for-creation.model';
import { PersonaForUpdate } from '../models/persona-for-update.model';
import { Persona } from '../models/persona.model';

@Injectable({
  providedIn: 'root',
})
export class PersonaService {
  constructor(private api: Api) {}

  public async getById(id: string): Promise<Persona | null> {
    const response: ApiResponse<Persona | null> = await this.api.get(ApiController.Persona, id);

    return response.data;
  }

  public async getByProjectId(projectId: string): Promise<Persona[]> {
    const response: ApiResponse<Persona[]> = await this.api.get(
      ApiController.Persona,
      `by-project/${projectId}`,
    );

    return response.data ?? [];
  }

  public async create(persona: PersonaForCreation): Promise<Persona | null> {
    const response: ApiResponse<Persona | null> = await this.api.post(
      ApiController.Persona,
      '',
      persona,
    );

    return response.data;
  }

  public async update(persona: PersonaForUpdate): Promise<void> {
    await this.api.put(ApiController.Persona, '', persona);
  }

  public async delete(id: string): Promise<void> {
    await this.api.delete(ApiController.Persona, id);
  }
}
