import { Component } from '@angular/core';
import { Subscription, interval } from 'rxjs';
import { QuestionSet } from 'src/app/models/question-set';
import { QuestionSetResponse } from 'src/app/models/question-set.response';
import { QuestionSetService } from 'src/app/services/question-set.service';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-list-question-set',
  templateUrl: './list-question-set.component.html',
  styleUrls: ['./list-question-set.component.css']
})
export class ListQuestionSetComponent {
  list: QuestionSet[] = [];
  baseUrl: string = environment.baseUrl;
  constructor(
    private questionSetService: QuestionSetService
  ) { }

  ngOnInit() {

    this.questionSetService.getAll().subscribe(response => {
      if(response.code !== 200) 
          return alert('Failed to fetch data');
      const res = this.questionSetService.decrypt(response.data) as any[];

      res.forEach(item => {
        this.list.push({
          id: item.Id,
          name: item.Name,
          description: item.Description,
          imageUrl: this.baseUrl + item.ImageUrl,
          creator: item.Creator,
          createdDate: item.CreatedDate,
          updatedDate: item.UpdatedDate,
          attemptCount: item.AttemptCount,
          questionCount: item.QuestionCount,
          testTime: item.TestTime
        });
      });

      
    });
  }

  
}
