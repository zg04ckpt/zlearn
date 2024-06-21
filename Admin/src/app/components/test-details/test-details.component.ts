import { Component } from '@angular/core';
import { TestResultDetail } from 'src/app/interfaces/test-result-detail';
import { DecryptService } from 'src/app/services/decrypt.service';
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
    private decryptService: DecryptService
  ) {}

  ngOnInit(): void {
    this.getList();
  }

  getList() {
    this.testResultDetailService.getAll().subscribe(
      (data) => {
        this.list = []
        const res = this.decryptService.decrypt(data.data) as any[];
        res.forEach((item) => {
          this.list.push({
            id: item.Id,
            score: item.Score,
            correctsCount: item.CorrectsCount,
            usedTime: {
              minutes: item.UsedTime.Minutes,
              seconds: item.UsedTime.Seconds
            },
            startTime: item.StartTime,
            userInfo: item.UserInfo,
            questionSetId: item.QuestionSetId
          })
        })
      },error => {
        if(error.status == 401)
        {
          alert("You must login to do this action");
          localStorage.removeItem('token');
          window.location.href = "/login";``
        }
        else if(error.status == 403)
        {
          alert("You don't have permission to view this page");
          window.location.href = "/";
        }
        else
          alert("Error: " + JSON.stringify(error));
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
