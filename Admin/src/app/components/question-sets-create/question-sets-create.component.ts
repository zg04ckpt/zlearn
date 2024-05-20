import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { QSCreateRequest } from 'src/app/interfaces/qsCreateRequest';
import { QuestionModule } from 'src/app/models/question.module';
import { QuestionSetsService } from 'src/app/services/question-sets.service';
import * as XLSX from 'xlsx';

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
    questions: [],
    testTime: {
      minutes: 0,
      seconds: 0
    }
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
      }, error => {
        if(error.status == 401)
        {
          alert("You must login to do this action");
          localStorage.removeItem('token');
          window.location.href = "/login";``
        }
        else
          alert("Error: " + JSON.stringify(error));
      })
    }
  }

  onSelectedChange()
  {
    this.selectedCount = this.questions.filter(x => x.selected).length;
  }

  onImageFileSelected(event: any) 
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

  onExcelFileSelected(event: any)
  {
    const target: DataTransfer = <DataTransfer>(event.target);
    if(target.files.length != 1) throw new Error("Không thể tải lên nhiều file");
    const reader: FileReader = new FileReader();
    reader.onload = (e: any) => {
      const bstr: string = e.target.result;
      const wb: XLSX.WorkBook = XLSX.read(bstr, {type: 'binary'});
      const wsname: string = wb.SheetNames[0];
      const ws: XLSX.WorkSheet = wb.Sheets[wsname];
      const range: XLSX.Range = XLSX.utils.decode_range(ws['!ref']!);
      for(let i=0; i<=range.e.r; i++)
      {
        let question: QuestionModule = new QuestionModule(i+1, "", "", " ", "", "", "", 1, false);
        question.content = ws[XLSX.utils.encode_cell({c: 0, r: i})]?.v;
        question.answerA = ws[XLSX.utils.encode_cell({c: 1, r: i})]?.v;
        question.answerB = ws[XLSX.utils.encode_cell({c: 2, r: i})]?.v;
        question.answerC = ws[XLSX.utils.encode_cell({c: 3, r: i})]?.v;
        question.answerD = ws[XLSX.utils.encode_cell({c: 4, r: i})]?.v;
        question.correctAnswer = Number(ws[XLSX.utils.encode_cell({c: 5, r: i})]?.v);
        this.questions.push(question);
      }
    };
    reader.readAsBinaryString(target.files[0]);
  }
}
