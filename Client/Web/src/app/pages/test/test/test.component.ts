import { Component, DestroyRef, inject, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ComponentService } from '../../../services/component.service';
import { TestStatus } from '../../../enums/test.enum';
import { Test } from '../../../entities/test/test.entity';
import { Observable, Subject } from 'rxjs';
import { DecimalPipe, NgClass, NgStyle } from '@angular/common';
import { MarkTestResultDTO } from '../../../dtos/test/test-result.dto';
import { TestService } from '../../../services/test.service';
import { MarkTestDTO } from '../../../dtos/test/mark-test.dto';
import { CommonService } from '../../../services/common.service';
import { UserService } from '../../../services/user.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CanComponentDeactivate } from '../../../guards/can-deactivate.guard';
import { Title } from '@angular/platform-browser';
import { BreadcrumbService } from '../../../services/breadcrumb.service';


@Component({
  selector: 'app-test-content',
  standalone: true,
  imports: [
    DecimalPipe,
    NgClass,
    NgStyle
  ],
  templateUrl: './test.component.html',
  styleUrl: './test.component.css'
})
export class TestComponent implements OnInit, CanComponentDeactivate {
  status: TestStatus = TestStatus.Loading;
  testId: string|null = null;
  test: Test|null = null;
  remainder: Subject<number>|null = null; //sec
  remainderTime: number = 0; //sec
  answer: MarkTestDTO|null = null;
  result: MarkTestResultDTO|null = null;
  start: Date|null = null;
  end: Date|null = null;
  title: string = "";

  //define
  TestStatus: any = TestStatus;
  Math: any = Math;
  Array: any = Array;
  destroyRef = inject(DestroyRef);

  isShowStatus = true;

  constructor(
    private activatedRoute: ActivatedRoute,
    private componentService: ComponentService,
    private testService: TestService,
    private commonService: CommonService,
    private userService: UserService,
    private router: Router,
    private titleService: Title,
    private breadcrumbService: BreadcrumbService,
    private renderer: Renderer2
  ) {}

  canDeactivate(): Observable<boolean> | Promise<boolean> | boolean {
    if(this.test && this.status != TestStatus.ShowAnswer) {
      return confirm("Bạn chưa nộp bài thi, rời đi sẽ hủy bỏ trạng thái làm bài, xác nhận rời khỏi?");
    }
    return true;
  }

  ngOnInit(): void {
    this.testId = this.activatedRoute.snapshot.paramMap.get('id');
    const option = this.activatedRoute.snapshot.paramMap.get('option');
    if(!option || !this.testId) {
      this.componentService.displayMessage("Đã có lỗi xảy ra");
      return;
    }

    this.testService.getContent(this.testId)
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe(res => {
        this.componentService.$showLoadingStatus.next(false);
        this.test = res;
        this.test.questions.forEach(x => x.selectedAnswer = 0);

        //start
        this.start = new Date;
        this.status = TestStatus.Testing;

        //countDown
        if(option == "testing") {
          this.title = `${this.test.name} - Thi thử`;

          this.remainder = new Subject<number>();
          this.remainderTime = this.test!.duration * 60;
          this.remainder.next(this.remainderTime);
          const subscribeId = setInterval(() => {
            this.remainderTime--;
            this.remainder!.next(this.remainderTime);
          }, 1000);

          //when complete
          this.remainder.subscribe(next => {
            if(next == 0) {
              clearInterval(subscribeId);
              this.endTest();
            }
          });
        } else {
          this.title = `${this.test.name} - Thi thử`;
        }
        this.titleService.setTitle(this.title);
        debugger;
    });

    this.titleService.setTitle("Làm đề - ZLEARN")
  }

  endTest() {
    if(this.status == TestStatus.Testing) {
      this.componentService.displayConfirmMessage("Xác nhận kết thúc?", () => {
        this.status = TestStatus.Completed;
      });
    } else {
      this.status = TestStatus.Completed;
    }
  }

  markTest() {
    debugger
    this.componentService.$showLoadingStatus.next(true);
    this.answer = {
      answers: this.test!.questions.map(x => ({
        id: x.id,
        selected: x.selectedAnswer
      })),
      startTime: this.test!.startTime.toISOString(),
      testId: this.testId!,
      testName: this.test!.name,
    };
    
    this.testService.markTest(this.answer).subscribe(res=> {
      debugger;
      this.componentService.$showLoadingStatus.next(false);
      this.result = res;
      this.status = TestStatus.ShowAnswer;
      this.scrollToItem();
    });
  }

  scrollToItem() {
    const element = document.getElementById('top');
    element?.scrollIntoView({
      behavior: 'smooth',
      block: 'start'
    })
  }

  navigate(url: string) {
    this.router.navigateByUrl(url);
  }

  scrollTo(id: number) {
    const element = document.getElementById(id.toString());
    element?.scrollIntoView({
      behavior: 'smooth',
      block: 'start'
    })
  }
}
