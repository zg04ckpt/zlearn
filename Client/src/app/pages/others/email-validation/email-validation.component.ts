import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ComponentService } from '../../../services/component.service';
import { AuthService } from '../../../services/auth.service';
import { LayoutService } from '../../../services/layout.service';
import { Title } from '@angular/platform-browser';
import { BreadcrumbService } from '../../../services/breadcrumb.service';

@Component({
  selector: 'app-email-validation',
  standalone: true,
  imports: [],
  templateUrl: './email-validation.component.html',
  styleUrl: './email-validation.component.css'
})
export class EmailValidationComponent implements OnInit {
  isSending: boolean = true;
  isSuccess: boolean = false;
  error: string = "";
  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private componentService: ComponentService,
    private authService: AuthService,
    private layoutService: LayoutService,
    private titleService: Title,
    private breadcrumbService: BreadcrumbService
  ) { }
  
  ngOnInit(): void {
    this.layoutService.$showSidebar.next(false);
    this.componentService.$showLoadingStatus.next(true);
    this.activatedRoute.queryParams.subscribe(param => {
      const id = param['id'];
      const token = param['token'];
      if(id && token) {
        this.authService.confirmEmail({
          userId: id, 
          token: token
        }).subscribe({
          next: res => {
            this.isSending = false;
            this.isSuccess = true;
            this.componentService.$showLoadingStatus.next(false);
          },

          error: res => {
            this.isSending = false;
            this.isSuccess = false;
            this.error = res.error?.message || res.statusText;
            this.componentService.$showLoadingStatus.next(false);
          }
        })
      } else {
        this.isSending = false;
        this.isSuccess = false;
        this.error = "Link xác thực không hợp lệ!";
        this.componentService.$showLoadingStatus.next(false);
      }
    });
    // other
    this.titleService.setTitle("Xác nhận email");
    this.breadcrumbService.addBreadcrumb("Xác nhận email", this.router.url);
  }

  showLogin() {
    this.componentService.$showLoginDialog.next(true);
  }

  navigate(url: string) {
    this.router.navigateByUrl(url);
  }
}
