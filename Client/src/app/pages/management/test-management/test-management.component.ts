import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { ManagementService } from '../../../services/management.service';
import { TestDetail } from '../../../entities/test/test-detail.entity';
import { FormsModule } from '@angular/forms';
import { ComponentService } from '../../../services/component.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { DatePipe } from '@angular/common';
import { TestService } from '../../../services/test.service';
import { RouterLink } from '@angular/router';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-test-management',
  standalone: true,
  imports: [
    FormsModule,
    DatePipe,
    RouterLink
  ],
  templateUrl: './test-management.component.html',
  styleUrl: './test-management.component.css'
})
export class TestManagementComponent implements OnInit {
  pageIndex: number = 1;
  pageSize: number = 10;
  totalPage: number = 1;
  totalRecord: number = 0;
  testDetails: TestDetail[] = []
  key: string = "";
  destroyRef = inject(DestroyRef);

  constructor(
    private managementService: ManagementService,
    private componentService: ComponentService,
    private testService: TestService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.search();
  }

  showUserDetail(userId: string) {
    this.userService.$showInfo.next(userId);
  }

  search() {
    this.managementService.getAllTests(this.pageIndex, this.pageSize, this.key)
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe(res => {
      this.componentService.$showLoadingStatus.next(false);
      debugger;
      this.totalRecord = res.total;
      this.totalPage = Math.ceil(this.totalRecord / this.pageSize);
      if(this.totalPage == 0) this.pageIndex = 0;
      this.testDetails = res.data;
    });
  }

  delete(testId: string) {
    this.componentService.displayConfirmMessage("Xác nhận xóa test này", () => {
      this.testService.delete(testId).subscribe(next => {
        this.componentService.$showLoadingStatus.next(false);
        this.componentService.displayMessage("Xóa test thành công!");
        this.search();
      });
    });
  }
}

