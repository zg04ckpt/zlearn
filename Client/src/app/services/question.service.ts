import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class QuestionService {
  constructor(private httpClient: HttpClient) { }
  baseUrl = environment.baseUrl;

  getAllById(id: string) : Observable<any> {
    return this.httpClient.get<any>(
      `${this.baseUrl}/api/questions/${id}`
    );
  }
}
