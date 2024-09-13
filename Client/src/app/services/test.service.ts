import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { finalize, map, Observable, tap } from "rxjs";
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

@Injectable({
    providedIn: 'root'
})
export class TestService {
    baseUrl: string = environment.baseUrl;
    constructor(
        private http: HttpClient,
        private componentService: ComponentService
    ) {}

    getAll(
        pageIndex: number, 
        pageSize: number, 
        key: String
    ): Observable<PagingResultDTO<TestItem>> {
        return this.http.get<PagingResultDTO<TestItem>>(
            `tests?pageIndex=${pageIndex}&pageSize=${pageSize}&key=${key}`,
        ).pipe(map(res => {
            res.data.forEach(x => x.imageUrl = this.baseUrl + x.imageUrl);
            return res;
        }));
    }

    getAllMyTests(): Observable<TestDetail[]> {
        return this.http.get<TestDetail[]>(
            `tests/my-tests`,
        ).pipe(map(res => {
            res.forEach(x => x.imageUrl = this.baseUrl + x.imageUrl);
            return res;
        }));
    }

    getDetail(id: string): Observable<TestDetail> {
        return this.http.get<TestDetail>(
            `tests/${id}/detail`
        ).pipe(map(res => {
            res.imageUrl = this.baseUrl + res.imageUrl;
            return res;
        }));
    }

    getContent(id: string): Observable<Test> {
        return this.http.get<Test>(
            `tests/${id}/content`
        ).pipe(map(res => {
            res.questions.forEach(x => x.imageUrl = this.baseUrl + x.imageUrl);
            return res;
        }));
    }

    markTest(data: MarkTestDTO): Observable<MarkTestResultDTO> {
        return this.http.post<MarkTestResultDTO>(
            `tests/mark-test`,
            data
        );
    }
    
    create(data: CreateTestDTO): Observable<void> {
        this.componentService.$showLoadingStatus.next(true);
        const formData = new FormData();
        formData.append('name', data.name);
        if (data.image !== null) {
            formData.append('image', data.image);
        }
        formData.append('description', data.description);
        formData.append('source', data.source);
        formData.append('duration', data.duration.toString());
        data.questions.forEach((value, index) => {
            formData.append(`questions[${index}].content`, value.content);
            if(value.image !== null) {
                formData.append(`questions[${index}].image`, value.image);
            }
            formData.append(`questions[${index}].answerA`, value.answerA);
            formData.append(`questions[${index}].answerB`, value.answerB);
            if(value.answerC !== null) {
                formData.append(`questions[${index}].answerC`, value.answerC);
            }
            if(value.answerD !== null) {
                formData.append(`questions[${index}].answerD`, value.answerD);
            }
            formData.append(`questions[${index}].correctAnswer`, value.correctAnswer.toString());
        });

        return this.http.post<void>(`tests`, formData)
        .pipe(finalize(() => this.componentService.$showLoadingStatus.next(false)));
    }

    update(id: string, data: UpdateTestDTO): Observable<void> {
        const formData = new FormData();
        formData.append('name', data.name);
        if (data.image !== null) {
            formData.append('image', data.image);
        }
        formData.append('description', data.description);
        formData.append('source', data.source);
        formData.append('duration', data.duration.toString());
        data.questions.forEach((value, index) => {
            if(value.id !== null) {
                formData.append(`questions[${index}].id`, value.id);
            }
            formData.append(`questions[${index}].content`, value.content);
            if(value.image !== null) {
                formData.append(`questions[${index}].image`, value.image);
            }
            formData.append(`questions[${index}].answerA`, value.answerA);
            formData.append(`questions[${index}].answerB`, value.answerB);
            if(value.answerC !== null) {
                formData.append(`questions[${index}].answerC`, value.answerC);
            }
            if(value.answerD !== null) {
                formData.append(`questions[${index}].answerD`, value.answerD);
            }
            formData.append(`questions[${index}].correctAnswer`, value.correctAnswer.toString());
        });
        formData.append('isPrivate', data.isPrivate.toString());

        return this.http.put<void>(`tests/${id}`, formData);
    }

    delete(id: string): Observable<void> {
        return this.http.delete<void>(`tests/${id}`);
    }

    saveTest(testId: string): Observable<void> {
        return this.http.post<void>(
            `tests/save?testId=${testId}`, 
            null
        );
    }

    isSaved(testId: string): Observable<boolean> {
        return this.http.get<boolean>(
            `tests/save/isSaved?testId=${testId}`
        );
    }

    removeSavedTest(id: string): Observable<void> {
        const params = new HttpParams().set("testId", id);
        return this.http.delete<void>('tests/save', { params });
    }

    getAllSavedTests(): Observable<TestItem[]> {
        return this.http.get<TestItem[]>('tests/save');
    }

    getUpdateContent(testId: string): Observable<TestUpdateContent> {
        return this.http.get<TestUpdateContent>(
            `tests/${testId}/update-content`
        ).pipe(map(res => {
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
        this.componentService.$showLoadingStatus.next(true);
        return this.http.get<TestResult[]>('tests/my-results')
        .pipe(finalize(() => this.componentService.$showLoadingStatus.next(false)));
    }
}