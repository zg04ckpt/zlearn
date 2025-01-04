import { Component, OnInit } from '@angular/core';
import { CreateDocumentDTO } from '../../../dtos/document/create-document.dto';
import { CategoryItem } from '../../../entities/common/category-item.entity';
import { BankInfo } from '../../../dtos/document/bank-info.dto';
import { environment } from '../../../../environments/environment';
import { DocumentService } from '../../../services/document.service';
import { ComponentService } from '../../../services/component.service';
import { Title } from '@angular/platform-browser';
import { BreadcrumbService } from '../../../services/breadcrumb.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UpdateDocumentDTO } from '../../../dtos/document/update-document.dto';

@Component({
  selector: 'app-update-document',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './update-document.component.html',
  styleUrl: './update-document.component.css'
})
export class UpdateDocumentComponent implements OnInit {
  id: string|null = null;
  data: UpdateDocumentDTO = {
    name: "",
    categoryId: "",
    description: "",
    fileName: "",
    imageUrl: null,
    newImage: null,
    newSourceFile: null,
    previewImages: [],
    paymentInfo: null,
  }
  categories: CategoryItem[] = [];
  isShowPaymentInfo = false;
  banks: BankInfo[] = [];

  defaultImageUrl = environment.defaultImageUrl;

  constructor(
    private documentService: DocumentService,
    private componentService: ComponentService,
    private titleService: Title,
    private breadcrumbService: BreadcrumbService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {}

  async ngOnInit(): Promise<void> {
    //get id from path
    this.activatedRoute.params.subscribe(params => {
      if(params['id']) {
        this.id = params['id'];
        this.documentService.getUpdateContent(this.id!).subscribe(res => {
          this.componentService.$showLoadingStatus.next(false);
          this.data = res;
        });
      } else {
        this.componentService.$showToast.next('ID tài liệu trống');
      }
    });

    //get bank
    this.banks = await this.documentService.getBankList();
    this.componentService.$showLoadingStatus.next(false);

    //get categories
    this.documentService.getCategories().subscribe(next => {
      this.componentService.$showLoadingStatus.next(false);
      this.categories = next;
    });

    //set bread and title
    this.titleService.setTitle("Cập nhật tài liệu");
    this.breadcrumbService.getBreadcrumb('tai-lieu');
  }

  uploadFile(event: Event) {
    const target = event.target as HTMLInputElement;
    if(target.files!.length > 1) {
      this.componentService.displayError("Chỉ được chọn 1 file", []);
      return;
    }
    const reader = new FileReader();
    reader.onload = e => {
      this.data.newSourceFile = target.files![0];
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
      this.data.imageUrl = reader.result as string;
      this.data.newImage = target.files![0];
    };
    reader.readAsDataURL(target.files![0]);
  }

  removeImageAt(index: number) {
    console.log(index);
    this.data.previewImages.splice(index, 1);
  }

  addPreviewImage(event: Event) {
    const target = event.target as HTMLInputElement;
    
    for(let i = 0; i < Math.min(target.files!.length, 4); i++) {
      if(this.data.previewImages.length < 4) {
        const reader = new FileReader();
        reader.onload = e => {
          this.data.previewImages.push({
            id: null,
            imageUrl: reader.result as string,
            image: target.files![0]
          });
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
    if(this.data.previewImages.length == 0) {
      this.componentService.displayError("Thêm ít nhất 1 ảnh xem trước!", []);
      return;
    }

    this.documentService.updateDocument(this.id!, this.data).subscribe(res => {
      this.componentService.$showLoadingStatus.next(false);
      this.componentService.$showToast.next("Cập tài liệu thành công!");
      window.history.back();
    });
  }
}
