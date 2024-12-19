import { Component, OnInit } from '@angular/core';
import { CreateCommentDTO } from '../../../dtos/comment/create-comment.dto';
import { CreateDocumentDTO } from '../../../dtos/document/create-document.dto';
import { FormsModule } from '@angular/forms';
import { CategoryItem } from '../../../entities/management/category-item.entity';
import { NgClass } from '@angular/common';
import { DocumentService } from '../../../services/document.service';
import { BankInfo } from '../../../dtos/document/bank-info.dto';
import { ComponentService } from '../../../services/component.service';
import { environment } from '../../../../environments/environment';
import { Title } from '@angular/platform-browser';
import { BreadcrumbService } from '../../../services/breadcrumb.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-document',
  standalone: true,
  imports: [
    FormsModule,
    NgClass
  ],
  templateUrl: './create-document.component.html',
  styleUrl: './create-document.component.css'
})
export class CreateDocumentComponent implements OnInit {
  data: CreateDocumentDTO = {
    name: "",
    categoryId: "",
    description: "",
    image: null,
    sourceFile: null,
    previewImages: [],
    paymentInfo: null
  }
  categories: CategoryItem[] = [];
  isShowPaymentInfo = false;
  banks: BankInfo[] = [];

  defaultImageUrl = environment.defaultImageUrl;
  imageUrls:{
    imageUrl: string,
    previewImageUrls: string[]
  } = {
    imageUrl: '',
    previewImageUrls: []
  }

  constructor(
    private documentService: DocumentService,
    private componentService: ComponentService,
    private titleService: Title,
    private breadcrumbService: BreadcrumbService,
    private router: Router,
  ) {}

  async ngOnInit(): Promise<void> {
    //get bank
    this.banks = await this.documentService.getBankList();
    this.componentService.$showLoadingStatus.next(false);

    //get categories
    this.documentService.getCategories().subscribe(next => {
      this.componentService.$showLoadingStatus.next(false);
      this.categories = next;
    });

    //set bread and title
    this.titleService.setTitle("Tạo tài liệu mới");
    this.breadcrumbService.addBreadcrumb("Tạo tài liệu mới", this.router.url);
  }

  uploadFile(event: Event) {
    const target = event.target as HTMLInputElement;
    if(target.files!.length > 1) {
      this.componentService.displayError("Chỉ được chọn 1 file", []);
      return;
    }
    if(target.files![0].size > 5 * 1024 * 1024) {
      this.componentService.displayError(`Kích thước tài liệu không được vượt quá 5MB`, []);
      target.value = '';
      return;
    }
    const reader = new FileReader();
    reader.onload = e => {
      this.data.sourceFile = target.files![0];
    };
    reader.readAsArrayBuffer(target.files![0]);
  }

  setImage(event: Event) {
    const target = event.target as HTMLInputElement;
    if(target.files!.length > 1) {
      this.componentService.displayError("Chỉ được chọn 1 ảnh", []);
      return;
    }

    const reader = new FileReader();
    reader.onload = e => {
      this.imageUrls.imageUrl = reader.result as string;
      this.data.image = target.files![0];
    };
    reader.readAsDataURL(target.files![0]);
  }

  removeImageAt(index: number) {
    console.log(index);
    this.data.previewImages.splice(index, 1);
    this.imageUrls.previewImageUrls.splice(index, 1);
}

  addPreviewImage(event: Event) {
    const target = event.target as HTMLInputElement;
    
    for(let i = 0; i < Math.min(target.files!.length, 4); i++) {
      if(this.data.previewImages.length < 4) {
        const reader = new FileReader();
        reader.onload = e => {
          this.imageUrls.previewImageUrls.push(reader.result as string);
          this.data.previewImages.push(target.files![i]);
        };
        reader.readAsDataURL(target.files![i]);
      } else {
        break;
      }
    }
  }

  save() {
    console.log(this.data);
    if(!this.data.name) {
      this.componentService.displayError("Tên tài liệu không hợp lệ!", []);
      return;
    }
    if(!this.data.description) {
      this.componentService.displayError("Mô tả tài liệu không hợp lệ!", []);
      return;
    }
    if(!this.data.categoryId) {
      this.componentService.displayError("Chưa chọn danh mục cho tài liệu!", []);
      return;
    }
    if(!this.data.sourceFile) {
      this.componentService.displayError("File tài liệu trống!", []);
      return;
    }
    if(this.data.previewImages.length == 0) {
      this.componentService.displayError("Thêm ít nhất 1 ảnh xem trước!", []);
      return;
    }

    this.documentService.create(this.data).subscribe(res => {
      this.componentService.$showLoadingStatus.next(false);
      this.componentService.displayMessage("Tạo tài liệu thành công!");
      window.history.back();
    });
  }
}
