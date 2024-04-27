import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { QSCreateRequest } from 'src/app/interfaces/qsCreateRequest';
import { QuestionModule } from 'src/app/models/question.module';
import { QuestionSetsService } from 'src/app/services/question-sets.service';

@Component({
  selector: 'app-question-sets-create',
  templateUrl: './question-sets-create.component.html',
  styleUrls: ['./question-sets-create.component.css']
})
export class QuestionSetsCreateComponent {

  constructor(
    private location: Location,
    private service: QuestionSetsService
  ) {}

  selectedCount: number = 0
  modalTitle?: string
  modelMessage?: string
  modelValid: boolean = false

  title:string = "Tạo bộ câu hỏi mới";
  imageUrl: string = ""

  questions: QuestionModule[] = []
  question: QuestionModule = new QuestionModule(0, "", "", " ", "", "", "", 1, false);

  qsCreateRequest: QSCreateRequest = {
    name: "",
    description: "",
    creator: "admin",
    image: new File([""], "filename"),
    questions: []
  }

  ngOnInit(): void {
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
    if(this.qsCreateRequest.name == "")
    {
      this.modalTitle = "Lỗi"
      this.modelMessage = "Tên bộ câu hỏi không được để trống"
      return;
    }
    if(this.qsCreateRequest.description == "")
    {
      this.modalTitle = "Lỗi"
      this.modelMessage = "Mô tả bộ câu hỏi không được để trống"
      return;
    }

    if(this.imageUrl == "")
      {
        this.modalTitle = "Lỗi"
        this.modelMessage = "Ảnh minh họa không được để trống"
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
      this.qsCreateRequest.questions = this.questions.map(x => {
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
        };
      });

      this.service.create(this.qsCreateRequest)
      .subscribe(res => {
        console.log(res.message);
        alert("Lưu thành công");
        this.location.back();
      })
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
      this.qsCreateRequest.image = file;
      const reader = new FileReader();
      reader.onload = e => this.imageUrl = reader.result as string;
      reader.readAsDataURL(file);
    }
  }
}
