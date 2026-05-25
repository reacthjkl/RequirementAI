import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { Api } from '../../core/services/api';
import { ApiController } from '../enums/api-controller.enum';
import { ApiResponse } from '../models/api-response.model';
import { ProjectForCreation } from '../models/project-for-creation.model';
import { ProjectForUpdate } from '../models/project-for-update.model';
import { ProjectQualityOverview } from '../models/project-quality-overview.model';
import { Project } from '../models/project.model';

@Injectable({
  providedIn: 'root',
})
export class ProjectService {
  private readonly projectsChangedSubject = new Subject<void>();
  public readonly projectsChanged$ = this.projectsChangedSubject.asObservable();

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

    if (response.successful) {
      this.projectsChangedSubject.next();
    }

    return response.data;
  }

  public async update(project: ProjectForUpdate): Promise<void> {
    await this.api.put(ApiController.Project, '', project);
    this.projectsChangedSubject.next();
  }

  public async getQualityOverview(projectId: string): Promise<ProjectQualityOverview | null> {
    const response: ApiResponse<ProjectQualityOverview | null> = await this.api.get(
      ApiController.Project,
      `${projectId}/overview`,
    );

    return response.data;
  }

  public async analyze(projectId: string): Promise<void> {
    await this.api.put(ApiController.Project, `analyze/${projectId}`);
  }

  public async refine(id: string, customInstructions: string | null): Promise<void> {
    await this.api.put<string | null, null>(
      ApiController.Project,
      `refine/${id}`,
      customInstructions,
    );
  }

  public async delete(id: string): Promise<void> {
    await this.api.delete(ApiController.Project, id);
    this.projectsChangedSubject.next();
  }
}
