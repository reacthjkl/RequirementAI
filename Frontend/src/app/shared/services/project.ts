import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { Api } from '../../core/services/api';
import { ApiController } from '../enums/api-controller.enum';
import { ApiResponse } from '../models/api-response.model';
import { ProjectForCreation } from '../models/project-for-creation.model';
import { ProjectForUpdate } from '../models/project-for-update.model';
import { ProjectQualityOverview } from '../models/project-quality-overview.model';
import { Project } from '../models/project.model';
import { QualityAnalysisJob } from '../models/quality-analysis-job.model';

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

  public async analyze(projectId: string): Promise<string> {
    const response = await this.api.put<null, string>(
      ApiController.Project,
      `analyze/${projectId}`,
    );

    if (!response.successful || !response.data) {
      throw new Error(response.message || 'Could not start quality analysis');
    }

    return response.data;
  }

  public async getQualityAnalysisJob(
    jobId: string,
  ): Promise<ApiResponse<QualityAnalysisJob | null>> {
    try {
      return await this.api.get<QualityAnalysisJob | null>(ApiController.QualityAnalysis, jobId);
    } catch (error) {
      if (error instanceof HttpErrorResponse && this.isApiResponse(error.error)) {
        return error.error as ApiResponse<QualityAnalysisJob | null>;
      }

      throw error;
    }
  }

  public async refine(id: string, customInstructions: string | null): Promise<void> {
    await this.api.put<{ customInstructions: string | null } | null, null>(
      ApiController.Project,
      `refine/${id}`,
      { customInstructions },
    );
  }

  public async delete(id: string): Promise<void> {
    await this.api.delete(ApiController.Project, id);
    this.projectsChangedSubject.next();
  }

  private isApiResponse(value: unknown): value is ApiResponse<unknown> {
    return (
      typeof value === 'object' &&
      value !== null &&
      'successful' in value &&
      typeof value.successful === 'boolean'
    );
  }
}
