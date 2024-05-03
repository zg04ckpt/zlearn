import { Component } from '@angular/core';
import { Subscription, interval } from 'rxjs';
import { Question } from 'src/app/models/question';
import { QuestionSet } from 'src/app/models/question-set';
import { Test, TestStatus } from 'src/app/models/test';
import { TestTime } from 'src/app/models/test-time';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

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
    id: '1',
    name: 'Đề 1',
    description: 'Test 1 description',
    imageUrl: '',
    creator: 'admin',
    createdDate: new Date(),
    updatedDate: new Date(),
    numberOfQuestions: 50
  }

  questions: Question[] = []

  qTags: any[] = []
  showSelectedTable: boolean = false;
  currentTime: TestTime = { minutes: 0, seconds: 0 }
  subscription: Subscription = new Subscription();

  constructor(
    private location: Location,
    private route: ActivatedRoute,
  ) { }

  ngOnInit() {
    this.qSet.id = this.route.snapshot.paramMap.get('id')!;

    this.test = {
      duration: { minutes: 1, seconds: 15 },
      status: TestStatus.InProgress,
      result: null
    }

    this.currentTime = {...this.test.duration};

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
            this.markTest();
          }
        }
      }
    });

    for (let i=0; i<this.qSet.numberOfQuestions; i++) {
      this.qTags.push(i+1);
    }

    //seed
    for (let i=0; i<this.qSet.numberOfQuestions; i++) {
      this.questions.push({
        order: i+1,
        content: `Câu ${i+1}: Nội dung câu hỏi ${i+1}`,
        answerA: 'Đáp án A',
        answerB: 'Đáp án B',
        answerC: 'Đáp án C',
        answerD: 'Đáp án D',
        correctAnswer: 1,
        selectedAnswer: 0,
        mark: false
      })
    }
  }

  back() {
    this.location.back();
  }

  markTest() {
    let corrects = 0;
    this.questions.forEach(q => {
      if (q.selectedAnswer === q.correctAnswer) {
        corrects++;
      }
    });

    console.log(this.test.duration);

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
  }

  confirm() {
    if(confirm('Bạn có chắc chắn muốn kết thúc?')) {
      this.test.status = TestStatus.Completed;
      this.subscription.unsubscribe();
      this.markTest();
    }
  }
}
