import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { lastValueFrom, map, Observable } from "rxjs";
import { APIResult } from "../dtos/common/api-result.dto";

@Injectable({
    providedIn: 'root'
})
export class FileService {
    constructor(private http: HttpClient) {}

    saveImage(image: File): Promise<string> {
        const form = new FormData();
        form.append('image', image);
        return lastValueFrom(this.http
            .post<APIResult<string>>(`files/images/save`, form)
            .pipe(map(res => res.data!))
        );
    }

    updateImage(url: string, image: File): Promise<string> {
        const form = new FormData();
        form.append('image', image);
        form.append('url', url);
        return lastValueFrom(this.http
            .post<APIResult<string>>('files/images/update', form)
            .pipe(map(res => res.data!))
        );
    }
}