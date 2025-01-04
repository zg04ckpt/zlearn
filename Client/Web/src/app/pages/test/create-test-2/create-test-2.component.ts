import { Component, OnInit } from '@angular/core';
import { CreateTestDTO } from '../../../dtos/test/create-test.dto';
import { FormsModule } from '@angular/forms';
import { environment } from '../../../../environments/environment';
import { CategoryItem } from '../../../entities/common/category-item.entity';
import { ComponentService } from '../../../services/component.service';
import { TestService } from '../../../services/test.service';
import { UserService } from '../../../services/user.service';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { BreadcrumbService } from '../../../services/breadcrumb.service';
import * as XLSX from 'xlsx';
import { NgClass } from '@angular/common';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-create-test-2',
  standalone: true,
  imports: [
    FormsModule,
    NgClass
  ],
  templateUrl: './create-test-2.component.html',
  styleUrl: './create-test-2.component.css'
})
export class CreateTest2Component implements OnInit {
  constructor(
    private componentService: ComponentService,
    private testService: TestService,
    private userService: UserService,
    private authService: AuthService,
    private router: Router,
    private titleService: Title,
    private breadcrumbService: BreadcrumbService
  ) {}

  // Test
  data: CreateTestDTO = {
    name: "",
    image: null,
    description: "",
    source: "",
    duration: 0,
    categorySlug: "",
    isPrivate: false,
    questions: []
  };
  testImageUrl = environment.defaultImageUrl;
  categories: CategoryItem[] = [];
  success = false; // true if this test created successfully

  // Questions
  selectedCount = 0;

  questionManager: {
    imageUrl: string|null;
    selected: boolean;
  }[] = [];

  addQuestionModule:{
    isShow: boolean,
    content: string,
    image: File|null,
    imageUrl: string|null,
    answerA: string,
    answerB: string,
    answerC: string|null,
    answerD: string|null,
    correctAnswer: number
  } = {
    isShow: false,
    content: "",
    image: null,
    imageUrl: null,
    answerA: "",
    answerB: "",
    answerC: null,
    answerD: null,
    correctAnswer: 0
  }

  updateQuestionModule:{
    index: number,
    isShow: boolean,
    content: string,
    image: File|null,
    imageUrl: string|null,
    answerA: string,
    answerB: string,
    answerC: string|null,
    answerD: string|null,
    correctAnswer: number
  } = {
    index: -1,
    isShow: false,
    content: "",
    image: null,
    imageUrl: null,
    answerA: "",
    answerB: "",
    answerC: null,
    answerD: null,
    correctAnswer: 0
  }

  uploadQuestionModule = {
    isShow: false,
    quesIndex: 1,
    aIndex: 2,
    bIndex: 3,
    cIndex: 4,
    dIndex: 5,
    ansIndex: 6,
    startRow: 2,
    rowsCount: 50,
    instructionImageUrl: environment.baseUrl + '/api/images/system/hd1.jpg'
  }

  public ngOnInit(): void {
    this.testService.getCategories().subscribe(next => {
      this.componentService.$showLoadingStatus.next(false);
      this.categories = next;
    });
  }

  // TEST ------------------------------------------------------------------
  public save() {
    // Check valid test
    if(!this.data.name) {
      this.componentService.displayError("Tên đề không hợp lệ", []);
      return;
    }
    if(!this.data.categorySlug) {
      this.componentService.displayError("Chưa chọn danh mục", []);
      return;
    }
    if(!this.data.description) {
      this.componentService.displayError("Mô tả không hợp lệ", []);
      return;
    }
    if(!this.data.source) {
      this.componentService.displayError("Nguồn không hợp lệ", []);
      return;
    }
    if(this.data.duration < 1) {
      // Check valid test
    }

    // Check valid question
    if(this.data.questions.length == 0) {
      this.componentService.displayError("Bộ câu hỏi trống", []);
      return;
    }
    for(let i = 0; i < this.data.questions.length; i++) {
      const q = this.data.questions[i];
      if(!q.content) {
        this.componentService.displayError(`Nội dung câu hỏi thứ ${i+1} không hợp lệ`, []);
        return;
      }
      if(!q.answerA) {
        this.componentService.displayError(`Nội dung đáp án A câu hỏi thứ ${i+1} không hợp lệ`, []);
        return;
      }
      if(!q.answerB) {
        this.componentService.displayError(`Nội dung đáp án B câu hỏi thứ ${i+1} không hợp lệ`, []);
        return;
      }
      if(q.answerC != null && !q.answerC.trim()) {
        this.componentService.displayError(`Nội dung đáp án C câu hỏi thứ ${i+1} không hợp lệ`, []);
        return;
      }
      if(q.answerD != null && !q.answerD.trim()) {
        this.componentService.displayError(`Nội dung đáp án D câu hỏi thứ ${i+1} không hợp lệ`, []);
        return;
      }
      if(q.correctAnswer == 0) {
        this.componentService.displayError(`Chưa chọn đáp án đúng cho câu hỏi thứ ${i+1}`, []);
        return;
      }

      // Check duplicate
      let ans = [q.answerA.trim(), q.answerB.trim()]
      if(q.answerC) {
        ans.push(q.answerC.trim());
      }
      if(q.answerD) {
        ans.push(q.answerD.trim());
      }
      if(new Set(ans).size != ans.length) {
        this.componentService.displayError(`Câu hỏi thứ ${i+1} chứa đáp án trùng`, []);
        return;
      }
    }

    this.componentService.displayConfirmMessage("Xác nhận tạo đề này?", () => {
      this.testService.create2(this.data).subscribe(next => {
        this.componentService.$showLoadingStatus.next(false);
        this.componentService.$showToast.next("Tạo thành công");
        this.success = true;
        window.history.back();
      })
    });
  }

