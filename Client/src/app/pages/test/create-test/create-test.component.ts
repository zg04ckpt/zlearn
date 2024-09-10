import { Component, DestroyRef, inject, OnDestroy, OnInit } from '@angular/core';
import { Question } from '../../../entities/test/question.entity';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CreateTestDTO } from '../../../dtos/test/create-test.dto';
import { NgClass } from '@angular/common';
import { ComponentService } from '../../../services/component.service';
import * as XLSX from 'xlsx';
import { TestService } from '../../../services/test.service';
import { UserService } from '../../../services/user.service';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CanComponentDeactivate } from '../../../guards/can-deactivate.guard';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-create-test',
  standalone: true,
  imports: [
    FormsModule,
    NgClass
  ],
  templateUrl: './create-test.component.html',
  styleUrl: './create-test.component.css'
})
export class CreateTestComponent implements CanComponentDeactivate{
  selectedCount: number = 0;
  questionManager: {
    previewImageUrl: string|null;
    selected: boolean;
  }[] = [];
  data: CreateTestDTO = {
    name: "",
    image: null,
    description: "",
    source: "",
    duration: 0,
    isPrivate: false,
    questions: []
  };
  adding: boolean = true;
  questionImagePreviewUrl: string|null = null;
  testImagePreviewUrl: string|null = null;
  isSelectedAll: boolean = false;
  isAdding: boolean = false;
  editIndex: number = 0;
  editQuestion: {
    content: string,
    image: File|null,
    answerA: string,
    answerB: string,
    answerC: string|null,
    answerD: string|null,
    correctAnswer: number
  }|null = null;
  isSuccess: boolean = false;
  destroyRef = inject(DestroyRef);

  constructor(
    private componentService: ComponentService,
    private testService: TestService,
    private userService: UserService,
    private authService: AuthService,
    private router: Router
  ) {}

  canDeactivate = () => {
    if(!this.isSuccess) {
      return confirm("Test chưa được tạo, rời khỏi sẽ mất bản nháp, xác nhận?");
    }
    return true;
  };

  backToList() {
    this.router.navigateByUrl("/tests/my-tests");
  }

  saveTest() {
    //check valid before send
    if(this.data.name.trim() == "") {
      this.componentService.displayMessage("Tên bài test trống!");
      return;
    }
    if(this.data.description.trim() == "") {
      this.componentService.displayMessage("Mô tả test trống!");
      return;
    }
    if(this.data.source.trim() == "") {
      this.componentService.displayMessage("Nguồn test trống!");
      return;
    }
    if(this.data.duration <= 0) {
      this.componentService.displayMessage("Thời gian làm test không hợp lệ!");
      return;
    }
    if(this.data.questions.length == 0) {
      this.componentService.displayMessage("Danh sách câu hỏi trống!");
      return;
    }

    debugger

    const currentUser = this.userService.getLoggedInUser();
    if(currentUser == null) {
      this.authService.showLoginRequirement();
    } else {
      this.componentService.displayConfirmMessage("Xác nhận tạo test mới?", () => {
        this.componentService.$showLoadingStatus.next(true);
        this.testService.create(this.data)
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe({
          next: res => {
            this.componentService.$showLoadingStatus.next(false);
            this.componentService.displayMessage("Tạo test thành công");
            this.isSuccess = true;
          },
  
          error: res => {
            this.componentService.$showLoadingStatus.next(false);
            this.isSuccess = false;
            this.componentService.displayAPIError(res);
          },

          complete: () => this.componentService.$showLoadingStatus.next(false)
        });
      });
    }
  }

  selectOrUnselectAll(value: boolean) {
    this.isSelectedAll = value;
    this.questionManager.forEach(x => x.selected = value);
  }

  removeSelected() {
    while(this.selectedCount > 0) {
      let index = this.questionManager.findIndex(x => x.selected);
      this.questionManager.splice(index, 1);
      this.data.questions.splice(index, 1);
      this.selectedCount--;
    }
  }

  select(i: number) {
    if(!this.questionManager[i].selected) {
      this.selectedCount++;
    } else {
      this.selectedCount--;
    }
    this.questionManager[i].selected = !this.questionManager[i].selected;
  }

  uploadQuestionImage(event: any) {
    const file:File = event.target.files[0];
    if(file != null) {
      this.editQuestion!.image = file;
      const reader = new FileReader();
      reader.onload = e => this.questionImagePreviewUrl = reader.result as string;
      reader.readAsDataURL(file);
    }
  }

  getImageUrl(index: number) {
    const image = this.data.questions[index].image;
    if(image != null) {
      const reader = new FileReader();
      reader.onload = () => 
        this.questionManager[index].previewImageUrl = reader.result as string;
      reader.readAsDataURL(image);
    } else {
      this.questionManager[index].previewImageUrl = null;
    }
  }

  addAnswerInEditQuestion() {
    if(this.editQuestion!.answerC == null) {
      this.editQuestion!.answerC = "";
      return;
    } 

    if(this.editQuestion!.answerD == null) {
      this.editQuestion!.answerD = "";
      return;
    } 
  }
  
