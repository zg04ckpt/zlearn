import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { apiInterceptor } from '../interceptors/api.interceptor';
import { tokenInterceptor } from '../interceptors/token.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptors([
      apiInterceptor,
      tokenInterceptor
    ])),
    provideRouter(routes)
  ]
};
