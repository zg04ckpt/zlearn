import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TestResultDetail } from '../interfaces/test-result-detail';
import { environment } from 'src/environments/environment';
import { TestResultResponse } from '../interfaces/test-result-response';

@Injectable({
  providedIn: 'root'
})
export class TestResultDetailService {
  constructor(private httpClient: HttpClient) { }

  private baseUrl = environment.baseUrl
  
  getAll(): Observable<TestResultResponse> {
    return this.httpClient.get<TestResultResponse>(
      `${this.baseUrl}/api/test-results`
    );
  }

  deleteAll(): Observable<any> {
    return this.httpClient.delete(`${this.baseUrl}/api/test-results`);
  }
}
