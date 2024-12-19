import { NgClass } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { DocumentSearchingDTO } from '../../../dtos/document/document-searching.dto';
import { Title } from '@angular/platform-browser';
import { DocumentItemDTO } from '../../../dtos/document/document-item';
import { CategoryItem } from '../../../entities/management/category-item.entity';
import { BreadcrumbService } from '../../../services/breadcrumb.service';
import { ComponentService } from '../../../services/component.service';
import { DocumentService } from '../../../services/document.service';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-doc-management',
  standalone: true,
  imports: [
    FormsModule,
    NgClass,
    RouterLink
  ],
  templateUrl: './doc-management.component.html',
  styleUrl: './doc-management.component.css'
})
export class DocManagementComponent implements OnInit {
  searchingData: DocumentSearchingDTO = {
    pageIndex: 1,
    pageSize: 20,
    totalPage: 1,
    name: '',
    categorySlug: ''
  };

  categories: CategoryItem[] = [];
  data: DocumentItemDTO[] = [];
  infoDialog:{
    show: boolean,
    data: DocumentItemDTO|null,
    desc: string,
    images: string[]
  } = {
    show: false,
    data: null,
    desc: '',
    images: [
    ]
  }

  constructor(
    private documentService: DocumentService,
    private componentService: ComponentService,
    private userService: UserService,
    private titleService: Title,
    private breadcrumbService: BreadcrumbService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.titleService.setTitle("Quản lý tài liệu");
    this.breadcrumbService.addBreadcrumb("Quản lý tài liệu", this.router.url);

    this.search();
    //get categories
    this.documentService.getCategories().subscribe(next => {
      this.componentService.$showLoadingStatus.next(false);
      this.categories = next;
    });
  }

  search() {
    this.documentService.getAsList(this.searchingData).subscribe(res => {
      this.componentService.$showLoadingStatus.next(false);
      this.searchingData.totalPage = Math.ceil(res.total / this.searchingData.pageSize);
      this.data = res.data;
    });
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

  getDetail(document: DocumentItemDTO) {
    this.documentService.getDetail(document.id).subscribe(res => {
      debugger
      this.componentService.$showLoadingStatus.next(false);
      this.infoDialog.desc = res.description;
      this.infoDialog.images = res.previewImagePaths;
      this.infoDialog.data = document;
      this.infoDialog.show = true;
    });
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
