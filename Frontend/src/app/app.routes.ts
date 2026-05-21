import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth-guard';
import { Login } from './features/login/login';
import { ProjectWizzard } from './features/project-wizzard/project-wizzard';
import { Projects } from './features/projects/projects';

export const routes: Routes = [
  { path: 'login', component: Login },
  { path: 'projects', component: Projects, canActivate: [authGuard] },
  { path: 'project-wizzard', component: ProjectWizzard, canActivate: [authGuard] },
  { path: '**', redirectTo: 'login' },
];
