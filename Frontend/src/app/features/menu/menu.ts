import { Component, ElementRef, HostListener } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faChevronUp,
  faFolderOpen,
  faRightFromBracket,
  faUserCircle,
} from '@fortawesome/free-solid-svg-icons';
import { CurrentUserService } from '../../core/services/current-user.service';
import { User } from '../../shared/models/user.model';
import { AuthService } from '../../shared/services/auth';

@Component({
  selector: 'app-menu',
  imports: [RouterModule, FontAwesomeModule],
  templateUrl: './menu.html',
  styleUrl: './menu.scss',
})
export class Menu {
  public currentUser?: User;
  public userMenuOpen = false;

  // icons
  faChevronUp = faChevronUp;
  faFolderOpen = faFolderOpen;
  faRightFromBracket = faRightFromBracket;
  faUserCircle = faUserCircle;

  constructor(
    private elementRef: ElementRef<HTMLElement>,
    private userSvc: CurrentUserService,
    private authSvc: AuthService,
    private router: Router,
  ) {}

  async ngOnInit() {
    this.currentUser = (await this.userSvc.get()) ?? undefined;
  }

  toggleUserMenu(): void {
    this.userMenuOpen = !this.userMenuOpen;
  }

  async logout(): Promise<void> {
    this.userMenuOpen = false;
    await this.authSvc.logout();
    this.currentUser = undefined;
    await this.router.navigate(['/login']);
  }

  @HostListener('document:click', ['$event'])
  closeUserMenuOnOutsideClick(event: MouseEvent): void {
    if (!this.userMenuOpen) {
      return;
    }

    const target = event.target;

    const userMenuElement = this.elementRef.nativeElement.querySelector('.user-menu');

    if (target instanceof Node && userMenuElement?.contains(target)) {
      return;
    }

    this.userMenuOpen = false;
  }
}