  public back() {
    window.history.back();
  }

  public uploadTestImage(event: Event) {
    const target = event.target as HTMLInputElement;
    if(target.files?.length != 1) {
      this.componentService.displayError("Chỉ được tải 1 ảnh", []);
      return;
    }
    
    const reader = new FileReader();
    reader.onload = e => {
      this.data.image = target.files![0];
      this.testImageUrl = reader.result as string;
      target.value = '';
    }
    reader.readAsDataURL(target.files![0])
  }
  
  // UPLOAD QUESTIONS -----------------------------------------------------
  public checkUploadConfig(event: Event) {
    if(
      this.uploadQuestionModule.quesIndex == 0 ||
      this.uploadQuestionModule.aIndex == 0 || 
      this.uploadQuestionModule.bIndex == 0 ||
      this.uploadQuestionModule.ansIndex == 0 ||
      this.uploadQuestionModule.startRow == 0 ||
      this.uploadQuestionModule.rowsCount == 0
    ) {
      this.componentService.displayError("Vui lòng điền đủ cấu hình (trừ đáp án C, và D có thể bỏ)", []);
      event.preventDefault();
    }
  }

  public convertExcelFileToData(event: Event) {

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
        debugger
        let i = this.uploadQuestionModule.startRow-1;
        for(let k = 0; k < this.uploadQuestionModule.rowsCount; k++) {

          this.data.questions.push({
            content: ws[XLSX.utils.encode_cell({c: this.uploadQuestionModule.quesIndex-1, r: i+k})]?.v || "",
            answerA: ws[XLSX.utils.encode_cell({c: this.uploadQuestionModule.aIndex-1, r: i+k})]?.v || "",
            answerB: ws[XLSX.utils.encode_cell({c: this.uploadQuestionModule.bIndex-1, r: i+k})]?.v || "",
            answerC: ws[XLSX.utils.encode_cell({c: this.uploadQuestionModule.cIndex-1, r: i+k})]?.v || null,
            answerD: ws[XLSX.utils.encode_cell({c: this.uploadQuestionModule.dIndex-1, r: i+k})]?.v || null,
            correctAnswer: ws[XLSX.utils.encode_cell({c: this.uploadQuestionModule.ansIndex-1, r: i+k})]?.v || 0,
            image: null
          });

          this.questionManager.push({ imageUrl: null, selected: false});
        }
        this.uploadQuestionModule.isShow = false;
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

  // ADD QUESTIONS ----------------------------------------------
  public addNewQuestionToData() {
    // Check valid
    if(!this.addQuestionModule.content) {
      this.componentService.displayError("Nội dung câu hỏi không hợp lệ", []);
      return;
    }
    if(!this.addQuestionModule.answerA) {
      this.componentService.displayError("Nội dung đáp án A không hợp lệ", []);
      return;
    }
    if(!this.addQuestionModule.answerB) {
      this.componentService.displayError("Nội dung đáp án B không hợp lệ", []);
      return;
    }
    if(this.addQuestionModule.answerC != null && !this.addQuestionModule.answerC.trim()) {
      this.componentService.displayError("Nội dung đáp án C không hợp lệ", []);
      return;
    }
    if(this.addQuestionModule.answerD != null && !this.addQuestionModule.answerD.trim()) {
      this.componentService.displayError("Nội dung đáp án D không hợp lệ", []);
      return;
    }
    if(this.addQuestionModule.correctAnswer == 0) {
      this.componentService.displayError("Chưa chọn đáp án đúng", []);
      return;
    }

    // Display confirm
    this.componentService.displayConfirmMessage("Xác nhận thêm câu hỏi này?", () => {
      this.data.questions.push({
        content: this.addQuestionModule.content,
        image: this.addQuestionModule.image,
        answerA: this.addQuestionModule.answerA,
        answerB: this.addQuestionModule.answerB,
        answerC: this.addQuestionModule.answerC,
        answerD: this.addQuestionModule.answerD,
        correctAnswer: this.addQuestionModule.correctAnswer
      });
      this.questionManager.push({
        imageUrl: this.addQuestionModule.imageUrl,
        selected: false
      });
      this.addQuestionModule.isShow = false;
    });
  }

  // UPDATE QUESTIONS --------------------------------------------
  public updateQuestionToData() {
    // Check valid
    if(!this.updateQuestionModule.content) {
      this.componentService.displayError("Nội dung câu hỏi không hợp lệ", []);
      return;
    }
    if(!this.updateQuestionModule.answerA) {
      this.componentService.displayError("Nội dung đáp án A không hợp lệ", []);
      return;
    }
    if(!this.updateQuestionModule.answerB) {
      this.componentService.displayError("Nội dung đáp án B không hợp lệ", []);
      return;
    }
    if(this.updateQuestionModule.answerC != null && !this.updateQuestionModule.answerC.trim()) {
      this.componentService.displayError("Nội dung đáp án C không hợp lệ", []);
      return;
    }
    if(this.updateQuestionModule.answerD != null && !this.updateQuestionModule.answerD.trim()) {
      this.componentService.displayError("Nội dung đáp án D không hợp lệ", []);
      return;
    }
    if(this.updateQuestionModule.correctAnswer == 0) {
      this.componentService.displayError("Chưa chọn đáp án đúng", []);
      return;
    }

    this.componentService.displayConfirmMessage("Xác nhận cập nhật câu này?", () => {
      this.data.questions[this.updateQuestionModule.index] = {
        content: this.updateQuestionModule.content,
        image: this.updateQuestionModule.image,
        answerA: this.updateQuestionModule.answerA,
        answerB: this.updateQuestionModule.answerB,
        answerC: this.updateQuestionModule.answerC,
        answerD: this.updateQuestionModule.answerD,
        correctAnswer: this.updateQuestionModule.correctAnswer
      };
      this.questionManager[this.updateQuestionModule.index].imageUrl = this.updateQuestionModule.imageUrl;
      this.updateQuestionModule.isShow = false;
    });
  }

  public updateQuestion(index: number) {
    debugger
    this.updateQuestionModule.index = index
    this.updateQuestionModule.content = this.data.questions[index].content; 
    this.updateQuestionModule.answerA = this.data.questions[index].answerA; 
    this.updateQuestionModule.answerB = this.data.questions[index].answerB; 
    this.updateQuestionModule.answerC = this.data.questions[index].answerC; 
    this.updateQuestionModule.answerD = this.data.questions[index].answerD; 
    this.updateQuestionModule.correctAnswer = this.data.questions[index].correctAnswer; 
    this.updateQuestionModule.image = this.data.questions[index].image; 
    this.updateQuestionModule.imageUrl = this.questionManager[index].imageUrl;
    this.updateQuestionModule.isShow = true;
    console.log(this.updateQuestionModule);
    
  }

  // OTHER ---------------------------------------------------------
  public uploadQuestionImage(event: Event, isAdding: boolean) {
    debugger
    if(isAdding) {
      const target = event.target as HTMLInputElement;
      if(target.files?.length != 1) {
        this.componentService.displayError("Chỉ được tải 1 ảnh", []);
        return;
      }
      const reader = new FileReader();
      reader.onload = e => {
        this.addQuestionModule.image = target.files![0];
        this.addQuestionModule.imageUrl = reader.result as string;
        target.value = '';
      }
      reader.readAsDataURL(target.files![0]);
    } else {
      const target = event.target as HTMLInputElement;
      if(target.files?.length != 1) {
        this.componentService.displayError("Chỉ được tải 1 ảnh", []);
        return;
      }
      const reader = new FileReader();
      reader.onload = e => {
        this.updateQuestionModule.image = target.files![0];
        this.updateQuestionModule.imageUrl = reader.result as string;
        target.value = '';
      }
      reader.readAsDataURL(target.files![0]);
    }
  }

  public unselectAll() {
    this.selectedCount = 0;
    this.questionManager.forEach(x => x.selected = false);
  }

  public selectAll() {
    this.selectedCount = this.data.questions.length;
    this.questionManager.forEach(x => x.selected = true);
  }

  public removeSelectedQuestions() {
    this.componentService.displayConfirmMessage("Xác nhận xóa hết câu hỏi đã chọn?", () => {
      while(this.selectedCount > 0) {
        let index = this.questionManager.findIndex(x => x.selected);
        this.questionManager.splice(index, 1);
        this.data.questions.splice(index, 1);
        this.selectedCount--;
      }
    });
  }

  public canDeactivate(): Observable<boolean> | Promise<boolean> | boolean {
    if(!this.success) {
      return confirm("Bạn chưa tạo đề thành công, rời khỏi sẽ mất bản nháp?");
    }
    return true;
  }
}
