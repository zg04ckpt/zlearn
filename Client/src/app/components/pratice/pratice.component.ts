import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription, interval } from 'rxjs';
import { Question } from 'src/app/models/question';
import { QuestionSet } from 'src/app/models/question-set';
import { Test, TestStatus } from 'src/app/models/test';
import { TestTime } from 'src/app/models/test-time';
import { QuestionSetService } from 'src/app/services/question-set.service';
import { QuestionService } from 'src/app/services/question.service';
import { TestResultService } from 'src/app/services/test-result.service';
import { Location } from '@angular/common';
import { TestResultRequest } from 'src/app/models/test-result.request';

@Component({
  selector: 'app-pratice',
  templateUrl: './pratice.component.html',
  styleUrls: ['./pratice.component.css']
})
export class PraticeComponent {
  test: Test = {
    duration: { minutes: 0, seconds: 0 },
    status: TestStatus.InProgress,
    result: null
  }
  qSet: QuestionSet = {
    id: '',
    name: '',
    description: '',
    imageUrl: '',
    creator: '',
    createdDate: new Date(),
    updatedDate: new Date(),
    attemptCount: 0,
    questionCount: 0,
    testTime: { minutes: 0, seconds: 0 }
  }
  startTime: Date = new Date();
  questions: Question[] = []
  showSelectedTable: boolean = true;
  currentTime: TestTime = { minutes: 0, seconds: 0 }
  subscription: Subscription = new Subscription();

  constructor(
    private location: Location,
    private route: ActivatedRoute,
    private questionSetService: QuestionSetService,
    private questionService: QuestionService,
    private testResultService: TestResultService
  ) { }

  ngOnInit() {
    this.qSet.id = this.route.snapshot.paramMap.get('id')!;

    this.questionSetService.getById(this.qSet.id).subscribe(response => {
      if(response.code !== 200) 
          return alert('Failed to fetch data');
      const res = this.questionSetService.decrypt(response.data) as any;
      this.qSet = {
        id: res.Id,
        name: res.Name,
        description: res.Description,
        imageUrl: res.ImageUrl,
        creator: res.Creator,
        createdDate: res.CreatedDate,
        updatedDate: res.UpdatedDate,
        attemptCount: res.AttemptCount,
        questionCount: res.QuestionCount,
        testTime: { 
          minutes: res.TestTime.Minutes, 
          seconds: res.TestTime.Seconds 
        }
      };

      this.test.duration = this.qSet.testTime;
      this.currentTime = {...this.test.duration};
    });

    this.test = {
      duration: { 
        minutes: 1, 
        seconds: 15 
      },
      status: TestStatus.InProgress,
      result: null
    }

    // this.subscription = interval(1000).subscribe(x => {
    //   if (this.test.status === TestStatus.InProgress) {
    //     if (this.currentTime.seconds > 0) {
    //       this.currentTime.seconds--;
    //     } else {
    //       if (this.currentTime.minutes > 0) {
    //         this.currentTime.minutes--;
    //         this.currentTime.seconds = 59;
    //       } else {
    //         this.test.status = TestStatus.Completed;
    //         this.subscription.unsubscribe();
    //         this.showResult();
    //       }
    //     }
    //   }
    // });

    this.questionService.getAllById(this.qSet.id).subscribe(response => {
      if(response.code !== 200) 
          return alert('Failed to fetch data');
      const res = this.questionSetService.decrypt(response.data) as any[];
      res.forEach(item => {
        this.questions.push({
          order: item.Order,
          content: item.Content,
          imageUrl: null,
          answerA: item.AnswerA,
          answerB: item.AnswerB,
          answerC: item.AnswerC,
          answerD: item.AnswerD,
          correctAnswer: item.CorrectAnswer,
          selectedAnswer: 0,
          mark: item.Mark
        });
      });
      this.questions.sort(() => Math.random() - 0.5);
    });
  }

  back() {
    this.location.back();
  }

  showResult() {
    let corrects = 0;
    this.questions.forEach(q => {
      if (q.selectedAnswer === q.correctAnswer) {
        corrects++;
      }
    });

    let used_seconds = 
      this.test.duration.minutes * 60 + this.test.duration.seconds
      - this.currentTime.minutes * 60 - this.currentTime.seconds;

    this.test.result = {
      score: parseFloat((10.0 * corrects / this.qSet.questionCount).toFixed(2)),
      corrects: corrects,
      used_time: {
        minutes:  parseInt((used_seconds / 60).toString()),
        seconds: used_seconds % 60
      }
    }

    this.scrollToItem(1);
  }

  scrollToItem(index: number) {
    const element = document.getElementById(index.toString());
    const rect = element!.getBoundingClientRect();
    const offset = 150; // Đổi số này để thay đổi độ lệch
    window.scrollTo({
      top: rect.top + window.pageYOffset - offset,
      behavior: "smooth"
    });
  }

  confirm() {
    if(confirm('Bạn có chắc chắn muốn kết thúc?')) {
      this.test.status = TestStatus.Completed;
      // this.subscription.unsubscribe();
      this.showResult();
    }
  }
}
