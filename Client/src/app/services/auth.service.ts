import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { StorageKey, StorageService } from "./storage.service";
import { map, Observable, Subject, tap } from "rxjs";
import { LoginDTO } from "../dtos/login.dto";
import { TokenDTO } from "../dtos/token.dto";
import { RegisterDTO } from "../dtos/register.dto";
import { UserDTO } from "../dtos/user.dto";
import { UserMapper } from "../mappers/user.mapper";
import { ConfirmEmailDTO } from "../dtos/confirm-email.dto";
import { User } from "../entities/user.entity";
import { UserService } from "./user.service";
import { ComponentService } from "./component.service";
import { Router } from "@angular/router";
import { Location } from "@angular/common";

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    constructor(
        private http: HttpClient,
        private storageService: StorageService,
        private userService: UserService,
        private componentService: ComponentService,
        private router: Router,
        private location: Location
    ) { }

    login(data: LoginDTO): Observable<User> {
        this.purgeAuth();
        const userMapper = new UserMapper;
        return this.http
            .post<UserDTO>(`auth/login`, data)
            .pipe(
                tap(res => {
                    this.storageService.save(StorageKey.accessToken, res.accessToken);
                    this.storageService.save(StorageKey.refreshToken, res.refreshToken);
                    this.storageService.save(StorageKey.expirationTime, res.expirationTime);
                    this.storageService.save(StorageKey.user, JSON.stringify(userMapper.map(res)));
                    return res;
                }),
                map(userMapper.map)
            );
    }

    register(data: RegisterDTO): Observable<void> {
        this.purgeAuth();
        return this.http.post<void>(`auth/register`, data);
    }

    logout(): Observable<void> {
        return this.http.post<void>(`auth/logout`, null);
    }

    refreshToken(): Observable<TokenDTO> {
        const accToken = this.storageService.get(StorageKey.accessToken);
        const refToken = this.storageService.get(StorageKey.refreshToken);

        return this.http
        .post<TokenDTO>(`auth/refresh-token`, {
            accessToken: accToken,
            refreshToken: refToken
        })
        .pipe(tap(res => {
            this.storageService.save(StorageKey.accessToken, res.accessToken);
            this.storageService.save(StorageKey.refreshToken, res.refreshToken);
        }));
    }

    confirmEmail(data: ConfirmEmailDTO): Observable<void> {
        this.purgeAuth();
        return this.http.get<void>(
            `auth/email-confirm?id=${data.userId}&token=${data.token}`
        );
    }

    purgeAuth() {
        this.storageService.remove(StorageKey.accessToken);
        this.storageService.remove(StorageKey.refreshToken);
        this.storageService.remove(StorageKey.expirationTime);
        this.storageService.remove(StorageKey.user);
        this.userService.$currentUser.next(null);
    }

    setLoginSessionTimer() {
        const timeString = this.storageService.get(StorageKey.expirationTime);
        if(timeString) {
            const expirationTime = new Date(timeString);
            const current = new Date();
            
            if(expirationTime <= current)
                this.showEndLoginSessionMessage();
            else {
                setTimeout(
                    () => this.showEndLoginSessionMessage(),
                    expirationTime.getTime() - current.getTime()
                );
            }
        }
    }

    showEndLoginSessionMessage() {
        this.purgeAuth();
        this.componentService.displayMessageWithActions(
            "Phiên đăng nhập hết hạn, vui lòng đăng nhập lại!",
            [
              { name: "Đăng nhập", action: () => this.componentService.$showLoginDialog.next(true) },
              { name: "Về trang chủ", action: () => this.router.navigateByUrl("/home") }
            ]
        );
    }

    showLoginRequirement() {
        this.componentService.displayMessageWithActions(
            "Vui lòng đăng nhập để tiếp tục!",
            [
              { name: "Đăng nhập", action: () => this.componentService.$showLoginDialog.next(true) },
              { name: "Quay lại", action: () => this.location.back() }
            ]
        );
    }
}