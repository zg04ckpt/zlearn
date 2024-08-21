import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ComponentService } from '../../../services/component.service';
import { AuthService } from '../../../services/auth.service';
import { LayoutService } from '../../../services/layout.service';

@Component({
  selector: 'app-email-validation',
  standalone: true,
  imports: [],
  templateUrl: './email-validation.component.html',
  styleUrl: './email-validation.component.css'
})
export class EmailValidationComponent {
  isSending: boolean = true;
  isSuccess: boolean = false;
  error: string = "";
  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private componentService: ComponentService,
    private authService: AuthService,
    private layoutService: LayoutService
  ) {
    layoutService.$showSidebar.next(false);
    componentService.$showLoadingStatus.next(true);
    activatedRoute.queryParams.subscribe(param => {
      const id = param['id'];
      const token = param['token'];
      if(id && token) {
        authService.confirmEmail({
          userId: id, 
          token: token
        }).subscribe({
          next: res => {
            this.isSending = false;
            this.isSuccess = true;
            componentService.$showLoadingStatus.next(false);
          },

          error: res => {
            this.isSending = false;
            this.isSuccess = false;
            this.error = res.error?.message || res.statusText;
            componentService.$showLoadingStatus.next(false);
          }
        })
      } else {
        this.isSending = false;
        this.isSuccess = false;
        this.error = "Link xác thực không hợp lệ!";
        componentService.$showLoadingStatus.next(false);
      }
    });
    
  }

  showLogin() {
    this.componentService.$showLoginDialog.next(true);
  }

  navigate(url: string) {
    this.router.navigateByUrl(url);
  }
}
