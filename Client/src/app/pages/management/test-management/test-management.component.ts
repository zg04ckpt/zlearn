import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { ManagementService } from '../../../services/management.service';
import { TestDetail } from '../../../entities/test/test-detail.entity';
import { FormsModule } from '@angular/forms';
import { ComponentService } from '../../../services/component.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { DatePipe } from '@angular/common';
import { TestService } from '../../../services/test.service';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { BreadcrumbService } from '../../../services/breadcrumb.service';
import { Title } from '@angular/platform-browser';

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
  title: string = "Quản lý đề";

  constructor(
    private managementService: ManagementService,
    private componentService: ComponentService,
    private testService: TestService,
    private userService: UserService,
    private router: Router,
    private titleService: Title,
    private breadcrumbService: BreadcrumbService
  ) {}

  ngOnInit(): void {
    this.search();
    this.titleService.setTitle(this.title);
    this.breadcrumbService.addBreadcrumb(this.title, this.router.url);
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

