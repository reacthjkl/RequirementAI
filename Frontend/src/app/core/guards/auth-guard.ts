import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AuthService } from '../../shared/services/auth';

export const authGuard: CanActivateFn = (route, state) => {
  const auth: AuthService = inject(AuthService);

  return auth.checkIsLoggedIn();
};
