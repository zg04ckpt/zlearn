import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { QSUpdateRequest } from 'src/app/interfaces/qsUpdateRequest';
import { QuestionSetsModule } from 'src/app/models/question-sets.module';
import { QuestionModule } from 'src/app/models/question.module';
import { QuestionSetsService } from 'src/app/services/question-sets.service';
import { environment } from 'src/environments/environment';
import { Location } from '@angular/common';

@Component({
  selector: 'app-question-sets-update',
  templateUrl: './question-sets-update.component.html',
  styleUrls: ['./question-sets-update.component.css']
})
export class QuestionSetsUpdateComponent {

  id: string = ""
  title: string = "Cập nhật bộ câu hỏi"

  selectedCount: number = 0
  modalTitle?: string
  modelMessage?: string
  modelValid: boolean = false
  image: File | null = null

  questions: QuestionModule[] = []
  question: QuestionModule = new QuestionModule(0, "", "", " ", "", "", "", 1, false);
  questionSet: QuestionSetsModule = {
    id: "",
    name: "",
    description: "",
    imageUrl: "",
    creator: "",
    createdDate: new Date(),
    updatedDate: new Date(),
    questionCount: 0,
    testTime: {
      minutes: 0,
      seconds: 0
    }
  }

  constructor(
    private location: Location,
    private service: QuestionSetsService,
    private route: ActivatedRoute
  ) {}


  ngOnInit(): void {
    this.id = this.route.snapshot.params['id'];
    this.service.getById(this.id)
    .subscribe(res => {
      this.questionSet = this.service.convertToQuestionSet(res);
      this.questionSet.imageUrl = environment.baseUrl + this.questionSet.imageUrl;
    })

    this.service.getAllQuestions(this.id)
    .subscribe(res => {
      this.questions = this.service.convertToQuestions(res);
    })
  }

  addNewQuestion()
  {
    this.questions.push(this.question);
    this.question = new QuestionModule(0, "", "", " ", "", "", "", 1, false);
  }

  openAddModal()
  {
    this.question = new QuestionModule(0, "", "", " ", "", "", "", 1, false);
  }

  getEditQuestion()
  {
    this.question = this.questions.find(x => x.selected)!;
  }

  questionValid() : boolean
  {
    if(this.question.content == "") return false;
    if(this.question.answerA == "") return false;
    if(this.question.answerB == "") return false;
    if(this.question.answerC == "") return false;
    if(this.question.answerD == "") return false;
    return true;
  }

  removeSelected()
  {
    this.questions = this.questions.filter(x => !x.selected);
    this.selectedCount = 0;
  }

  cancelSelectAll()
  {
    this.questions.forEach(x => x.selected = false);
    this.selectedCount = 0;
  }

  checkValidAndConfirm()
  {
    this.modelValid = false;
    if(this.questionSet.name == "")
    {
      this.modalTitle = "Lỗi"
      this.modelMessage = "Tên bộ câu hỏi không được để trống"
      return;
    }
    if(this.questionSet.description == "")
    {
      this.modalTitle = "Lỗi"
      this.modelMessage = "Mô tả bộ câu hỏi không được để trống"
      return;
    }

    if(this.questions.length == 0)
    {
      this.modalTitle = "Lỗi"
      this.modelMessage = "Bộ câu hỏi phải có ít nhất 1 câu hỏi"
      return;
    }

    this.modelValid = true;
    this.modalTitle = "Thành công"
    this.modelMessage = "Xác nhận lưu bộ câu hỏi ?"
  }

  saveQuestionSets(): void {
    if(this.modelValid)
    {
      let request: QSUpdateRequest = {
        name: this.questionSet.name,
        description: this.questionSet.description,
        image: this.image,
        questions: this.questions.map(x => {
          return {
            order: x.order,
            content: x.content,
            image: null,
            answerA: x.answerA,
            answerB: x.answerB,
            answerC: x.answerC,
            answerD: x.answerD,
            correctAnswer: x.correctAnswer,
            mark: false
          }
        }),
        mark: false,
        testTime: this.questionSet.testTime
      }
      this.service.update(this.id, request)
      .subscribe(
        res => {
          alert("Cập nhật bộ câu hỏi thành công")
          this.location.back();
        },
        error => {
          if(error.status == 401)
          {
            alert("You must login to do this action");
            localStorage.removeItem('token');
            window.location.href = "/login";``
          }
          else
            alert("Error: " + JSON.stringify(error));
        }
      )
    }
  }

  onSelectedChange()
  {
    this.selectedCount = this.questions.filter(x => x.selected).length;
  }

  onFileSelected(event: any) 
  {
    const file: File = event.target.files[0];
    if(file) 
    {
      this.image = file;
      const reader = new FileReader();
      reader.onload = e => this.questionSet.imageUrl = reader.result as string;
      reader.readAsDataURL(file);
    }
  }
}
