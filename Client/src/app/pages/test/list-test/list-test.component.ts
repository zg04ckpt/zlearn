import { Component, DestroyRef, inject, Inject, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { TestItem } from '../../../entities/test/test-item.entity';
import { TestService } from '../../../services/test.service';
import { ComponentService } from '../../../services/component.service';
import { environment } from '../../../../environments/environment';
import { FormsModule } from '@angular/forms';
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { UserService } from '../../../services/user.service';
import { AuthService } from '../../../services/auth.service';

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
  pageSize: number = 6;
  pageIndex: number = 1;
  totalPage: number = 0;
  total: number = 0;
  key: string = "";
  destroyRef = inject(DestroyRef);

  constructor(
    private router: Router,
    private testService: TestService,
    private componentService: ComponentService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.search();
  }

  isLoggedIn(): boolean {
    return this.userService.getLoggedInUser() != null;
  }

  search() {
    this.componentService.$showLoadingStatus.next(true);
    this.testService.getAll(this.pageIndex, this.pageSize, this.key)
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe(res => {
        this.componentService.$showLoadingStatus.next(false);
        debugger;
        this.total = res.total;
        this.totalPage = Math.ceil(this.total / this.pageSize);
        if(this.totalPage == 0) this.pageIndex = 0;
        this.list = res.data;
      });
  }

  navigate(url: string) {
    this.router.navigateByUrl("tests/" + url);
  }
}
