import { NgClass } from "@angular/common";
import { Component, HostListener, OnInit } from "@angular/core";
import { Router, RouterLink } from "@angular/router";
import { User } from "../../../entities/user/user.entity";
import { Breadcrumb, BreadcrumbService } from "../../../services/breadcrumb.service";
import { environment } from "../../../../environments/environment";
import { UserService } from "../../../services/user.service";
import { AuthService } from "../../../services/auth.service";
import { ComponentService } from "../../../services/component.service";


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
export class HeaderComponent implements OnInit {
  user: User|null = null;
  breadcrumbs: Breadcrumb[] = []
  defaultAvtUrl = environment.defaultAvtUrl;

  isShowNotification = false;
  isShowNotificationDetail = false;
  notifications = [
    'test',
    'test',
    'test',
    'test',
    'test',
    'test',
    'test',
  ];


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

  ngOnInit(): void {
    //add close notification event
    window.onclick = (e: MouseEvent) => {
      const target = e.target as HTMLElement;
      const notificationElement = document.getElementById('noti');
      const bellIcon = target.closest('.label[title="Thông báo"]');

      if (notificationElement && !notificationElement.contains(target) && !bellIcon) {
        this.isShowNotification = false;
      }
    };
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
