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
  user: User|null = null;
  comments: CommentDTO[] = [];
  title: string = "";

  constructor(
    private activatedRoute: ActivatedRoute,
    private componentService: ComponentService,
    private testService: TestService,
    private router: Router,
    private location: Location,
    private userService: UserService,
    private commentService: CommentService,
    private titleService: Title,
    private breadcrumbService: BreadcrumbService
  ) {  }

  ngOnInit(): void {
    this.userService.$currentUser.subscribe(next => this.user = next);

    this.id = this.activatedRoute.snapshot.paramMap.get('id');
    if(this.id == null) {
      this.componentService.displayMessage("Không tìm thấy đề");
      return;
    }

    this.componentService.$showLoadingStatus.next(true);
    this.testService.getDetail(this.id)
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

    if(this.userService.getLoggedInUser() != null) {
      //check if saved
      this.testService.isSaved(this.id)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(res => {
          debugger
          this.isSaved = res;
          this.componentService.$showLoadingStatus.next(false);
      });
    }

    //comment
    this.getComments();
    
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

  like(comment: CommentDTO) {
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
}
