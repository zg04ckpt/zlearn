import { Component, DestroyRef, inject } from '@angular/core';
import { SavedTestDTO } from '../../../dtos/test/saved-test.dto';
import { environment } from '../../../../environments/environment';
import { CategoryItem } from '../../../entities/common/category-item.entity';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Title } from '@angular/platform-browser';
import { BreadcrumbService } from '../../../services/breadcrumb.service';
import { ComponentService } from '../../../services/component.service';
import { TestService } from '../../../services/test.service';
import { UserService } from '../../../services/user.service';
import { FormsModule } from '@angular/forms';
import { ShareFunction } from '../../../utilities/share-function.uti';

@Component({
  selector: 'app-saved-tests',
  standalone: true,
  imports: [
    FormsModule,
    RouterLink
  ],
  templateUrl: './saved-tests.component.html',
  styleUrl: './saved-tests.component.css'
})
export class SavedTestsComponent {
  list: SavedTestDTO[] = [];
  destroyRef = inject(DestroyRef);
  title: string = "Đề đã lưu";
  defaultImageUrl = environment.defaultImageUrl;

  uti = new ShareFunction()

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
    this.breadcrumbService.getBreadcrumb('trac-nghiem');
    this.search();
  }

  search() {
    this.componentService.$showLoadingStatus.next(true);
    this.testService.getAllSavedTests()
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe(res => {
      this.componentService.$showLoadingStatus.next(false);
      this.list = res;
    });
  }

  removeSavedTest(id: string) {
    this.componentService.displayConfirmMessage("Xác nhận bỏ lưu?", () => {
      this.componentService.$showLoadingStatus.next(true);
      this.testService.removeSavedTest(id)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(res => {
        this.componentService.$showLoadingStatus.next(false);
        this.search();
      })
    });
  }
}
