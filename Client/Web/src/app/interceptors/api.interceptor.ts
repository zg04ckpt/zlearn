import { HttpInterceptorFn } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { catchError, EMPTY, switchMap, throwError } from 'rxjs';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { ComponentService } from '../services/component.service';
import { AuthService } from '../services/auth.service';
import { StorageKey, StorageService } from '../services/storage.service';

export const apiInterceptor: HttpInterceptorFn = (req, next) => {
  
  const thirdPartyUrls = [
    'https://api.vietqr.io/v2/banks'
  ];
  // Check if the request URL is in the list of third-party URLs
  if (thirdPartyUrls.some(url => req.url.includes(url))) {
    return next(req); 
  }

  req = req.clone({
    url: `${environment.baseUrl}/api/${req.url}`
  });
  const router = inject(Router);
  const commonService = inject(ComponentService);
  const authService = inject(AuthService);
  const storageService = inject(StorageService);
  const componentService = inject(ComponentService);

  commonService.$show503.next(false);
  commonService.$show403.next(false);
  commonService.$show404.next(false);
  return next(req).pipe(catchError(res => {
    componentService.$showLoadingStatus.next(false);
    //api not response
    if(res.status == 0) {
      commonService.$show503.next(true);
      componentService.closeAllComponent();
      return EMPTY;
    }
    //forbidden
    if(res.status == 403) {
      commonService.$show403.next(true);
      return EMPTY;
    }
    //unauthorized
    if(res.status === 401) {
      //if user is null, login to continue
      const userData = storageService.get(StorageKey.user);
      if(userData == null) {
        authService.showLoginRequirement();
        return EMPTY;
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
            authService.showEndLoginSessionMessage();
            componentService.$showLoadingStatus.next(false);
            return EMPTY;
          })
        );
      }
    }
    commonService.displayAPIError(res.error);
    return EMPTY;
  }));
};
