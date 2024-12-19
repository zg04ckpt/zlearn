import { Component, OnInit } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { DocumentSearchingDTO } from '../../../dtos/document/document-searching.dto';
import { CategoryItem } from '../../../entities/management/category-item.entity';
import { DocumentItemDTO } from '../../../dtos/document/document-item';
import { DocumentService } from '../../../services/document.service';
import { ComponentService } from '../../../services/component.service';
import { UserService } from '../../../services/user.service';
import { Title } from '@angular/platform-browser';
import { BreadcrumbService } from '../../../services/breadcrumb.service';
import { Router, RouterLink } from '@angular/router';
import { NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-my-document',
  standalone: true,
  imports: [
    NgClass,
    FormsModule,
    RouterLink
  ],
  templateUrl: './my-document.component.html',
  styleUrl: './my-document.component.css'
})
export class MyDocumentComponent implements OnInit {
  defaultImageUrl = environment.defaultImageUrl;
  data: DocumentItemDTO[] = [];

  constructor(
    private documentService: DocumentService,
    private componentService: ComponentService,
    private userService: UserService,
    private titleService: Title,
    private breadcrumbService: BreadcrumbService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    //set bread and title
    this.titleService.setTitle("Tài liệu đã tải lên");
    this.breadcrumbService.addBreadcrumb("Tài liệu đã tải lên", this.router.url);

    //get documents
    this.search();
  }

  search() {
    this.documentService.getMyDocuments().subscribe(res => {
      this.componentService.$showLoadingStatus.next(false);
      this.data = res;
    });
  }

  priceFormatter(amount: number): string {
    return new Intl.NumberFormat('vi-VN').format(amount);
  }

  dateFormatter(date: Date): string {
    const now = Date.now();
    const past = date.getTime();
    const duration = (now - past) / 1000;
  
    if (duration < 60) {
      return Math.floor(duration) + " giây trước";
    } else if (duration < 3600) {
      return Math.floor(duration / 60) + " phút trước";
    } else if (duration < 86400) {
      return Math.floor(duration / 3600) + " giờ trước";
    } else {
      return Math.floor(duration / 86400) + " ngày trước";
    }
  }

  showAuthorInfo(userId: string) {
    this.userService.$showInfo.next(userId);
  }

  deleteDocument(id: string) {
    this.componentService.displayConfirmMessage("Xác nhận xóa tài liệu này?", () => {
      this.documentService.deleteDocument(id).subscribe(res => {
        this.componentService.$showLoadingStatus.next(false);
        this.componentService.displayMessage("Xóa thành công!");
        this.search();
      });
    });
  }
}
