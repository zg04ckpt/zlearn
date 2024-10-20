import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ComponentService } from '../../../services/component.service';
import { TestService } from '../../../services/test.service';
import { TestDetail } from '../../../entities/test/test-detail.entity';
import { FormsModule } from '@angular/forms';
import { Location } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-test',
  standalone: true,
  imports: [
    RouterLink,
    FormsModule
  ],
  templateUrl: './test-detail.component.html',
  styleUrl: './test-detail.component.css'
})
export class TestDetailComponent implements OnInit {
  id: string|null = null;
  data: TestDetail|null = null;
  mode: string = "practice";
  destroyRef = inject(DestroyRef);
  isSaved: boolean = false;

  constructor(
    private activatedRoute: ActivatedRoute,
    private componentService: ComponentService,
    private testService: TestService,
    private router: Router,
    private location: Location,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.paramMap.get('id');
    if(this.id == null) {
      this.componentService.displayMessage("Không tìm thấy bài test");
      return;
    }

    this.componentService.$showLoadingStatus.next(true);
    this.testService.getDetail(this.id)
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe({
      next: res => {
        this.componentService.$showLoadingStatus.next(false);
        this.data = res;
      }
    });

    if(this.userService.getLoggedInUser() != null) {
      //check if saved
      this.testService.isSaved(this.id)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(res => {
          debugger
          this.isSaved = res;
          this.componentService.$showLoadingStatus.next(false);
        });
    }
  }

  checkPrivacy(): boolean {
    if(this.data?.isPrivate) {
      const currentUser = this.userService.getLoggedInUser();
      if(this.data.authorId != currentUser?.id) {
        return false;
      }
    }
    return true;
  }

  saveThisTest() {
    this.componentService.$showLoadingStatus.next(true);
    this.testService.saveTest(this.id!)
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe({
      next: res => {
        this.componentService.$showLoadingStatus.next(false);
        this.isSaved = true;
      },

      error: res => {
        this.componentService.$showLoadingStatus.next(false);
        this.componentService.displayAPIError(res);
      },

      complete: () => this.componentService.$showLoadingStatus.next(false)
    });
  }

  goToHome() {
    this.router.navigateByUrl("/");
  }

  back() {
    this.location.back();
  }
}
