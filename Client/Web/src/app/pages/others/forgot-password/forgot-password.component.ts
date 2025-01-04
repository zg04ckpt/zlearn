import { NgClass } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { debounceTime, distinctUntilChanged, Subject } from 'rxjs';
import { Breadcrumb, BreadcrumbService } from '../../../services/breadcrumb.service';
import { AuthService } from '../../../services/auth.service';
import { ComponentService } from '../../../services/component.service';
import { LayoutService } from '../../../services/layout.service';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [
    NgClass,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css'
})
export class ForgotPasswordComponent implements OnInit {
  userId: string|null = null;
  token: string|null = null;
  showForgotPassword: boolean = false;
  showResetPassword: boolean = true;

  emailInput = {
    value: '',
    valid: true,
    focus: false,
    listener: new Subject<string>
  };
  
  passInput = {
    value: '',
    valid: true,
    focus: false,
    listener: new Subject<string>
  };

  confirmPassInput = {
    value: '',
    valid: true,
    focus: false,
    listener: new Subject<string>
  };

  constructor(
    private activatedRoute: ActivatedRoute,
    private titleService: Title,
    private breadcrumbService: BreadcrumbService,
    private layoutService: LayoutService,
    private router: Router,
    private authService: AuthService,
    private componentService: ComponentService
  ) {}

  ngOnInit(): void {
    this.layoutService.$showSidebar.next(false);

    // get params
    this.activatedRoute.queryParams.subscribe(next => {
      this.userId = next['id'];
      this.token = next['token'];
      if(this.userId && this.token) {
        this.showForgotPassword = false;
        this.showResetPassword = true;
      } else {
        this.showForgotPassword = true;
        this.showResetPassword = false;
      }
    });

    // listener for email
    this.emailInput.listener
      .pipe(debounceTime(1000))
      .subscribe(next => {
        const pattern = /^[a-zA-Z0-9._]+@[a-zA-Z0-9.]+\.[a-zA-Z]{2,4}$/;
        this.emailInput.valid = pattern.test(next);
      });

    // pass
    this.passInput.listener
      .pipe(debounceTime(1000))
      .subscribe(next => {
        const pattern = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[.@!#*]).{8,16}$/;
        this.passInput.valid = pattern.test(next);
      });
    
    // confirm pass
    this.confirmPassInput.listener
      .pipe(debounceTime(1000))
      .subscribe(next => {
        this.confirmPassInput.valid = next == this.passInput.value;
      });
    
    // other
    this.titleService.setTitle("Quên mật khẩu");
    // this.breadcrumbService.getBreadcrumb('quen-mat-khau');
  }

  public forgotPassword() {
    this.authService.forgotPassword(this.emailInput.value).subscribe(next => {
      if(next.success) {
        this.componentService.$showLoadingStatus.next(false);
        this.componentService.displayMessage(next.message!);
        this.router.navigateByUrl("/");
      } else {
        this.componentService.$showLoadingStatus.next(false);
        this.componentService.displayMessage(next.message!);
      }
    });
  }

  public resetPassword() {
    this.authService.resetPassword(
      this.passInput.value,
      this.confirmPassInput.value,
      this.userId!,
      this.token!
    ).subscribe(next => {
      if(next.success) {
        this.componentService.$showLoadingStatus.next(false);
        this.componentService.displayMessageWithActions(
          next.message!,
          [ {name: "Đăng nhập", action: () => this.componentService.$showLoginDialog.next(true)} ]
        );
        this.router.navigateByUrl("/");
      } else {
        this.componentService.$showLoadingStatus.next(false);
        this.componentService.displayMessage(next.message!);
      }
    });
  }
}
