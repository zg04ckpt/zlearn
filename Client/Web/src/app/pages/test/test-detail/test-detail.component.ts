import { Component, DestroyRef, inject, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ComponentService } from '../../../services/component.service';
import { TestService } from '../../../services/test.service';
import { TestDetail } from '../../../entities/test/test-detail.entity';
import { FormsModule } from '@angular/forms';
import { DatePipe, Location } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { UserService } from '../../../services/user.service';
import { User } from '../../../entities/user/user.entity';
import { CommentDTO } from '../../../dtos/comment/comment.dto';
import { CommentService } from '../../../services/comment.service';
import { Title } from '@angular/platform-browser';
import { BreadcrumbService } from '../../../services/breadcrumb.service';
import { TestItem } from '../../../entities/test/test-item.entity';
import { HomeService } from '../../../services/home.service';
import { Subscription } from 'rxjs';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-test',
  standalone: true,
  imports: [
    RouterLink,
    FormsModule,
    DatePipe
  ],
  templateUrl: './test-detail.component.html',
  styleUrl: './test-detail.component.css'
})
export class TestDetailComponent implements OnInit{
  id: string|null = null;
  data: TestDetail|null = null;
  mode: string = "practice";
  destroyRef = inject(DestroyRef);
  isSaved: boolean = false;
  defaultImageUrl = environment.defaultImageUrl;
  // user: User|null = null;
  comments: CommentDTO[] = [];
  title: string = "";
  currentUserId: string|null = null;
  randomTests: TestItem[] = [];

  constructor(
    private activatedRoute: ActivatedRoute,
    private componentService: ComponentService,
    private testService: TestService,
    private router: Router,
    private location: Location,
    private userService: UserService,
    private commentService: CommentService,
    private titleService: Title,
    private breadcrumbService: BreadcrumbService,
    private homeService: HomeService
  ) { 
    
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.id = params.get('id');
      if(this.id) {
        this.getData();
      }
    })
  }

  getData() {
    //Get current logged in user id
    this.currentUserId = this.userService.getLoggedInUser()?.id || null;
    
    this.id = this.activatedRoute.snapshot.paramMap.get('id');
    if(this.id == null) {
      this.componentService.displayMessage("Không tìm thấy đề");
      return;
    }

    // Get detail
    this.testService.getDetail(this.id!)
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe({
      next: res => {
        this.componentService.$showLoadingStatus.next(false);
        this.data = res;
        this.title = `${this.data!.name}`;
        this.titleService.setTitle(this.title);
        this.breadcrumbService.addBreadcrumb(this.title, this.router.url);
      }
    });

    //check if this test saved
    if(this.userService.getLoggedInUser() != null) {
      this.testService.isSaved(this.id!)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(res => {
          debugger
          this.isSaved = res;
          this.componentService.$showLoadingStatus.next(false);
      });
    }

    //comment
    this.getComments();
    // Random 10 tests
    this.homeService.getRandomTest(10).subscribe(res => {
      debugger
      this.randomTests = res;
      this.componentService.$showLoadingStatus.next(false);
    });
  }

  getComments() {
    this.commentService.getAllCommentsOfTest(this.id!).subscribe(next => {
      this.comments = next;
      this.componentService.$showLoadingStatus.next(false);
    });
  }

  comment(content: string) {
    this.commentService.sendComment({
      content: content,
      parentId: null,
      testId: this.id!
    }).subscribe(next => {
      this.getComments();
      this.componentService.$showLoadingStatus.next(false);
    });
  }

  removeComment(commentId: string) {
    this.componentService.displayConfirmMessage("Xác nhận xóa bình luận này?", () => {
      this.commentService.removeComment(commentId).subscribe(next => {
        this.componentService.$showLoadingStatus.next(false);
        this.componentService.displayMessage("Xóa thành công!");
        this.getComments();
      });
    });
  }

  like(comment: CommentDTO) {
    if(comment.userId == this.currentUserId) {
      return;
    }
    this.commentService.like(comment.id).subscribe(next => {
      comment.likes++;
      this.componentService.$showLoadingStatus.next(false);
    });
  }

  checkPrivacy(): boolean {
    if(this.data?.isPrivate) {
      const currentUser = this.userService.getLoggedInUser();
      if(this.data.authorId != currentUser?.id) {
        return false;
      }
    }
    return true;
  }

  saveThisTest() {
    this.componentService.$showLoadingStatus.next(true);
    this.testService.saveTest(this.id!)
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe({
      next: res => {
        this.componentService.$showLoadingStatus.next(false);
        this.isSaved = true;
      },

      error: res => {
        this.componentService.$showLoadingStatus.next(false);
        this.componentService.displayAPIError(res);
      },

      complete: () => this.componentService.$showLoadingStatus.next(false)
    });
  }

  showInfo(userId: string) {
    this.userService.$showInfo.next(userId);
  }

  goToHome() {
    this.router.navigateByUrl("/");
  }

  back() {
    this.location.back();
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
