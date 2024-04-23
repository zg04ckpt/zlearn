import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { QuestionSetsModule } from 'src/app/models/question-sets.module';
import { QuestionModule } from 'src/app/models/question.module';

@Component({
  selector: 'app-question-sets-create',
  templateUrl: './question-sets-create.component.html',
  styleUrls: ['./question-sets-create.component.css']
})
export class QuestionSetsCreateComponent {

  constructor(
    private route: ActivatedRoute,
    private location: Location
  ) {}

  selectedCount: number = 0
  modalTitle?: string
  modelMessage?: string
  modelValid: boolean = false

  question: QuestionModule = new QuestionModule("", "", false, "", "", "", "", 1);
  

  questionSets:QuestionSetsModule = new QuestionSetsModule(
    "1",
    "699 câu hỏi trắc nghiệm 1",
    "Đây là bộ câu hỏi trắc nghiệm 1",
    "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b2/Bootstrap_logo.svg/800px-Bootstrap_logo.svg.png",
    "admin",
    new Date("1-1-2003"),
    new Date("1-1-2003"),
    [
      new QuestionModule("1-1", "Câu hỏi thứ 1", false, "Đáp án a", "Đáp án b", "Đáp án c", "Đáp án d", 1),
      new QuestionModule("1-2", "Câu hỏi thứ 2", false, "Đáp án a", "Đáp án b", "Đáp án c", "Đáp án d", 2),
      new QuestionModule("1-3", "Câu hỏi thứ 3", false, "Đáp án a", "Đáp án b", "Đáp án c", "Đáp án d", 3),
      new QuestionModule("1-4", "Câu hỏi thứ 4", false, "Đáp án a", "Đáp án b", "Đáp án c", "Đáp án d", 4)
    ]
  )

  addNewQuestion()
  {
    if(this.question.content == "")
    {
      alert("Câu hỏi không được để trống")
      return;
    }

    if(this.question.a == "")
    {
      alert("Đáp án A không được để trống")
      return;
    }

    if(this.question.b == "")
    {
      alert("Đáp án B không được để trống")
      return;
    }

    if(this.question.c == "")
    {
      alert("Đáp án C không được để trống")
      return;
    }

    if(this.question.d == "")
    {
      alert("Đáp án D không được để trống")
      return;
    }

    //thêm id mặc định
    this.question.id = this.questionSets.id + "-" + this.questionSets.questions.length + 1 + "";
    
    this.questionSets.questions.push(this.question);
    alert("Thêm câu hỏi thành công");
    this.question = new QuestionModule("", "", false, "", "", "", "", 0);
  }

  getEditQuestion()
  {
    this.question = this.questionSets.questions.find(x => x.selected)!;
  }

  updateQuestionValid() : boolean
  {
    if(this.question.content == "") return false;
    if(this.question.a == "") return false;
    if(this.question.b == "") return false;
    if(this.question.c == "") return false;
    if(this.question.d == "") return false;
    return true;
  }

  removeSelected()
  {
    this.questionSets.questions = this.questionSets.questions.filter(x => !x.selected);
    this.selectedCount = 0;
  }

  cancelSelectAll()
  {
    this.questionSets.questions.forEach(x => x.selected = false);
    this.selectedCount = 0;
  }

  checkValidAndConfirm()
  {
    this.modelValid = false;
    if(this.questionSets.name == "")
    {
      this.modalTitle = "Lỗi"
      this.modelMessage = "Tên bộ câu hỏi không được để trống"
      return;
    }
    if(this.questionSets.desc == "")
    {
      this.modalTitle = "Lỗi"
      this.modelMessage = "Mô tả bộ câu hỏi không được để trống"
      return;
    }
    if(this.questionSets.questions.length == 0)
    {
      this.modalTitle = "Lỗi"
      this.modelMessage = "Bộ câu hỏi phải có ít nhất 1 câu hỏi"
      return;
    }

    this.modelValid = true;
    this.modalTitle = "Thành công"
    this.modelMessage = "Xác nhận lưu bộ câu hỏi ?"
  }

  saveQuestionSets() {
    if(this.modelValid)
    {
      alert("Lưu thành công")
      this.location.back();
    }
  }

  onSelectedChange()
  {
    this.selectedCount = this.questionSets.questions.filter(x => x.selected).length;
  }

  onFileSelected(event: any) 
  {
    const file: File = event.target.files[0];
    if(file) 
    {
      const reader = new FileReader();
      reader.onload = (e: any) => {
          this.questionSets.imageUrl = e.target.result;
      };
      reader.readAsDataURL(file);
    }
  }
}
