import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth-guard';
import { Login } from './features/login/login';
import { ProjectBoard } from './features/project-board/project-board';
import { ProjectOverview } from './features/project-overview/project-overview';
import { ProjectPersonaEditor } from './features/project-persona-editor/project-persona-editor';
import { ProjectPersonas } from './features/project-personas/project-personas';
import { ProjectScenarioEditor } from './features/project-scenario-editor/project-scenario-editor';
import { ProjectScenarios } from './features/project-scenarios/project-scenarios';
import { ProjectSettings } from './features/project-settings/project-settings';
import { ProjectWizard } from './features/project-wizard/project-wizard';
import { Projects } from './features/projects/projects';

export const routes: Routes = [
  { path: 'login', component: Login },
  { path: 'projects', component: Projects, canActivate: [authGuard] },
  { path: 'projects/wizard', component: ProjectWizard, canActivate: [authGuard] },
  { path: 'projects/wizard/:projectId', component: ProjectWizard, canActivate: [authGuard] },
  { path: 'projects/:projectId', redirectTo: 'projects/:projectId/overview' },
  { path: 'projects/:projectId/overview', component: ProjectOverview, canActivate: [authGuard] },
  { path: 'projects/:projectId/personas', component: ProjectPersonas, canActivate: [authGuard] },
  {
    path: 'projects/:projectId/personas/new',
    component: ProjectPersonaEditor,
    canActivate: [authGuard],
  },
  {
    path: 'projects/:projectId/personas/:personaId/edit',
    component: ProjectPersonaEditor,
    canActivate: [authGuard],
  },
  { path: 'projects/:projectId/scenarios', component: ProjectScenarios, canActivate: [authGuard] },
  {
    path: 'projects/:projectId/scenarios/new',
    component: ProjectScenarioEditor,
    canActivate: [authGuard],
  },
  {
    path: 'projects/:projectId/scenarios/:scenarioId/edit',
    component: ProjectScenarioEditor,
    canActivate: [authGuard],
  },
  { path: 'projects/:projectId/board', component: ProjectBoard, canActivate: [authGuard] },
  { path: 'projects/:projectId/settings', component: ProjectSettings, canActivate: [authGuard] },
  { path: 'board/:projectId', redirectTo: 'projects/:projectId/board' },
  { path: 'project-wizard', redirectTo: 'projects/wizard' },
  { path: 'project-wizard/:projectId', component: ProjectWizard, canActivate: [authGuard] },
  { path: '**', redirectTo: 'login' },
];
