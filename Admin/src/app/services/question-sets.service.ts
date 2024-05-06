import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { QSCreateRequest } from '../interfaces/qsCreateRequest';
import { QuestionSetsModule } from '../models/question-sets.module';
import { QuestionModule } from '../models/question.module';
import { QSUpdateRequest } from '../interfaces/qsUpdateRequest';

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

  getQuestions(id: string): Observable<any> {
    return this.httpClient.get(`${this.baseUrl}/api/questions/${id}`);
  }

  getAllQuestions(id: string): Observable<any> {
    return this.httpClient.get(`${this.baseUrl}/api/questions/${id}`);
  }

  create(data: QSCreateRequest): Observable<any> {
    return this.httpClient.post(
      `${this.baseUrl}/api/question-sets`, 
      this.packageQSCreateRequest(data)
    );
  }

  update(id: string, data: QSUpdateRequest): Observable<any> {
    return this.httpClient.put(
      `${this.baseUrl}/api/question-sets/${id}`, 
      this.packageQSUpdateRequest(data)
    );
  }

  delete(id: string): Observable<any> {
    return this.httpClient.delete(`${this.baseUrl}/api/question-sets/${id}`);
  }

  //đóng gói request thành form data
  packageQSCreateRequest(request: QSCreateRequest): FormData {
    const formData = new FormData();
    formData.append("name", request.name);
    formData.append("description", request.description);
    formData.append("creator", request.creator);
    formData.append("image", request.image, request.image.name);
    for(let i=0; i<request.questions.length; i++) {
      formData.append(`questions[${i}].order`, request.questions[i].order.toString());
      formData.append(`questions[${i}].content`, request.questions[i].content);

      if(request.questions[i].image != null)
        formData.append(`questions[${i}].image`, request.questions[i].image!, request.questions[i].image!.name);
      
      formData.append(`questions[${i}].answerA`, request.questions[i].answerA);
      formData.append(`questions[${i}].answerB`, request.questions[i].answerB);
      formData.append(`questions[${i}].answerC`, request.questions[i].answerC);
      formData.append(`questions[${i}].answerD`, request.questions[i].answerD);
      formData.append(`questions[${i}].correctAnswer`, request.questions[i].correctAnswer.toString());
      formData.append(`questions[${i}].mark`, request.questions[i].mark.toString());
    }
    formData.append("testTime.minutes", request.testTime.minutes.toString());
    formData.append("testTime.seconds", request.testTime.seconds.toString());
    return formData;
  }

  //đóng gói request thành form data
  packageQSUpdateRequest(request: QSUpdateRequest): FormData {
    const formData = new FormData();
    formData.append("name", request.name);
    formData.append("description", request.description);

    if(request.image != null)
    formData.append("image", request.image, request.image.name);
  
    for(let i=0; i<request.questions.length; i++) {
      formData.append(`questions[${i}].order`, request.questions[i].order.toString());
      formData.append(`questions[${i}].content`, request.questions[i].content);

      if(request.questions[i].image != null)
        formData.append(`questions[${i}].image`, request.questions[i].image!, request.questions[i].image!.name);
      
      formData.append(`questions[${i}].answerA`, request.questions[i].answerA);
      formData.append(`questions[${i}].answerB`, request.questions[i].answerB);
      formData.append(`questions[${i}].answerC`, request.questions[i].answerC);
      formData.append(`questions[${i}].answerD`, request.questions[i].answerD);
      formData.append(`questions[${i}].correctAnswer`, request.questions[i].correctAnswer.toString());
      formData.append(`questions[${i}].mark`, request.questions[i].mark.toString());
    }
    formData.append("testTime.minutes", request.testTime.minutes.toString());
    formData.append("testTime.seconds", request.testTime.seconds.toString());
    return formData;
  }

  //chuyển dữ liệu từ server thành dữ liệu dùng cho client
  convertToQuestions(data: any): QuestionModule[] {
    data = data.data
    let questions: QuestionModule[] = [];
    data.forEach((element: any) => {
      let question: QuestionModule = {
        order: element.order,
        content: element.content,
        imageUrl: element.image,
        answerA: element.answerA,
        answerB: element.answerB,
        answerC: element.answerC,
        answerD: element.answerD,
        correctAnswer: element.correctAnswer,
        selected: false,
        mark: element.mark,
        changeCorrectAnswer: function (select: number): void {
          this.correctAnswer = select;
        }
      }
      questions.push(question);
    });
    return questions;
  }

  convertToQuestionSet(data: any): QuestionSetsModule {
    data = data.data
    let questionSet: QuestionSetsModule = {
      id: data.id,
      name: data.name,
      description: data.description,
      imageUrl: data.imageUrl,
      creator: data.creator,
      createdDate: data.createdDate,
      updatedDate: data.createdDate,
      questionCount: data.questionCount,
      testTime: {
        minutes: data.testTime.minutes,
        seconds: data.testTime.seconds
      }
    }
    return questionSet;
  }

  convertToListQuestionSet(data: any): QuestionSetsModule[] {
    let questionSets: QuestionSetsModule[] = [];
    data.data.forEach((element: any) => {
      let questionSet: QuestionSetsModule = {
        id: element.id,
        name: element.name,
        description: element.description,
        imageUrl: element.imageUrl,
        creator: element.creator,
        createdDate: element.createdDate,
        updatedDate: element.updatedDate,
        questionCount: element.questionCount,
        testTime: {
          minutes: element.testTime.minutes,
          seconds: element.testTime.seconds
        }
      }
      questionSets.push(questionSet);
    });
    return questionSets;
  }

}
