import { Component } from '@angular/core';
import { QuestionSet } from 'src/app/models/question-set';
import { QuestionSetService } from 'src/app/services/test.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-list',
  templateUrl: './test-list.component.html',
  styleUrls: ['./test-list.component.css']
})
export class TestListComponent {
  list: QuestionSet[] = [];
  baseUrl: string = environment.baseUrl;
  constructor(
    private testService: QuestionSetService
  ) { }

  ngOnInit() {
    this.testService.getAll().subscribe(response => {
      if(response.code !== 200) 
          return alert('Failed to fetch data');
      const res = this.testService.decrypt(response.data) as any[];

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
