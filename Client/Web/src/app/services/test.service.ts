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
import { CategoryItem } from "../entities/management/category-item.entity";

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

    public getAll(
        pageIndex: number, 
        pageSize: number, 
        key: string,
        cate: string
    ): Observable<PagingResultDTO<TestItem>> {
        return this.http
            .get<APIResult<PagingResultDTO<TestItem>>>(
                `tests?pageIndex=${pageIndex}&pageSize=${pageSize}&name=${key}&categorySlug=${cate}`,
            ).pipe(
                map(res => res.data!),
                map(res => {
                    res.data.forEach(x => {
                        x.imageUrl = x.imageUrl? this.baseUrl + x.imageUrl : null;
                    });
                    return res;
                })
            );
    }

    public getAllInfos(
        pageIndex: number, 
        pageSize: number, 
        key: string,
        cate: string
    ): Observable<PagingResultDTO<TestDetail>> {
        return this.http.get<APIResult<PagingResultDTO<TestDetail>>>(
            `tests/all-info?pageIndex=${pageIndex}&pageSize=${pageSize}&name=${key}&categorySlug=${cate}`,
        ).pipe(
            map(res => res.data!),
            map(res => {
                res.data.forEach(x => {
                    x.imageUrl = x.imageUrl? this.baseUrl + x.imageUrl : null;
                    x.createdDate = new Date(x.createdDate);
                    x.updatedDate = new Date(x.updatedDate);
                });
                return res;
            })
        );
    }

    public getAllMyTests(): Observable<TestDetail[]> {
        return this.http.get<APIResult<TestDetail[]>>(
            `tests/my-tests`,
        ).pipe(
            map(res => {
                return res.data!;
            }),
            map(res => {
                res.forEach(x => {
                    x.imageUrl = x.imageUrl? this.baseUrl + x.imageUrl : null;
                    x.updatedDate = new Date(x.updatedDate);
                    x.createdDate = new Date(x.createdDate);
                });
                
                return res;
            })
        );
    }

    public getDetail(id: string): Observable<TestDetail> {
        return this.http.get<APIResult<TestDetail>>(
            `tests/${id}/detail`
        ).pipe(
            map(res => {
                return res.data!;
            }),
            map(res => {
                if(res.imageUrl) {
                    res.imageUrl = this.baseUrl + res.imageUrl;
                }
                res.updatedDate = new Date(res.updatedDate);
                res.createdDate = new Date(res.createdDate);
                return res;
            })
        );
    }

    public getContent(id: string): Observable<Test> {
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

    public getCategories(): Observable<CategoryItem[]> {
        return this.http
            .get<APIResult<CategoryItem[]>>(`tests/categories`)
            .pipe(map(res => res.data!));
    }

    public markTest(data: MarkTestDTO): Observable<MarkTestResultDTO> {
        return this.http.post<APIResult<MarkTestResultDTO>>(
            `tests/mark-test`,
            data
        ).pipe(map(res => {
            return res.data!;
        }));
    }

    public create2(data: CreateTestDTO): Observable<void> {
        const formData = new FormData();
        formData.append(`name`, data.name);
        if(data.image) {
            formData.append(`image`, data.image);
        }
        formData.append(`description`, data.description);
        formData.append(`source`, data.source);
        formData.append(`duration`, data.duration.toString());
        formData.append(`categorySlug`, data.categorySlug);
        formData.append(`isPrivate`, data.isPrivate.toString());
        data.questions.forEach((q, i) => {
            formData.append(`questions[${i}].content`, q.content);
            if(q.image) {
                formData.append(`questions[${i}].image`, q.image);
            }
            formData.append(`questions[${i}].answerA`, q.answerA);
            formData.append(`questions[${i}].answerB`, q.answerB);
            if(q.answerC) {
                formData.append(`questions[${i}].answerC`, q.answerC);
            }
            if(q.answerD) {
                formData.append(`questions[${i}].answerD`, q.answerD);
            }
            formData.append(`questions[${i}].correctAnswer`, q.correctAnswer.toString());
        });
        
        return this.http
            .post<APIResult<void>>(`tests`, formData)
            .pipe(map(res => res.data!));
    }

    public update2(id: string, data: UpdateTestDTO): Observable<void> {
        const formData = new FormData();
        formData.append(`name`, data.name);
        if(data.image) {
            formData.append(`image`, data.image);
        }
        formData.append(`description`, data.description);
        formData.append(`source`, data.source);
        formData.append(`duration`, data.duration.toString());
        formData.append(`categorySlug`, data.categorySlug);
        formData.append(`isPrivate`, data.isPrivate.toString());
        data.questions.forEach((q, i) => {
            if(q.id) {
                formData.append(`questions[${i}].id`, q.id);
            }
            formData.append(`questions[${i}].content`, q.content);
            if(q.image) {
                formData.append(`questions[${i}].image`, q.image);
            }
            if(q.imageUrl) {
                formData.append(`questions[${i}].imageUrl`, q.imageUrl);
            }
            formData.append(`questions[${i}].answerA`, q.answerA);
            formData.append(`questions[${i}].answerB`, q.answerB);
            if(q.answerC) {
                formData.append(`questions[${i}].answerC`, q.answerC);
            }
            if(q.answerD) {
                formData.append(`questions[${i}].answerD`, q.answerD);
            }
            formData.append(`questions[${i}].correctAnswer`, q.correctAnswer.toString());
        });
        
        return this.http
            .put<APIResult<void>>(`tests/${id}`, formData)
            .pipe(map(res => res.data!));
    }

    public delete(id: string): Observable<void> {
        return this.http
            .delete<APIResult<void>>(`tests/${id}`)
            .pipe(map(res => {
                return res.data!;
            }));
    }

    public saveTest(testId: string): Observable<void> {
        return this.http.post<APIResult<void>>(
            `tests/save?testId=${testId}`, 
            null
        ).pipe(map(res => {
            return res.data!;
        }));
    }

    public isSaved(testId: string): Observable<boolean> {
        return this.http.get<APIResult<boolean>>(
            `tests/save/isSaved?testId=${testId}`
        ).pipe(map(res => {
            return res.data!;
        }));
    }

    public removeSavedTest(id: string): Observable<void> {
        const params = new HttpParams().set("testId", id);
        return this.http
            .delete<APIResult<void>>('tests/save', { params })
            .pipe(map(res => {
                return res.data!;
            }));       
    }

    public getAllSavedTests(): Observable<TestItem[]> {
        return this.http
        .get<APIResult<TestItem[]>>('tests/save')
        .pipe(map(res => res.data!))
        .pipe(map(res => {
            res.forEach(x => x.imageUrl = x.imageUrl? this.baseUrl + x.imageUrl : null);
            return res;
        }));
    }

    public getUpdateContent(testId: string): Observable<UpdateTestDTO> {
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

    public getImageFile(url: string): Observable<File> {
        return this.http.get(url, { responseType: 'blob' }).pipe(
            map(blob => {
              const fileName = url.split('/').pop() || 'image.jpg';
              return new File([blob], fileName, { type: blob.type });
            })
        );
    }

    public getResultsByUserId(): Observable<TestResult[]> {
        return this.http
        .get<APIResult<TestResult[]>>('tests/my-results')
        .pipe(map(res => {
            debugger
            return res.data!;
        }));
    }
}