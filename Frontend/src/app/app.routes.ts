import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth-guard';
import { Login } from './features/login/login';
import { ProjectWizard } from './features/project-wizard/project-wizard';
import { Projects } from './features/projects/projects';

export const routes: Routes = [
  { path: 'login', component: Login },
  { path: 'projects', component: Projects, canActivate: [authGuard] },
  { path: 'project-wizard', component: ProjectWizard },
  { path: 'project-wizard/:projectId', component: ProjectWizard, canActivate: [authGuard] },
  { path: '**', redirectTo: 'login' },
];
