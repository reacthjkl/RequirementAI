import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth-guard';
import { Login } from './features/login/login';
import { ProjectBoard } from './features/project-board/project-board';
import { ProjectWizard } from './features/project-wizard/project-wizard';
import { Projects } from './features/projects/projects';

export const routes: Routes = [
  { path: 'login', component: Login },
  { path: 'projects', component: Projects, canActivate: [authGuard] },
  { path: 'board/:projectId', component: ProjectBoard, canActivate: [authGuard] },
  { path: 'projects/wizard', component: ProjectWizard, canActivate: [authGuard] },
  { path: 'projects/wizard/:projectId', component: ProjectWizard, canActivate: [authGuard] },
  { path: 'project-wizard', redirectTo: 'projects/wizard' },
  { path: 'project-wizard/:projectId', component: ProjectWizard, canActivate: [authGuard] },
  { path: '**', redirectTo: 'login' },
];
