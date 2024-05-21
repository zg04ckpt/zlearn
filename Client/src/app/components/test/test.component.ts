import { Component } from '@angular/core';
import { Subscription, interval } from 'rxjs';
import { Question } from 'src/app/models/question';
import { QuestionSet } from 'src/app/models/question-set';
import { Test, TestStatus } from 'src/app/models/test';
import { TestTime } from 'src/app/models/test-time';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { QuestionSetService } from 'src/app/services/question-set.service';
import { QuestionService } from 'src/app/services/question.service';
import { TestResultRequest } from 'src/app/models/test-result.request';
import { TestResultService } from 'src/app/services/test-result.service';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css']
})
export class TestComponent {

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
    numberOfQuestions: 0,
    testTime: { minutes: 0, seconds: 0 }
  }
  startTime: Date = new Date();
  questions: Question[] = []
  showSelectedTable: boolean = false;
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
      this.qSet = {
        id: response.data.id,
        name: response.data.name,
        description: response.data.description,
        imageUrl: response.data.imageUrl,
        creator: response.data.creator,
        createdDate: response.data.createdDate,
        updatedDate: response.data.updatedDate,
        attemptCount: response.data.attemptCount,
        numberOfQuestions: response.data.questionCount,
        testTime: { 
          minutes: response.data.testTime.minutes, 
          seconds: response.data.testTime.seconds 
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

    this.subscription = interval(1000).subscribe(x => {
      if (this.test.status === TestStatus.InProgress) {
        if (this.currentTime.seconds > 0) {
          this.currentTime.seconds--;
        } else {
          if (this.currentTime.minutes > 0) {
            this.currentTime.minutes--;
            this.currentTime.seconds = 59;
          } else {
            this.test.status = TestStatus.Completed;
            this.subscription.unsubscribe();
            this.showResult();
          }
        }
      }
    });

    this.questionService.getAllById(this.qSet.id).subscribe(response => {
      if(response.code !== 200) 
          return alert('Failed to fetch data');
      response.data.forEach(item => {
        this.questions.push({
          order: item.order,
          content: item.content,
          imageUrl: null,
          answerA: item.answerA,
          answerB: item.answerB,
          answerC: item.answerC,
          answerD: item.answerD,
          correctAnswer: item.correctAnswer,
          selectedAnswer: 0,
          mark: item.mark
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
      score: parseFloat((10.0 * corrects / this.qSet.numberOfQuestions).toFixed(2)),
      corrects: corrects,
      used_time: {
        minutes:  parseInt((used_seconds / 60).toString()),
        seconds: used_seconds % 60
      }
    }

    this.sendResultToServer();
  }

  sendResultToServer() {
    let testResult: TestResultRequest = {
      score: this.test.result!.score,
      correctsCount: this.test.result!.corrects,
      usedTime: this.test.result!.used_time,
      startTime: this.startTime,
      userInfo: 'Undefined',
      questionSetId: this.qSet.id
    }

    this.testResultService.create(testResult).subscribe( response => {
      if(response.code !== 200) 
          console.log('Failed to send test result');
    }); 
  }

  confirm() {
    if(confirm('Bạn có chắc chắn muốn kết thúc?')) {
      this.test.status = TestStatus.Completed;
      this.subscription.unsubscribe();
      this.showResult();
    }
  }
}
