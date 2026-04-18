import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';

import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { routes } from './app.routes';
import { NOTYF, notyfFactory } from './core/configs/notyf.injection-token';
import { authInterceptor } from './core/http-intercecptors/auth.interceptor';
import { loadingInterceptor } from './core/http-intercecptors/loading.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(withInterceptors([authInterceptor, loadingInterceptor])),
    { provide: NOTYF, useFactory: notyfFactory },
  ],
};
