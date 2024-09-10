import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { TestItem } from '../../../entities/test/test-item.entity';
import { Router, RouterLink } from '@angular/router';
import { TestDetail } from '../../../entities/test/test-detail.entity';
import { TestService } from '../../../services/test.service';
import { ComponentService } from '../../../services/component.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import moment from 'moment';
import { concatMap } from 'rxjs';

@Component({
  selector: 'app-my-tests',
  standalone: true,
  imports: [
    RouterLink
  ],
  templateUrl: './my-tests.component.html',
  styleUrl: './my-tests.component.css'
})
export class MyTestsComponent implements OnInit {
  list1: TestDetail[] = [];
  list2: TestItem[] = [];
  list3: TestDetail[] = [];
  destroyRef = inject(DestroyRef)

  constructor(
    private testService: TestService,
    private componentService: ComponentService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.showCreatedTests();
  }

  showCreatedTests() {
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
}
