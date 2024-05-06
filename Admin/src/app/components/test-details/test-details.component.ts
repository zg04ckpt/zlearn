import { Component } from '@angular/core';
import { TestResultDetail } from 'src/app/interfaces/test-result-detail';
import { TestResultDetailService } from 'src/app/services/test-result-detail.service';

@Component({
  selector: 'app-test-details',
  templateUrl: './test-details.component.html',
  styleUrls: ['./test-details.component.css']
})
export class TestDetailsComponent {
  list: TestResultDetail[] = []

  constructor(
    private testResultDetailService: TestResultDetailService
  ) {}

  ngOnInit(): void {
    this.testResultDetailService.getAll().subscribe(
      (data) => {
        data.data.forEach((item) => {
          this.list.push({
            id: item.id,
            score: item.score,
            correctsCount: item.correctsCount,
            usedTime: {
              minutes: item.usedTime.minutes,
              seconds: item.usedTime.seconds
            },
            startTime: item.startTime,
            userInfo: item.userInfo,
            questionSetId: item.questionSetId
          })
        })
      }
    )
  }
}
