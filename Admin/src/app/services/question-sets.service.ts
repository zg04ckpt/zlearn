import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { QuestionSet } from '../interfaces/questionSet';
import { QuestionSetsModule } from '../models/question-sets.module';

@Injectable({
  providedIn: 'root'
})
export class QuestionSetsService {
  private baseUrl = environment.baseUrl;

  constructor(private httpClient: HttpClient) { }

  getAll() : Observable<any> {
    return this.httpClient.get(`${this.baseUrl}/api/question-sets`);
  }

  getById(id: string): Observable<any> {
    return this.httpClient.get(`${this.baseUrl}/api/question-sets/${id}`);
  }

  create(data: QuestionSet): Observable<any> {
    return this.httpClient.post(`${this.baseUrl}/api/question-sets`, data);
  }

  update(id: string, data: QuestionSet): Observable<any> {
    return this.httpClient.put(`${this.baseUrl}/api/question-sets/${id}`, data);
  }

  delete(id: string): Observable<any> {
    return this.httpClient.delete(`${this.baseUrl}/api/question-sets/${id}`);
  }

  convertToQuestionSet(data: any): QuestionSetsModule[] {
    let questionSets: QuestionSetsModule[] = [];
    data.data.forEach((element: any) => {
      let questionSet: QuestionSetsModule = {
        id: element.id,
        name: element.name,
        desc: element.description,
        creator: "element.creator",
        createdDate: element.createdDate,
        updatedDate: element.createdDate,
        imageUrl: element.image,
        questions: []
      }
      questionSets.push(questionSet);
    });
    return questionSets;
  }

  /**
   *{
      "id": "dea9688f-71f6-42cf-abe1-c3f49c3fd360",
      "name": "test",
      "description": "test",
      "createdDate": "2024-04-24T21:59:07.9913407",
      "image": "test",
      "numberOfQuestions": 1
    }
   */
}
