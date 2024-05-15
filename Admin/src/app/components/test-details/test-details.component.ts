import { Component } from '@angular/core';
import { TestResultDetail } from 'src/app/interfaces/test-result-detail';
import { TestResultDetailService } from 'src/app/services/test-result-detail.service';
import * as XLSX from 'xlsx';

@Component({
  selector: 'app-test-details',
  templateUrl: './test-details.component.html',
  styleUrls: ['./test-details.component.css']
})
export class TestDetailsComponent {
  list: TestResultDetail[] = []

  constructor(
    private testResultDetailService: TestResultDetailService,
  ) {}

  ngOnInit(): void {
    this.getList();
  }

  getList() {
    this.testResultDetailService.getAll().subscribe(
      (data) => {
        this.list = []
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

  saveDataAsExcel() {
    const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(this.list);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
    XLSX.writeFile(wb, `history_${(new Date()).toLocaleDateString()}.xlsx`);
  }

  deleteAll() {
    this.saveDataAsExcel();
    this.testResultDetailService.deleteAll().subscribe(response => {
      if (response.code !== 200) {
        alert('Failed to delete all data')
      }
      else {
        this.getList();
      }
    });
  }
}
