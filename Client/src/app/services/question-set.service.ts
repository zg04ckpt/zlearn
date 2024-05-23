import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { QuestionSetsResponse } from '../models/question-sets.reponse';
import { QuestionSetResponse } from '../models/question-set.response';

@Injectable({
  providedIn: 'root'
})
export class QuestionSetService {
  constructor(private httpClient: HttpClient) { }
  baseUrl = environment.baseUrl;

  getAll() : Observable<QuestionSetsResponse> {
    return this.httpClient.get<QuestionSetsResponse>(
      `${this.baseUrl}/api/question-sets`
    );
  }

  getById(id: string) : Observable<QuestionSetResponse> {
    return this.httpClient.get<QuestionSetResponse>(
      `${this.baseUrl}/api/question-sets/${id}`
    );
  }
}
