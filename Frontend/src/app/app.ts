import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Auth } from './shared/services/auth';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  public loggedIn = signal(false);

  constructor(private auth: Auth) {
    this.loggedIn = this.auth.loggedIn;
  }
}