  checkValid(): boolean {
    if(this.editQuestion!.content.trim() == "") {
      this.componentService.displayMessage("Nội dung câu hỏi trống!");
      return false;
    }

    if(this.editQuestion!.answerA.trim() == "" 
    || this.editQuestion!.answerB.trim() == "" 
    || this.editQuestion!.answerC?.trim() == "" 
    || this.editQuestion!.answerD?.trim() == "" ) {
      this.componentService.displayMessage("Nội dung câu trả lời trống!");
      return false;
    }

    if(this.editQuestion!.correctAnswer == 0) {
      this.componentService.displayMessage("Chưa chọn đáp án đúng!");
      return false;
    }

    return true;
  }

  addQuestionToData() {
    if(!this.checkValid()) return;

    this.componentService.displayConfirmMessage("Xác nhận thêm câu hỏi này?", () => {
      this.data.questions.push(this.editQuestion!);
      this.questionManager.push({ previewImageUrl: null, selected: false});
      this.getImageUrl(this.data.questions.length-1);
      this.editQuestion = null;
    });
  }

  updateQuestionToData() {
    if(!this.checkValid()) return;

    this.componentService.displayConfirmMessage("Xác nhận cập nhật câu hỏi này?", () => {
      this.data.questions[this.editIndex] = this.editQuestion!;
      this.getImageUrl(this.editIndex);
      this.editQuestion = null;
    });
  }

  cancelSelectAll() {
    this.selectedCount = 0;
    this.questionManager.forEach(x => x.selected = false);
  }

  uploadTestImage(event: any) {
    const file:File = event.target.files[0];
    if(file != null) {
      this.data.image = file;
      const reader = new FileReader();
      reader.onload = e => this.testImagePreviewUrl = reader.result as string;
      reader.readAsDataURL(file);
    }
  }

  showAddQuestionModal() {
    this.isAdding = true;
    this.editQuestion = {
      content: "",
      image: null,
      answerA: "",
      answerB: "",
      answerC: null,
      answerD: null,
      correctAnswer: 0
    };
    this.questionImagePreviewUrl = null;
  }

  showEditQuestionModal(i: number) {
    this.editIndex = i;
    this.isAdding = false;
    this.editQuestion = {... this.data.questions[i]};
    this.questionImagePreviewUrl = this.questionManager[i].previewImageUrl;
  }

  scrollToTop() {
    const element = document.getElementById("test-create-form");
    console.log(element);
    if(element != null) {
        element.scrollTo({ top: 0, behavior: 'smooth' });
    }
  }

  convertExcelFileToData(event: Event) {
    const target = event.target as HTMLInputElement;
    if(target.files?.length !== 1) {
      this.componentService.displayMessage("Không thể tải lên nhiều file");
    }
    const reader = new FileReader();
    reader.onload = e => {
      const bstr = e.target?.result as string;
      const wb: XLSX.WorkBook = XLSX.read(bstr, {type: 'binary' });
      const ws: XLSX.WorkSheet = wb.Sheets[wb.SheetNames[0]];
      if(ws != null) {
        const range: XLSX.Range = XLSX.utils.decode_range(ws['!ref']!);
        for(let i = 1; i <= range.e.r; i++) {

          //get image from cell
          const imageCell = ws[XLSX.utils.encode_cell({ c: 6, r: i })];
          let imageFile: File | null = null;
          if (imageCell && imageCell.v) {
              const imageData = imageCell.v; // Assuming image data is stored in the cell
              const byteCharacters = atob(imageData);
              const byteNumbers = new Array(byteCharacters.length);
              for (let j = 0; j < byteCharacters.length; j++) {
                  byteNumbers[j] = byteCharacters.charCodeAt(j);
              }
              const byteArray = new Uint8Array(byteNumbers);
              imageFile = new File([byteArray], `image_${i}.png`, { type: 'image/png' });
          }
          
          this.data.questions.push({
            content: ws[XLSX.utils.encode_cell({c: 0, r: i})]?.v,
            answerA: ws[XLSX.utils.encode_cell({c: 1, r: i})]?.v,
            answerB: ws[XLSX.utils.encode_cell({c: 2, r: i})]?.v,
            answerC: ws[XLSX.utils.encode_cell({c: 3, r: i})]?.v,
            answerD: ws[XLSX.utils.encode_cell({c: 4, r: i})]?.v,
            correctAnswer: ws[XLSX.utils.encode_cell({c: 5, r: i})]?.v,
            image: imageFile
          });

          this.questionManager.push({ previewImageUrl: null, selected: false});
          this.getImageUrl(this.data.questions.length-1);
        }
        debugger
      }
    };

    //check if remain old question
    if(this.data.questions.length != 0) {
      this.componentService.displayYesNoConfirmMessage(
        "Xác nhận bỏ tất những câu hỏi đã thêm?", 
        () => {
          this.data.questions = [];
          reader.readAsBinaryString(target.files![0]);
        },
        () => reader.readAsBinaryString(target.files![0])
      );
    } else {
      reader.readAsBinaryString(target.files![0]);
    }
  }
}
