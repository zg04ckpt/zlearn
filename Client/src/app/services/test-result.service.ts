import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { TestResult } from '../models/test-result';
import { TestResultRequest } from '../models/test-result.request';

@Injectable({
  providedIn: 'root'
})
export class TestResultService {
  constructor(private httpClient: HttpClient) { }
  baseUrl = environment.baseUrl;

  create(data: TestResultRequest) {
    return this.httpClient.post<TestResult>(
      `${this.baseUrl}/api/test-results`,
      data
    );
  }
  
}
