import { Injectable } from "@angular/core";
import { Key } from "../models/key.model";

@Injectable({ providedIn: 'root' })
export class JwtService {
    get(): {
        accessToken: string | null,
        refreshToken: string | null
    } {
        return {
            accessToken: window.localStorage[Key.ACCESS_TOKEN],
            refreshToken: window.localStorage[Key.REFRESH_TOKEN]
        }
    }

    save(accessToken: string, refreshToken: string) {
        window.localStorage[Key.ACCESS_TOKEN] = accessToken;
        window.localStorage[Key.REFRESH_TOKEN] = refreshToken;
    }

    remove(): void {
        window.localStorage.removeItem(Key.ACCESS_TOKEN);
        window.localStorage.removeItem(Key.REFRESH_TOKEN);
    }
}