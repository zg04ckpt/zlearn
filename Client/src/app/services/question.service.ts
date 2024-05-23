import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { QuestionsResponse } from '../models/questions.response';

@Injectable({
  providedIn: 'root'
})
export class QuestionService {
  constructor(private httpClient: HttpClient) { }
  baseUrl = environment.baseUrl;

  getAllById(id: string) : Observable<QuestionsResponse> {
    return this.httpClient.get<QuestionsResponse>(
      `${this.baseUrl}/api/questions/${id}`
    );
  }
}
