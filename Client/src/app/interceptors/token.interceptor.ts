import { HttpInterceptorFn } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { StorageKey, StorageService } from '../services/storage.service';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const storageService = inject(StorageService);
  const authService = inject(AuthService);

  const accessToken = storageService.get(StorageKey.accessToken);
  if(accessToken)
  {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${accessToken}`
      }
    })
  }

  return next(req).pipe(catchError(err => {
    debugger;
    if(err.status === 401) {

      //if user is null, login to continue
      const userData = storageService.get(StorageKey.user);
      if(userData == null) {
        authService.showLoginRequirement();
        return throwError(() => null);
      } 
      else { //if user isn't null, try refresh access token
        return authService.refreshToken().pipe(

          switchMap(res => {
            //retry send request with new access token
            req = req.clone({
              setHeaders: {
                Authorization: `Bearer ${res.accessToken}`
              }
            });
            return next(req);
          }),

          catchError(res => {
            //error => login or redirect to home
            debugger;
            authService.showEndLoginSessionMessage();
            return throwError(() => res);
          })
        );
      }
      
    }

    return throwError(() => err);
  }));
};
