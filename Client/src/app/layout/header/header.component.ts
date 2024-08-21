import { Component, HostListener } from '@angular/core';
import { UserService } from '../../services/user.service';
import { Router, RouterLink } from '@angular/router';
import { StorageService } from '../../services/storage.service';
import { User } from '../../entities/user/user.entity';
import { AuthService } from '../../services/auth.service';
import { ComponentService } from '../../services/component.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    RouterLink
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  user: User|null = null;

  constructor(
      private userService: UserService,
      private authService: AuthService,
      private componentService: ComponentService,
      private router: Router
  ) {
      userService.$currentUser.subscribe(next => this.user = next);
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
        this.router.navigateByUrl("");
      },
      error: res => {
        // this.componentService.displayMessage(res.error?.message || res.statusText);
      }
    });
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: Event) {
    console.log('Screen width:', window.innerWidth);
    // Thực hiện các hành động khác khi kích thước màn hình thay đổi
  }
}
