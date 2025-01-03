import { Component, OnInit } from '@angular/core';
import { TestResult } from '../../../entities/test/test-result.entity';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { BreadcrumbService } from '../../../services/breadcrumb.service';
import { ComponentService } from '../../../services/component.service';
import { TestService } from '../../../services/test.service';
import { ShareFunction } from '../../../utilities/share-function.uti';

@Component({
  selector: 'app-test-history',
  standalone: true,
  imports: [],
  templateUrl: './test-history.component.html',
  styleUrl: './test-history.component.css'
})
export class TestHistoryComponent implements OnInit {
  list3: TestResult[] = [];

  uti = new ShareFunction()

  constructor(
    private testService: TestService,
      private componentService: ComponentService,
      private router: Router,
      private titleService: Title,
      private breadcrumbService: BreadcrumbService,
  ) {}

  ngOnInit(): void {
    this.titleService.setTitle("Lịch sử làm đề - ZLEARN")
    this.showTestResults();
  }

  showTestResults() {
    this.testService.getResultsByUserId().subscribe(next => {
      this.list3 = next;
      console.log(next);
      this.componentService.$showLoadingStatus.next(false);
    });
  }

  timeFormat(duration: number): string {
    return Math.floor(duration/60).toString().padStart(2, '0') + 'm:'
    + Math.floor(duration%60).toString().padStart(2, '0') + 's';
  } 
}
