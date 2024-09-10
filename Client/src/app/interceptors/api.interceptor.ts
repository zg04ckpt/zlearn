import { HttpInterceptorFn } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { catchError, EMPTY, throwError } from 'rxjs';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { ComponentService } from '../services/component.service';

export const apiInterceptor: HttpInterceptorFn = (req, next) => {
  if(!req.url.includes('images')) {
    req = req.clone({
      url: `${environment.baseUrl}/api/${req.url}`
    });
  }
  const router = inject(Router);
  const commonService = inject(ComponentService);
  return next(req).pipe(catchError(err => {
    if(err.status == 0) {
      commonService.$show503.next(true);
      return EMPTY;
    }
    if(err.status == 403) {
      commonService.$show403.next(true);
      return EMPTY;
    }
    return throwError(() => err);
  }));
};
