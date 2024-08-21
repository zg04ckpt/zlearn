import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ComponentService } from '../../../services/component.service';
import { TestType } from '../../../enums/test.enum';
import { Test } from '../../../entities/test/test.entity';
import { Subject } from 'rxjs';
import { DecimalPipe, NgClass } from '@angular/common';
import { TestResultDTO } from '../../../dtos/test/test-result.dto';

@Component({
  selector: 'app-test-content',
  standalone: true,
  imports: [
    DecimalPipe,
    NgClass
  ],
  templateUrl: './test.component.html',
  styleUrl: './test.component.css'
})
export class TestComponent implements OnInit {
  type: TestType|null = null;
  testId: string|null = null;
  test: Test|null = {
    name: "Tên bài kiểm tra",
    duration: 1,
    questions: [
      {
        content: "Câu hỏi thứ 1",
        imageUrl: null,
        answerA: "Đáp án 1",
        answerB: "Đáp án 2",
        answerC: "Đáp án 3",
        answerD: "Đáp án 4",
        selectedAnswer: 0
      },
      {
        content: "Câu hỏi thứ 2",
        imageUrl: null,
        answerA: "Đáp án 1",
        answerB: "Đáp án 2",
        answerC: null,
        answerD: null,
        selectedAnswer: 0
      },
      {
        content: "Câu hỏi thứ 3",
        imageUrl: "https://images2.thanhnien.vn/528068263637045248/2024/1/25/e093e9cfc9027d6a142358d24d2ee350-65a11ac2af785880-17061562929701875684912.jpg",
        answerA: "Đáp án 1",
        answerB: "Đáp án 2",
        answerC: "Đáp án 3",
        answerD: null,
        selectedAnswer: 0
      },
      {
        content: "Câu hỏi thứ 4",
        imageUrl: null,
        answerA: "Đáp án 1",
        answerB: "Đáp án 2",
        answerC: "Đáp án 3",
        answerD: "Đáp án 4",
        selectedAnswer: 0
      },
    ]
  }
  remainder: Subject<number>|null = null; //sec
  remainderTime: number = 0; //sec
  Math: any = Math;
  Array: any = Array;
  result: TestResultDTO|null = null;
  constructor(
    private activatedRoute: ActivatedRoute,
    private componentService: ComponentService
  ) {}

  ngOnInit(): void {
    this.testId = this.activatedRoute.snapshot.paramMap.get('id');
    this.type = this.activatedRoute.snapshot.paramMap.get('type') as TestType|null;
    if(!this.type || !this.testId) {
      this.componentService.displayMessage("Đã có lỗi xảy ra");
    }

    //countDown
    this.remainder = new Subject<number>();
    this.remainderTime = this.test!.duration * 60;
    this.remainder.next(this.remainderTime);
    const subscribeId = setInterval(() => {
      this.remainderTime--;
      this.remainder!.next(this.remainderTime);
    }, 1000);
    this.remainder.subscribe(next => {
      if(next == 0) {
        clearInterval(subscribeId);
        this.result = {
          total: 40,
          correct: 30,
          unselected: 2,
          score: 7.5,
          usedTime: 126,
          detail: [1, 1, 3, 2]
        };
      }
    });

    
  }

  end() {
    this.remainderTime = 0;
    this.remainder!.next(0);
  }

  scrollTo(id: number) {
    const element = document.getElementById(id.toString());
    element?.scrollIntoView({
      behavior: 'smooth',
      block: 'start'
    })
  }
}
