import { Component } from '@angular/core';
import { QuestionSetsModule } from 'src/app/models/question-sets.module';
import { QuestionSetsService } from 'src/app/services/question-sets.service';

@Component({
  selector: 'question-sets',
  templateUrl: './question-sets.component.html',
  styleUrls: ['./question-sets.component.css']
})
export class QuestionSetsComponent {
  questionSetsList: QuestionSetsModule[] = [];

  constructor(private service: QuestionSetsService) 
  { 
    
  }

  ngOnInit(): void {
    this.service.getAll().subscribe(data => {
      this.questionSetsList = this.service.convertToListQuestionSet(data);
    });
  }

  deleteQuestionSet(id: string) 
  {
    if(confirm("Are you sure you want to delete this question set?"))
    {
      alert("Đã xóa thành công !");
      //handle with api
    }
  }
}
