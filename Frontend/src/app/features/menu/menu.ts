import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faFolderOpen, faUserCircle } from '@fortawesome/free-solid-svg-icons';
import { CurrentUserService } from '../../core/services/current-user.service';
import { User } from '../../shared/models/user.model';

@Component({
  selector: 'app-menu',
  imports: [RouterModule, FontAwesomeModule],
  templateUrl: './menu.html',
  styleUrl: './menu.scss',
})
export class Menu {
  public currentUser?: User;

  // icons
  faFolderOpen = faFolderOpen;
  faUserCircle = faUserCircle;

  constructor(private userSvc: CurrentUserService) {}

  async ngOnInit() {
    this.currentUser = (await this.userSvc.get()) ?? undefined;
  }
}
