import { Component } from '@angular/core';
import { QuestionSet } from 'src/app/models/question-set';
import { QuestionSetService } from 'src/app/services/question-set.service';

@Component({
  selector: 'app-list-question-set',
  templateUrl: './list-question-set.component.html',
  styleUrls: ['./list-question-set.component.css']
})
export class ListQuestionSetComponent {
  list: QuestionSet[] = [];

  constructor(
    private questionSetService: QuestionSetService
  ) { }

  ngOnInit() {
    this.questionSetService.getAll().subscribe(response => {
      if(response.code !== 200) 
          return alert('Failed to fetch data');
      response.data.forEach(item => {
        this.list.push({
          id: item.id,
          name: item.name,
          description: item.description,
          imageUrl: item.imageUrl,
          creator: item.creator,
          createdDate: item.createdDate,
          updatedDate: item.updatedDate,
          numberOfQuestions: item.questionCount,
          testTime: item.testTime
        });
      });
    });
  }
}
