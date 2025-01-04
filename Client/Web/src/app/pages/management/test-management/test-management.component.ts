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
import { CommentService } from '../../../services/comment.service';
import { CommentDTO } from '../../../dtos/comment/comment.dto';
import { DocumentItemDTO } from '../../../dtos/document/document-item';
import { CategoryItem } from '../../../entities/common/category-item.entity';

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
  
  isShowCommentsOfTest = false;
  comments: CommentDTO[] = [];

  categories: CategoryItem[] = [];
  testCate: string = ''

  constructor(
    private componentService: ComponentService,
    private testService: TestService,
    private userService: UserService,
    private router: Router,
    private titleService: Title,
    private breadcrumbService: BreadcrumbService,
    private commentService: CommentService
  ) {}

  ngOnInit(): void {
    this.search();
    this.titleService.setTitle(this.title);
    //get categories
    this.testService.getCategories().subscribe(next => {
      this.componentService.$showLoadingStatus.next(false);
      this.categories = next;
    });
  }

  showUserDetail(userId: string) {
    this.userService.$showInfo.next(userId);
  }

  search() {
    this.testService.getAllInfos(this.pageIndex, this.pageSize, this.key, this.testCate)
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

  showCommentsOfTest(testId: string) {
    this.commentService.getAllCommentsOfTest(testId).subscribe(next => {
      this.comments = next;
      this.componentService.$showLoadingStatus.next(false);
      this.isShowCommentsOfTest = true;
    });
  }

  removeComment(commentId: string) {
    this.componentService.displayConfirmMessage("Xác nhận xóa bình luận này?", () => {
      this.commentService.removeComment(commentId).subscribe(next => {
        this.componentService.$showLoadingStatus.next(false);
        this.componentService.displayMessage("Xóa thành công!");
        this.comments = this.comments.filter(e => e.id != commentId);
      });
    });
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

