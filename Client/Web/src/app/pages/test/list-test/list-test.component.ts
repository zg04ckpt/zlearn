import { Component, DestroyRef, inject, Inject, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { TestItem } from '../../../entities/test/test-item.entity';
import { TestService } from '../../../services/test.service';
import { ComponentService } from '../../../services/component.service';
import { environment } from '../../../../environments/environment';
import { FormsModule } from '@angular/forms';
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { UserService } from '../../../services/user.service';
import { AuthService } from '../../../services/auth.service';
import { Title } from '@angular/platform-browser';
import { LayoutService } from '../../../services/layout.service';
import { BreadcrumbService } from '../../../services/breadcrumb.service';
import { CategoryItem } from '../../../entities/common/category-item.entity';

@Component({
  selector: 'app-list-test',
  standalone: true,
  imports: [
    RouterLink,
    FormsModule
  ],
  templateUrl: './list-test.component.html',
  styleUrl: './list-test.component.css'
})
export class ListTestComponent implements OnInit {
  list: TestItem[] = [];
  total: number = 0;
  key: string = "";
  destroyRef = inject(DestroyRef);
  title: string = "Trắc nghiệm";
  defaultImageUrl = environment.defaultImageUrl;

  categories: CategoryItem[] = [];
  cateSlug = "";
  paging = {
    page: 1,
    size: 24,
    total: 1,
    next: () => {
      if(this.paging.page < this.paging.total) {
        this.paging.page++;
        this.search();
      }
    },
    prev: () => {
      if(this.paging.page > 1) {
        this.paging.page--;
        this.search();
      }
    },
    end: () => {
      this.paging.page = this.paging.total
      this.search();
    },
    start: () => {
      this.paging.page = 1
      this.search();
    },
  }

  constructor(
    private router: Router,
    private testService: TestService,
    private componentService: ComponentService,
    private userService: UserService,
    private titleService: Title,
    private breadcrumbService: BreadcrumbService,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.titleService.setTitle(this.title);

    //get query param
    //?page=1&size=6&cate=&name=
    this.activatedRoute.queryParamMap.subscribe(next => {
      if(next.get('page')) {
        this.paging.page = Number(next.get('page'));
      }
      if(next.get('size')) {
        this.paging.size = Number(next.get('size'));
      }
      if(next.get('cate')) {
        this.cateSlug = next.get('cate')!;
        this.breadcrumbService.getBreadcrumb(this.cateSlug)
      } else {
        this.breadcrumbService.getBreadcrumb('trac-nghiem')
      }
      if(next.get('name')) {
        this.key = next.get('name')!;
      }

      this.search();
      this.testService.getCategories().subscribe(next => {
        this.componentService.$showLoadingStatus.next(false);
        this.categories = next;
      });
    });
  }

  isLoggedIn(): boolean {
    return this.userService.getLoggedInUser() != null;
  }

  updateBreadcrumb() {
    if(this.cateSlug) {
      this.breadcrumbService.getBreadcrumb(this.cateSlug);
    } else {
      this.breadcrumbService.getBreadcrumb('trac-nghiem');
    }
  }

  search() {
    this.componentService.$showLoadingStatus.next(true);
    this.testService.getAll(this.paging.page, this.paging.size, this.key, this.cateSlug)
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe(res => {
      this.componentService.$showLoadingStatus.next(false);
      debugger;
      this.total = res.total;
      this.paging.total = Math.ceil(this.total / this.paging.size);
      
      // test
      this.list = res.data;
      // for(let i = 1; i <= 60; i++) {
      //   this.list = this.list.concat(res.data)
      // }

      //update route
      this.updateQueryParams();
    });
  }

  updateQueryParams(): void {
    this.router.navigate([], {
      relativeTo: this.activatedRoute, 
      queryParams: { 
        page: this.paging.page, 
        size: this.paging.size, 
        cate: this.cateSlug, 
        name: this.key },
      queryParamsHandling: 'merge'
    });
  }

  navigate(url: string) {
    this.router.navigateByUrl("tests/" + url);
  }
}
