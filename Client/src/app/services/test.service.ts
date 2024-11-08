import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { finalize, lastValueFrom, map, Observable, pipe, tap } from "rxjs";
import { PagingResultDTO } from "../dtos/common/paging-result.dto";
import { TestItem } from "../entities/test/test-item.entity";
import { TestDetail } from "../entities/test/test-detail.entity";
import { Test } from "../entities/test/test.entity";
import { MarkTestDTO } from "../dtos/test/mark-test.dto";
import { MarkTestResultDTO } from "../dtos/test/test-result.dto";
import { CreateTestDTO } from "../dtos/test/create-test.dto";
import { environment } from "../../environments/environment";
import { TestUpdateContent } from "../dtos/test/update-test-content.dto";
import { UpdateTestDTO } from "../dtos/test/update-test.dts";
import { ComponentService } from "./component.service";
import { TestResult } from "../entities/test/test-result.entity";
import { APIResult } from "../dtos/common/api-result.dto";
import { FileService } from "./file.service";

@Injectable({
    providedIn: 'root'
})
export class TestService {
    baseUrl: string = environment.baseUrl;
    constructor(
        private http: HttpClient,
        private componentService: ComponentService,
        private fileService: FileService
    ) {}

    getAll(
        pageIndex: number, 
        pageSize: number, 
        key: String
    ): Observable<PagingResultDTO<TestItem>> {
        return this.http.get<APIResult<PagingResultDTO<TestItem>>>(
            `tests?pageIndex=${pageIndex}&pageSize=${pageSize}&name=${key}`,
        ).pipe(
            map(res => {
                return res.data!;
            }),
            map(res => {
                res.data.forEach(x => x.imageUrl = this.baseUrl + x.imageUrl);
                return res;
            })
        );
    }

    getAllMyTests(): Observable<TestDetail[]> {
        return this.http.get<APIResult<TestDetail[]>>(
            `tests/my-tests`,
        ).pipe(
            map(res => {
                return res.data!;
            }),
            map(res => {
                res.forEach(x => x.imageUrl = this.baseUrl + x.imageUrl);
                return res;
            })
        );
    }

    getDetail(id: string): Observable<TestDetail> {
        return this.http.get<APIResult<TestDetail>>(
            `tests/${id}/detail`
        ).pipe(
            map(res => {
                return res.data!;
            }),
            map(res => {
                res.imageUrl = this.baseUrl + res.imageUrl;
                return res;
            })
        );
    }

    getContent(id: string): Observable<Test> {
        return this.http.get<APIResult<Test>>(
            `tests/${id}/content`
        ).pipe(
            map(res => {
                return res.data!;
            }),
            map(res => {
                res.questions.forEach(x => x.imageUrl = this.baseUrl + x.imageUrl);
                return res;
            })
        );
    }

    markTest(data: MarkTestDTO): Observable<MarkTestResultDTO> {
        return this.http.post<APIResult<MarkTestResultDTO>>(
            `tests/mark-test`,
            data
        ).pipe(map(res => {
            return res.data!;
        }));
    }
    
    public async create(data: CreateTestDTO): Promise<void> {
        // Update test image
        if(data.image) {
            data.imageUrl = await this.fileService.saveImage(data.image);
        }

        // update question image
        for(var question of data.questions) {
            if(question.image) {
                question.imageUrl = await this.fileService.saveImage(question.image);
            }
        }

        return lastValueFrom(this.http
            .post<APIResult<void>>(`tests`, data)
            .pipe(map(res => res.data!)));
    }

    public async update(id: string, data: UpdateTestDTO): Promise<void> {
        debugger
        
        // Update test image
        if(data.image) {
            if(data.imageUrl) {
                data.imageUrl = await this.fileService.updateImage(data.imageUrl, data.image);
            } else {
                data.imageUrl = await this.fileService.saveImage(data.image);
            }
        }

        // update question image
        for(var question of data.questions) {
            if(question.image) {
                if(question.imageUrl) {
                    question.imageUrl = await this.fileService.updateImage(question.imageUrl, question.image);
                } else {
                    question.imageUrl = await this.fileService.saveImage(question.image);
                }
            }
        }

        return lastValueFrom(this.http
            .put<APIResult<void>>(`tests/${id}`, data)
            .pipe(map(res => res.data!)));
    }

    delete(id: string): Observable<void> {
        return this.http
            .delete<APIResult<void>>(`tests/${id}`)
            .pipe(map(res => {
                return res.data!;
            }));
    }

    saveTest(testId: string): Observable<void> {
        return this.http.post<APIResult<void>>(
            `tests/save?testId=${testId}`, 
            null
        ).pipe(map(res => {
            return res.data!;
        }));
    }

    isSaved(testId: string): Observable<boolean> {
        return this.http.get<APIResult<boolean>>(
            `tests/save/isSaved?testId=${testId}`
        ).pipe(map(res => {
            return res.data!;
        }));
    }

    removeSavedTest(id: string): Observable<void> {
        const params = new HttpParams().set("testId", id);
        return this.http
            .delete<APIResult<void>>('tests/save', { params })
            .pipe(map(res => {
                return res.data!;
            }));       
    }

    getAllSavedTests(): Observable<TestItem[]> {
        return this.http
        .get<APIResult<TestItem[]>>('tests/save')
        .pipe(map(res => {
            return res.data!;
        }));
    }

    getUpdateContent(testId: string): Observable<UpdateTestDTO> {
        return this.http.get<APIResult<UpdateTestDTO>>(
            `tests/${testId}/update-content`
        ).pipe(
            map(res => res.data!),
            map(res => {
            if(res.imageUrl) {
                res.imageUrl = this.baseUrl + res.imageUrl;
            }
            res.questions.forEach(x => {
                if(x.imageUrl) {
                    x.imageUrl = this.baseUrl + x.imageUrl;
                }
            });
            return res;
        }));
    }

    getImageFile(url: string): Observable<File> {
        return this.http.get(url, { responseType: 'blob' }).pipe(
            map(blob => {
              const fileName = url.split('/').pop() || 'image.jpg';
              return new File([blob], fileName, { type: blob.type });
            })
        );
    }

    getResultsByUserId(): Observable<TestResult[]> {
        return this.http
        .get<APIResult<TestResult[]>>('tests/my-results')
        .pipe(map(res => {
            debugger
            return res.data!;
        }));
    }
}