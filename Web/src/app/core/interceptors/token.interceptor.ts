import { HttpEvent, HttpHandler, HttpInterceptor, HttpInterceptorFn, HttpRequest } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { JwtService } from "../auth/services/jwt.service";
import { UserService } from "../auth/services/user.service";
import { Observable, catchError, switchMap, throwError } from "rxjs";

@Injectable({ providedIn: 'root' })
export class TokenInterceptor implements HttpInterceptor {
    constructor(
        private userService: UserService,
        private jwtService: JwtService
    ) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const accessToken = this.jwtService.get().accessToken;
        if(accessToken) {
            req = this.addAuthorization(req, accessToken);
        }

        return next.handle(req).pipe(
            catchError(err => {
                //check if the error is due to an expired access token
                if(err.status == 401 && accessToken) {
                    return this.handleExpiredAccessToken(req, next);
                }

                return throwError(() => err);
            })
        );
    }

    private addAuthorization(req: HttpRequest<any>, accessToken: string): HttpRequest<any> {
        return req.clone({
            setHeaders: {
                Authorization: `Bearer ${accessToken}`
            }
        });
    }

    private handleExpiredAccessToken(
        req: HttpRequest<any>, next: HttpHandler
    ): Observable<HttpEvent<any>> {
        //try get new access token
        return this.userService.refreshAccessToken().pipe(
            switchMap(() => {
                req = this.addAuthorization(req, this.jwtService.get().accessToken!);
                //retry the original req with new access token
                return next.handle(req);
            }),
            catchError(err => {
                //refresh token expired => redirect to login page
                return throwError(() => err);
            })
        );
    }
};