import { Injectable } from "@angular/core";

export enum StorageKey {
    accessToken = "ACCESS-TOKEN",
    refreshToken = "REFRESH-TOKEN",
    expirationTime = "EXPIRATION-TIME",
    user = "USER",
}

@Injectable({
    providedIn: 'root'
})
export class StorageService {

    save(k: string, v: string): void {
        localStorage.setItem(k, v);
    }
    
    get(k: string): string|null {
        return localStorage.getItem(k);
    }

    remove(k: string): void {
        localStorage.removeItem(k);
    }
}