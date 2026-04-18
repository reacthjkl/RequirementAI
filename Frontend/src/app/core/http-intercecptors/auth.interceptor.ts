import {
  HttpErrorResponse,
  HttpHandlerFn,
  HttpInterceptorFn,
  HttpRequest,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, from, switchMap, throwError } from 'rxjs';
import { Auth } from '../../shared/services/auth';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(Auth);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => handleError(error, req, next, auth)),
  );
};

function handleError(
  error: HttpErrorResponse,
  originalReq: HttpRequest<unknown>,
  next: HttpHandlerFn,
  auth: Auth,
) {
  if (error.status === 401 && !originalReq.url.includes('auth')) {
    return from(auth.refreshTokenOrLogout()).pipe(
      switchMap((success) => {
        return success ? next(originalReq) : throwError(() => error);
      }),
    );
  }

  return throwError(() => error);
}
