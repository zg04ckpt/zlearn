import { Component } from '@angular/core';

@Component({
  selector: 'app-list-question-set',
  templateUrl: './list-question-set.component.html',
  styleUrls: ['./list-question-set.component.css']
})
export class ListQuestionSetComponent {
  list: any[] = [
    { 
      id: 1, 
      name: 'Đề 1',
      creator: 'Admin',
      updated: '24/03/24',
      desc: 'Đây là đề số 1',
      numberOfQuestions: 10
    },
    { 
      id: 2, 
      name: 'Đề 2',
      creator: 'Admin',
      updated: '12/04/24',
      desc: 'Đây là đề số 2',
      numberOfQuestions: 20
    },
    { 
      id: 3, 
      name: 'Đề 3',
      creator: 'Admin',
      updated: '19/08/23',
      desc: 'Đây là đề số 3',
      numberOfQuestions: 40
    },
  ];
}
