import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { lastValueFrom, map, Observable } from "rxjs";
import { APIResult } from "../dtos/common/api-result.dto";
import { FileRequestDTO, FileResponseDTO } from "../dtos/common/file.dto";

@Injectable({
    providedIn: 'root'
})
export class FileService {
    constructor(private http: HttpClient) {}

    saveFile(data: FormData): Promise<APIResult<FileResponseDTO[]>> {
        return lastValueFrom(this.http
            .post<APIResult<FileResponseDTO[]>>(`files/images`, data));
    }

    updateImage(data: FormData): Promise<APIResult<FileResponseDTO[]>> {
        return lastValueFrom(this.http
            .put<APIResult<FileResponseDTO[]>>('files/images', data));
    }
}