import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { Auth } from '../../shared/services/auth';

export const authGuard: CanActivateFn = (route, state) => {
  const auth: Auth = inject(Auth);

  return auth.checkIsLoggedIn();
};
