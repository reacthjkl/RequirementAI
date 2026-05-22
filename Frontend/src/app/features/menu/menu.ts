import { Component, effect } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faAnglesLeft, faPlus, faRightFromBracket } from '@fortawesome/free-solid-svg-icons';
import { NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { CurrentUserService } from '../../core/services/current-user.service';
import { MenuDropdown } from '../../shared/components/menu-dropdown/menu-dropdown';
import { ENTITY_COLLECTION_ICONS, ENTITY_ICONS } from '../../shared/icons/entity-icons';
import { Project } from '../../shared/models/project.model';
import { User } from '../../shared/models/user.model';
import { AuthService } from '../../shared/services/auth';
import { ProjectService } from '../../shared/services/project';

type ProjectPageKey = 'overview' | 'personas' | 'scenarios' | 'board' | 'settings';

@Component({
  selector: 'app-menu',
  imports: [RouterModule, FontAwesomeModule, NgbTooltip, MenuDropdown],
  templateUrl: './menu.html',
  styleUrl: './menu.scss',
})
export class Menu {
  private readonly menuMinimizedStorageKey = 'requirement-ai-menu-minimized';

  public currentUser?: User;
  public projects: Project[] = [];
  public currentProject?: Project;
  public currentProjectPage: ProjectPageKey = 'overview';
  public isMinimized = localStorage.getItem(this.menuMinimizedStorageKey) === 'true';

  // icons
  public readonly entityCollectionIcons = ENTITY_COLLECTION_ICONS;
  public readonly entityIcons = ENTITY_ICONS;
  public readonly faAnglesLeft = faAnglesLeft;
  public readonly faPlus = faPlus;
  public readonly faRightFromBracket = faRightFromBracket;

  public readonly projectPages = [
    { key: 'overview', label: 'Overview', icon: ENTITY_ICONS.overview },
    { key: 'personas', label: 'Personas', icon: ENTITY_COLLECTION_ICONS.personas },
    { key: 'scenarios', label: 'Scenarios', icon: ENTITY_COLLECTION_ICONS.scenarios },
    { key: 'board', label: 'Board', icon: ENTITY_ICONS.board },
  ] as const;

  constructor(
    private readonly userSvc: CurrentUserService,
    private readonly authSvc: AuthService,
    private readonly projectSvc: ProjectService,
    private readonly router: Router,
  ) {
    effect(() => {
      this.router.currentNavigation();
      this.syncCurrentProject();
    });
  }

  public async ngOnInit(): Promise<void> {
    const [user, projects] = await Promise.all([this.userSvc.get(), this.projectSvc.get()]);
    this.currentUser = user ?? undefined;
    this.projects = projects;
    this.syncCurrentProject();
  }

  public async logout(): Promise<void> {
    await this.authSvc.logout();
    this.currentUser = undefined;
    await this.router.navigate(['/login']);
  }

  public async switchProject(project: Project): Promise<void> {
    await this.router.navigate(['/projects', project.id, this.currentProjectPage]);
  }

  public toggleMinimized(): void {
    this.setMinimized(!this.isMinimized);
  }

  public expandFromMinimized(event: MouseEvent): void {
    if (!this.isMinimized) {
      return;
    }

    event.preventDefault();
    event.stopPropagation();
    this.setMinimized(false);
  }

  private setMinimized(isMinimized: boolean): void {
    this.isMinimized = isMinimized;
    localStorage.setItem(this.menuMinimizedStorageKey, String(this.isMinimized));
  }

  public get otherProjects(): Project[] {
    return this.projects.filter((project) => project.id !== this.currentProject?.id);
  }

  public userInitials(user: User): string {
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
