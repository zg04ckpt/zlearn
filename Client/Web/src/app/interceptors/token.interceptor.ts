import { HttpInterceptorFn } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { StorageKey, StorageService } from '../services/storage.service';
import { catchError, EMPTY, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { ComponentService } from '../services/component.service';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const storageService = inject(StorageService);
  const accessToken = storageService.get(StorageKey.accessToken);
  const componentService = inject(ComponentService);
  if(accessToken)
  {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${accessToken}`
      }
    })
  }
  componentService.$showLoadingStatus.next(true);
  return next(req);
};
