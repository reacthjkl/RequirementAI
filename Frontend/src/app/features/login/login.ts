import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faLock } from '@fortawesome/free-solid-svg-icons';
import { ApiResponse } from '../../shared/models/api-response.model';
import { AuthRequest } from '../../shared/models/auth-request.model';
import { AuthService } from '../../shared/services/auth';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, FontAwesomeModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  //icons
  public readonly lock = faLock;

  public readonly loginForm = new FormGroup({
    email: new FormControl<string>('', {
      validators: [Validators.required, Validators.maxLength(255)],
    }),
    password: new FormControl<string>('', {
      validators: [Validators.required, Validators.maxLength(255)],
    }),
  });

  constructor(
    private readonly auth: AuthService,
    private readonly router: Router,
  ) {}

  public async login(): Promise<void> {
    if (this.loginForm.invalid) return;

    const value: AuthRequest = this.loginForm.value as AuthRequest;

    const result: ApiResponse<null> = await this.auth.authenticate(value);

    if (!result.successful) {
      this.loginForm.setErrors({ wrongCredentials: 'Invalid email or password.' });
      return;
    }

    await this.router.navigate(['projects']);
  }
}
