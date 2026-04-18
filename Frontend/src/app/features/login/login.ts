import { Component } from '@angular/core';
import { Auth } from '../../shared/services/auth';

@Component({
  selector: 'app-login',
  imports: [],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  constructor(private auth: Auth) {}
}
