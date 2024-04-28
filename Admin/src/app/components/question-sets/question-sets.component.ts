import { Component } from '@angular/core';
import { QuestionSetsModule } from 'src/app/models/question-sets.module';
import { QuestionSetsService } from 'src/app/services/question-sets.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'question-sets',
  templateUrl: './question-sets.component.html',
  styleUrls: ['./question-sets.component.css']
})
export class QuestionSetsComponent {
  questionSet: QuestionSetsModule = new QuestionSetsModule("", "", "", "", "", new Date(), new Date(), 0);
  questionSetsList: QuestionSetsModule[] = [];

  constructor(
    private service: QuestionSetsService,
    private modalService: NgbModal
  ) { 
    
  }

  ngOnInit(): void {
    this.service.getAll().subscribe(
      data => {
        this.questionSetsList = this.service.convertToListQuestionSet(data)
        this.questionSetsList.forEach(x => x.imageUrl = environment.baseUrl + x.imageUrl)
      },
      error => alert("Error: " + JSON.stringify(error))
  )}

  deleteQuestionSet(id: string) 
  {
    if(confirm("Are you sure you want to delete this question set?"))
    {
      this.service.delete(id).subscribe(
        data => {
          this.questionSetsList = this.service.convertToListQuestionSet(data)
          alert("Delete successfully! Please reload the page to see the changes.")
        },
        error => alert("Error: " + JSON.stringify(error))
      )
    }
  }

  showDetail(item:QuestionSetsModule) {
    this.questionSet = item;
  }
}
