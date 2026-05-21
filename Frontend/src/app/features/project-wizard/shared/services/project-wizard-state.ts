import { Injectable, signal } from '@angular/core';
import { Persona } from '../../../../shared/models/persona.model';
import { Project } from '../../../../shared/models/project.model';
import { Scenario } from '../../../../shared/models/scenario.model';
import { UserStory } from '../../../../shared/models/user-story.model';

@Injectable({
  providedIn: 'root',
})
export class ProjectWizardState {
  readonly project = signal<Project | null>(null);
  readonly personas = signal<Persona[]>([]);
  readonly scenarios = signal<Scenario[]>([]);
  readonly userStories = signal<UserStory[]>([]);

  setProject(project: Project): void {
    this.project.set(project);
  }

  setPersonas(personas: Persona[]): void {
    this.personas.set(personas);
  }

  setScenarios(scenarios: Scenario[]): void {
    this.scenarios.set(scenarios);
  }

  setUserStories(userStories: UserStory[]): void {
    this.userStories.set(userStories);
  }

  getProjectId(): string | null {
    return this.project()?.id ?? null;
  }

  clear(): void {
    this.project.set(null);
    this.personas.set([]);
    this.scenarios.set([]);
    this.userStories.set([]);
  }
}
