import { Injectable } from '@angular/core';
import { Api } from '../../core/services/api';
import { ApiController } from '../enums/api-controller.enum';
import { ApiResponse } from '../models/api-response.model';
import { ProjectForCreation } from '../models/project-for-creation.model';
import { ProjectForUpdate } from '../models/project-for-update.model';
import { Project } from '../models/project.model';

@Injectable({
  providedIn: 'root',
})
export class ProjectService {
  constructor(private api: Api) {}

  public async getById(id: string): Promise<Project | null> {
    const response: ApiResponse<Project | null> = await this.api.get(ApiController.Project, id);

    return response.data;
  }

  public async get(): Promise<Project[]> {
    const response: ApiResponse<Project[]> = await this.api.get(ApiController.Project, '');

    return response.data ?? [];
  }

  public async create(project: ProjectForCreation): Promise<Project | null> {
    const response: ApiResponse<Project> = await this.api.post<ProjectForCreation, Project>(
      ApiController.Project,
      '',
      project,
    );

    return response.data;
  }

  public async update(project: ProjectForUpdate): Promise<void> {
    await this.api.put(ApiController.Project, '', project);
  }

  public async refine(id: string): Promise<void> {
    await this.api.put<void, null>(ApiController.Project, `refine/${id}`);
  }

  public async delete(id: string): Promise<void> {
    await this.api.delete(ApiController.Project, id);
  }
}
