import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { PagingResultDTO } from "../dtos/common/paging-result.dto";
import { TestItem } from "../entities/test/test-item.entity";
import { TestDetail } from "../entities/test/test-detail.entity";
import { Test } from "../entities/test/test.entity";
import { MarkTestDTO } from "../dtos/test/mark-test.dto";
import { TestResultDTO } from "../dtos/test/test-result.dto";
import { CreateTestDTO } from "../dtos/test/create-test.dto";

@Injectable({
    providedIn: 'root'
})
export class TestService {
    constructor(
        private http: HttpClient
    ) {}

    getAll(
        pageIndex: number, 
        pageSize: number, 
        key: String
    ): Observable<PagingResultDTO<TestItem>> {
        return this.http.get<PagingResultDTO<TestItem>>(
            `tests?pageIndex=${pageIndex}&pageSize=${pageSize}&key=${key}`,
        );
    }


    getDetail(id: string): Observable<TestDetail> {
        return this.http.get<TestDetail>(`tests/${id}/detail`);
    }


    getContent(id: string): Observable<Test> {
        return this.http.get<Test>(`tests/${id}/content`);
    }

    markTest(data: MarkTestDTO): Observable<TestResultDTO> {
        return this.http.post<TestResultDTO>(
            `tests/mark-test`,
            data
        );
    }

    create(data: CreateTestDTO): Observable<void> {
        return this.http.post<void>(`tests`, data);
    }
}