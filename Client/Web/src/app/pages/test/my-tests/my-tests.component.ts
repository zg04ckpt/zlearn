import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { TestItem } from '../../../entities/test/test-item.entity';
import { Router, RouterLink } from '@angular/router';
import { TestDetail } from '../../../entities/test/test-detail.entity';
import { TestService } from '../../../services/test.service';
import { ComponentService } from '../../../services/component.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import moment from 'moment';
import { concatMap } from 'rxjs';
import { TestResult } from '../../../entities/test/test-result.entity';
import { DatePipe } from '@angular/common';
import { Title } from '@angular/platform-browser';
import { BreadcrumbService } from '../../../services/breadcrumb.service';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-my-tests',
  standalone: true,
  imports: [
    RouterLink,
    DatePipe
  ],
  templateUrl: './my-tests.component.html',
  styleUrl: './my-tests.component.css'
})
export class MyTestsComponent implements OnInit {
  list1: TestDetail[] = [];
  list2: TestItem[] = [];
  list3: TestResult[] = [];
  destroyRef = inject(DestroyRef);
  title: string = "Quản lý đề";
  defaultImageUrl = environment.defaultImageUrl;

  constructor(
    private testService: TestService,
    private componentService: ComponentService,
    private router: Router,
    private titleService: Title,
    private breadcrumbService: BreadcrumbService,
  ) {}

  ngOnInit(): void {
    this.showCreatedTests();
    this.breadcrumbService.addBreadcrumb(this.title, this.router.url);
    this.titleService.setTitle(this.title);
  }

  timeFormat(duration: number): string {
    return Math.floor(duration/60).toString().padStart(2, '0') + 'm:'
    + Math.floor(duration%60).toString().padStart(2, '0') + 's';
  } 

  back() {
    history.back();
  }

  showCreatedTests() {
    this.titleService.setTitle("Đề đã tạo - ZLEARN")
    this.list2 = [];
    this.list3 = [];
    this.componentService.$showLoadingStatus.next(true);
    this.testService.getAllMyTests()
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe({
      next: res => {
        this.componentService.$showLoadingStatus.next(false);
        this.list1 = res;
        this.sort("byName");
      },

      error: res => {
        this.componentService.$showLoadingStatus.next(false);
        this.componentService.displayAPIError(res);
      },

      complete: () => this.componentService.$showLoadingStatus.next(false)
    });
  }

  showSavedTests() {
    this.titleService.setTitle("Đề đã lưu - ZLEARN")
    this.list1 = [];
    this.list3 = [];
    this.componentService.$showLoadingStatus.next(true);
    this.testService.getAllSavedTests()
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe({
      next: res => {
        this.componentService.$showLoadingStatus.next(false);
        this.list2 = res;
      },

      error: res => {
        this.componentService.$showLoadingStatus.next(false);
        this.componentService.displayAPIError(res);
      },

      complete: () => this.componentService.$showLoadingStatus.next(false)
    });
  }

  showTestResults() {
    this.titleService.setTitle("Lịch sử làm đề - ZLEARN")
    this.list1 = [];
    this.list2 = [];
    this.testService.getResultsByUserId()
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe(next => {
      this.list3 = next;
      console.log(this.list3);
      
      this.list3.sort((a, b) => {
        const d1 = new Date(a.startTime);
        const d2 = new Date(b.startTime);
        return d2.getTime() - d1.getTime();
      });
      this.componentService.$showLoadingStatus.next(false);
    });
  }

  sort(option: string) {
    if(option == "byName") {
      this.list1.sort((a, b) => a.name.localeCompare(b.name));
    } else if (option == "byCreatedDate") {
      this.list1.sort((a, b) => {
        return moment(b.createdDate, 'DD/MM/YYYY').valueOf()
        - moment(a.createdDate, 'DD/MM/YYYY').valueOf()
      });
    } else if (option == "byNumOfAttempts") {
      this.list1.sort((a, b) => b.numberOfAttempts - a.numberOfAttempts)
    }
  }

  removeSavedTest(id: string) {
    this.componentService.displayConfirmMessage("Xác nhận xóa khỏi DS lưu?", () => {
      this.componentService.$showLoadingStatus.next(true);
      this.testService.removeSavedTest(id)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: res => {
          this.componentService.$showLoadingStatus.next(false);
          this.showSavedTests();
        },

        error: res => {
          this.componentService.$showLoadingStatus.next(false);
          this.componentService.displayAPIError(res);
        },

        complete: () => this.componentService.$showLoadingStatus.next(false)
      })
    });
  }

  deleteTest(id: string) {
    this.componentService.displayConfirmMessage("Xác nhận xóa?", () => {
      this.componentService.$showLoadingStatus.next(true);
      this.testService.delete(id)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: res => {
          this.componentService.$showLoadingStatus.next(false);
          this.componentService.$showToast.next("Xóa thành công!");
          this.showCreatedTests();
        },

        error: res => {
          this.componentService.$showLoadingStatus.next(false);
          this.componentService.displayAPIError(res);
        },

        complete: () => this.componentService.$showLoadingStatus.next(false)
      });
    });
  }

  navigate(url: string) {
    this.router.navigateByUrl("tests/" + url);
  }

  dateFormatter(date: Date): string {
    const now = Date.now();
    const past = date.getTime();
    const duration = (now - past) / 1000;
  
    if (duration < 60) {
      return Math.floor(duration) + " giây trước";
    } else if (duration < 3600) {
      return Math.floor(duration / 60) + " phút trước";
    } else if (duration < 86400) {
      return Math.floor(duration / 3600) + " giờ trước";
    } else {
      return Math.floor(duration / 86400) + " ngày trước";
    }
  }
}
