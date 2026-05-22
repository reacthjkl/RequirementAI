import { Component } from '@angular/core';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faFileLines,
  faFolderOpen,
  faGear,
  faHouse,
  faPlus,
  faRightFromBracket,
  faTableColumns,
  faUsers,
} from '@fortawesome/free-solid-svg-icons';
import { filter } from 'rxjs';
import { CurrentUserService } from '../../core/services/current-user.service';
import { MenuDropdown } from '../../shared/components/menu-dropdown/menu-dropdown';
import { Project } from '../../shared/models/project.model';
import { User } from '../../shared/models/user.model';
import { AuthService } from '../../shared/services/auth';
import { ProjectService } from '../../shared/services/project';

type ProjectPageKey = 'overview' | 'personas' | 'scenarios' | 'board' | 'settings';

@Component({
  selector: 'app-menu',
  imports: [RouterModule, FontAwesomeModule, MenuDropdown],
  templateUrl: './menu.html',
  styleUrl: './menu.scss',
})
export class Menu {
  public currentUser?: User;
  public projects: Project[] = [];
  public currentProject?: Project;
  public currentProjectPage: ProjectPageKey = 'overview';

  // icons
  faFileLines = faFileLines;
  faFolderOpen = faFolderOpen;
  faGear = faGear;
  faHouse = faHouse;
  faPlus = faPlus;
  faRightFromBracket = faRightFromBracket;
  faTableColumns = faTableColumns;
  faUsers = faUsers;

  readonly projectPages = [
    { key: 'overview', label: 'Overview', icon: faHouse },
    { key: 'personas', label: 'Personas', icon: faUsers },
    { key: 'scenarios', label: 'Scenarios', icon: faFileLines },
    { key: 'board', label: 'Board', icon: faTableColumns },
  ] as const;

  constructor(
    private userSvc: CurrentUserService,
    private authSvc: AuthService,
    private projectSvc: ProjectService,
    private router: Router,
  ) {
    this.router.events
      .pipe(filter((event): event is NavigationEnd => event instanceof NavigationEnd))
      .subscribe(() => this.syncCurrentProject());
  }

  async ngOnInit() {
    const [user, projects] = await Promise.all([this.userSvc.get(), this.projectSvc.get()]);
    this.currentUser = user ?? undefined;
    this.projects = projects;
    this.syncCurrentProject();
  }

  async logout(): Promise<void> {
    await this.authSvc.logout();
    this.currentUser = undefined;
    await this.router.navigate(['/login']);
  }

  async switchProject(project: Project): Promise<void> {
    await this.router.navigate(['/projects', project.id, this.currentProjectPage]);
  }

  get otherProjects(): Project[] {
    return this.projects.filter((project) => project.id !== this.currentProject?.id);
  }

  userInitials(user: User): string {
    const initials = user.name
      .split(' ')
      .filter(Boolean)
      .slice(0, 2)
      .map((part) => part[0])
      .join('');

    return initials || user.email[0]?.toUpperCase() || '?';
  }

  private syncCurrentProject(): void {
    const projectId = this.findRouteParam('projectId');
    this.currentProjectPage = this.resolveCurrentProjectPage();
    this.currentProject = this.projects.find((project) => project.id === projectId);
  }

  private findRouteParam(paramName: string): string | null {
    let route = this.router.routerState.snapshot.root;

    while (route) {
      const param = route.paramMap.get(paramName);

      if (param) {
        return param;
      }

      route = route.firstChild!;
    }

    return null;
  }

  private resolveCurrentProjectPage(): ProjectPageKey {
    const primarySegments =
      this.router.parseUrl(this.router.url).root.children['primary']?.segments.map((item) => item.path) ??
      [];
    const projectPage = primarySegments.find((segment) =>
      ['overview', 'personas', 'scenarios', 'board', 'settings'].includes(segment),
    );

    if (projectPage) {
      return projectPage as ProjectPageKey;
    }

    return 'overview';
  }
}
