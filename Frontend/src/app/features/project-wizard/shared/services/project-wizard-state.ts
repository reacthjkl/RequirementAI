import { Injectable, signal } from '@angular/core';
import { Project } from '../../../../shared/models/project.model';

@Injectable({
  providedIn: 'root',
})
export class ProjectWizardState {
  readonly project = signal<Project | null>(null);

  setProject(project: Project): void {
    this.project.set(project);
  }

  getProjectId(): string | null {
    return this.project()?.id ?? null;
  }

  clear(): void {
    this.project.set(null);
  }
}
