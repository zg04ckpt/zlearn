import { AfterViewInit, Component, ElementRef, HostListener, Renderer2, ViewChild } from '@angular/core';
import { UserService } from '../../services/user.service';
import { Router, RouterLink } from '@angular/router';
import { StorageService } from '../../services/storage.service';
import { User } from '../../entities/user/user.entity';
import { AuthService } from '../../services/auth.service';
import { ComponentService } from '../../services/component.service';
import { NgClass } from '@angular/common';
import { LayoutService } from '../../services/layout.service';
import { Breadcrumb, BreadcrumbService } from '../../services/breadcrumb.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    RouterLink,
    NgClass
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  user: User|null = null;
  breadcrumbs: Breadcrumb[] = []
  defaultAvtUrl = environment.defaultAvtUrl
  constructor(
    private userService: UserService,
    private authService: AuthService,
    private componentService: ComponentService,
    private router: Router,
    private breadcrumbService: BreadcrumbService
  ) {
    userService.$currentUser.subscribe(next => this.user = next);
    breadcrumbService.$breadcrumb.subscribe(next => {
      if(next == null) {
        this.breadcrumbs.pop();
        return;
      }

      if(next.url == '/') {
        this.breadcrumbs = [];
        return;
      }

      const i = this.breadcrumbs.findIndex(e => e.url == next.url);
      if(i == -1) {
        this.breadcrumbs.push(next);
        if(this.breadcrumbs.length >= 3) {
          this.breadcrumbs.shift();
        }
      } else {
        this.breadcrumbs = this.breadcrumbs.slice(0, i+1);
      }
    });
  }

  showLogin() {
    this.componentService.$showLoginDialog.next(true);
    this.componentService.$showRegisterDialog.next(false);
  }

  showRegister() {
    this.componentService.$showRegisterDialog.next(true);
    this.componentService.$showLoginDialog.next(false);
  }

  logout() {
    this.authService.logout().subscribe({
      next: res => {
        debugger;
        this.componentService.$showToast.next("Đã đăng xuất");
        this.authService.purgeAuth();
        this.componentService.$showLoadingStatus.next(false);
        this.router.navigateByUrl("");
      }
    });
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: Event) {
    // console.log('Screen width:', window.innerWidth);
    // Thực hiện các hành động khác khi kích thước màn hình thay đổi
  }
}
