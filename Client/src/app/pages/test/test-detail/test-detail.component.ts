import { Component, DestroyRef, inject, OnInit } from '@angular/core';
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
export class TestDetailComponent implements OnInit {
  id: string|null = null;
  data: TestDetail|null = null;
  mode: string = "practice";
  destroyRef = inject(DestroyRef);
  isSaved: boolean = false;
  user: User|null = null;
  comments: CommentDTO[] = [];
  //comments

  constructor(
    private activatedRoute: ActivatedRoute,
    private componentService: ComponentService,
    private testService: TestService,
    private router: Router,
    private location: Location,
    private userService: UserService,
    private commentService: CommentService,
    private titleService: Title
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
        this.titleService.setTitle(`${this.data!.name} - ZLEARN`);
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

    // for(let i = 0; i < 10; i++) {
    //   this.comments.push({
    //     id: 'id',
    //     content: "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Lorem ipsum dolor sit amet consectetur adipisicing elit. Pariatur eius quibusdam saepe sunt velit esse deserunt excepturi eaque vitae, cupiditate sint consequuntur aspernatur soluta porro ipsam qui. Quibusdam, inventore iusto! Pariatur blanditiis nulla fuga rem debitis consequuntur expedita! Molestiae natus esse fugit, nulla sint, quasi sequi recusandae praesentium, tempore quia accusamus eaque.",
    //     createdAt: new Date(),
    //     likes: i + 32,
    //     parentId: null,
    //     userName: 'nguyencao142' + i,
    //     userId: 'id',
    //     userAvatar: '',
    //     childsId: [
    //       'id1',
    //       'id1',
    //       'id1',
    //       'id1'
    //     ]
    //   })
    // }

    
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

  goToHome() {
    this.router.navigateByUrl("/");
  }

  back() {
    this.location.back();
  }
}
