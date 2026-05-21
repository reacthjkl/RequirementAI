import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { GlobalSpinner } from './core/components/global-spinner/global-spinner';
import { LoadingService } from './core/services/loading-service';
import { AuthService } from './shared/services/auth';
import { Menu } from './features/menu/menu';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, GlobalSpinner, Menu],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  public loggedIn = signal(false);
  public loading = signal(false);

  constructor(
    private auth: AuthService,
    private loadingSvc: LoadingService,
  ) {
    this.loggedIn = this.auth.loggedIn;
    this.loading = this.loadingSvc.isLoading;
  }
}
