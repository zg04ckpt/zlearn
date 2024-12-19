import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CategoryItem } from '../../../entities/management/category-item.entity';
import { environment } from '../../../../environments/environment';
import { DatePipe, NgClass } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { BreadcrumbService } from '../../../services/breadcrumb.service';
import { DocumentService } from '../../../services/document.service';
import { DocumentItemDTO } from '../../../dtos/document/document-item';
import { ComponentService } from '../../../services/component.service';
import { DocumentSearchingDTO } from '../../../dtos/document/document-searching.dto';
import { DocumentDetailDTO } from '../../../dtos/document/document-detail.dto';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-document',
  standalone: true,
  imports: [
    FormsModule,
    NgClass,
    RouterLink,
    DatePipe
  ],
  templateUrl: './list-documents.component.html',
  styleUrl: './list-documents.component.css'
})
export class ListDocumentsComponent implements OnInit {
  defaultImageUrl = environment.defaultImageUrl;
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
    size: number,
    images: string[]
  } = {
    show: false,
    data: null,
    desc: '',
    size: 0,
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
    

    //set bread and title
    this.titleService.setTitle("Tài liệu");
    this.breadcrumbService.addBreadcrumb("Tài liệu", this.router.url);

    //get query param
    //?page=1&size=6&cate=&name=
    this.activatedRoute.queryParamMap.subscribe(next => {
      if(next.get('page')) {
        this.searchingData.pageIndex = Number(next.get('page'));
      }
      if(next.get('size')) {
        this.searchingData.pageSize = Number(next.get('size'));
      }
      if(next.get('cate')) {
        this.searchingData.categorySlug = next.get('cate')!;
      }
      if(next.get('name')) {
        this.searchingData.name = next.get('name')!;
      }

      this.search();
      //get categories
      this.documentService.getCategories().subscribe(next => {
        this.componentService.$showLoadingStatus.next(false);
        this.categories = next;
      });
    });
  }

  search() {
    this.documentService.getAsList(this.searchingData).subscribe(res => {
      this.componentService.$showLoadingStatus.next(false);
      this.searchingData.totalPage = Math.ceil(res.total / this.searchingData.pageSize);
      this.data = res.data;

      this.updateQueryParams();
    });
  }

  getDetail(document: DocumentItemDTO) {
    this.documentService.getDetail(document.id).subscribe(res => {
      debugger
      this.componentService.$showLoadingStatus.next(false);
      this.infoDialog.desc = res.description;
      this.infoDialog.images = res.previewImagePaths;
      this.infoDialog.data = document;
      this.infoDialog.size = res.size;
      this.infoDialog.show = true;
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

  sizeFormatter(size: number): string {
    if (size < 1024) {
      return `${size}B`;
    }
    size /= 1024;
    if (size < 1024) {
      return `${size.toFixed(2)}KB`;
    }
    size /= 1024;
    return `${size.toFixed(2)}MB`;
  }

  showAuthorInfo(userId: string) {
    this.userService.$showInfo.next(userId);
  }

  download(doc: DocumentItemDTO) {
    this.componentService.displayConfirmMessage("Xác nhận tải xuống", () => {
      this.documentService.download(doc.id).subscribe(res => {
        const a = document.createElement('a');
        a.href = window.URL.createObjectURL(res);
        a.download = doc.name;

        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(a.href);
      });
      this.componentService.$showLoadingStatus.next(false);
    });
  }

  updateQueryParams(): void {
    this.router.navigate([], {
      relativeTo: this.activatedRoute, 
      queryParams: { 
        page: this.searchingData.pageIndex, 
        size: this.searchingData.pageSize, 
        cate: this.searchingData.categorySlug, 
        name: this.searchingData.name 
      },
      queryParamsHandling: 'merge'
    });
  }
}
