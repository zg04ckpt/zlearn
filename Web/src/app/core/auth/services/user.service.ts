import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, distinctUntilChanged, map, pipe, tap } from "rxjs";
import { User } from "../models/user.model";
import { HttpClient } from "@angular/common/http";
import { JwtService } from "./jwt.service";
import { Route, Router } from "@angular/router";
import { LoginResponse } from "../models/login-response.model";
import { LoginRequest } from "../models/login-request.model";
import { RegisterRequest } from "../models/register-request.model";
import { RegisterResponse } from "../models/register-response.model";
import { ConfirmEmailResponse } from "../models/confirm-email-response.model";
import { RefreshTokenResponse } from "../models/refresh-token-response.model";
import { environment } from "src/environments/environment";
import { BaseResponse } from "../../models/base-response.model";
import { UserDetail } from "../models/user-detail.model";

@Injectable({ providedIn: 'root' })
export class UserService {
    //using for sharing user info across components
    private currentUserSubject = new BehaviorSubject<User|null>(null);
    
    //instance
    public currentUser = this.currentUserSubject
        .asObservable()
        .pipe(distinctUntilChanged());//only emit when user info has a diff
    
    public isAuthenticated = this.currentUser
        .pipe(map(user => !!user)); //if user != null => authenticated
    
    constructor(
        private readonly http: HttpClient,
        private readonly jwtService: JwtService,
        private readonly router: Router
    ) {}

    login(
        request: LoginRequest
    ): Observable<LoginResponse> {
        
        //remove all token
        this.purgeAuth();

        return this.http.post<LoginResponse>(
            `users/login`,
            request,
        ).pipe(tap(res => {
            const data = res.data!;

            //save token
            this.jwtService.save(
                data.accessToken, 
                data.refreshToken
            );

            //save current user
            this.currentUserSubject.next({
                id: data.id,
                username: data.userName,
                email: data.email,
                roles: data.roles
            } as User);
        }));
    }

    register(
        request: RegisterRequest
    ): Observable<RegisterResponse> {
        //remove all token
        this.purgeAuth();

        return this.http.post<RegisterResponse>(
            'users/register',
            request
        );
    }

    confirmEmail(
        id: string, 
        token: string
    ): Observable<ConfirmEmailResponse> {
        this.purgeAuth();

        return this.http.get<ConfirmEmailResponse>(
            `users/email-confirm?id=${id}&token=${token}`
        );
    }

    refreshAccessToken(): Observable<RefreshTokenResponse> {
        
        return this.http.post<RefreshTokenResponse>(
            'users/refresh-token',
            this.jwtService.get()
        );
    }

    getDetail(id: string): Observable<BaseResponse<UserDetail>> {
        return this.http.get<BaseResponse<UserDetail>>(
            `users/${id}/detail`
        );
    }

    updateDetail(id: string, data: UserDetail): Observable<BaseResponse<null>> {
        return this.http.put<BaseResponse<null>>(
            `users/${id}`,
            data
        );
    }

    logout() {
        return this.http.get('users/logout');
    }

    //remove authenticated status
    purgeAuth() {
        this.jwtService.remove();
        this.currentUserSubject.next(null);
    }
}