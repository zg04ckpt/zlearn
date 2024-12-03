import { NgClass } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { debounceTime, Subject } from 'rxjs';
import { ComponentService } from '../../services/component.service';
import { AuthService } from '../../services/auth.service';
import { UserService } from '../../services/user.service';
import { FactoryTarget } from '@angular/compiler';

@Component({
  selector: 'app-login-2',
  standalone: true,
  imports: [
    FormsModule,
    RouterLink,
    NgClass
  ],
  templateUrl: './login-2.component.html',
  styleUrl: './login-2.component.css'
})
export class Login2Component implements OnInit {
  constructor(
    private componentService: ComponentService,
    private authService: AuthService,
    private userService: UserService,
  ) {}

  show = false;

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

  remember = false;

  public ngOnInit(): void {
    this.componentService.$showLoginDialog.subscribe(next => this.show = next);

    // listener for email
    this.emailInput.listener
      .pipe(debounceTime(1000))
      .subscribe(next => {
        const pattern = /^[a-zA-Z0-9._]+@[a-zA-Z0-9.]+\.[a-zA-Z]{2,4}$/;
        this.emailInput.valid = pattern.test(next);
      });
    
    // listener for email
    this.passInput.listener
      .pipe(debounceTime(1000))
      .subscribe(next => {
        this.passInput.valid = next != "";
      });
  }

  public submit() {
    if(!this.emailInput.value || !this.passInput.value) {
      this.componentService.displayError("Vui lòng nhập đủ thông tin!", []);
      return;
    }

    this.authService.login({
      email: this.emailInput.value,
      password: this.passInput.value,
      remember: this.remember
    }).subscribe({
      next: res => {
        debugger
        this.componentService.$showLoadingStatus.next(false);
        this.show = false;
        this.componentService.$showToast.next(`Xin chào ${res.fullName}!`);
        this.userService.$currentUser.next(res);
        this.authService.setLoginSessionTimer();
      }
    });
  }

  redirectToRegister() {
    this.componentService.$showRegisterDialog.next(true);
    this.show = false;
  }

  closeLoginDialog() {
    this.show = false;
  }
}
