import { NgClass } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule, NgModel } from '@angular/forms';
import { debounceTime, Subject } from 'rxjs';
import { ComponentService } from '../../services/component.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register-2',
  standalone: true,
  imports: [
    FormsModule,
    NgClass
  ],
  templateUrl: './register-2.component.html',
  styleUrl: './register-2.component.css'
})
export class Register2Component implements OnInit {
  constructor(
    private componentService: ComponentService,
    private authService: AuthService
  ) {}

  show = false;

  emailInput = {
    value: '',
    valid: true,
    focus: false,
    listener: new Subject<string>
  };

  lastNameInput = {
    value: '',
    valid: true,
    focus: false,
    listener: new Subject<string>
  };

  firstNameInput = {
    value: '',
    valid: true,
    focus: false,
    listener: new Subject<string>
  };

  userNameInput = {
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

  ngOnInit(): void {
    this.componentService.$showRegisterDialog.subscribe(next => this.show = next);

    // listener for email
    this.emailInput.listener
      .pipe(debounceTime(1000))
      .subscribe(next => {
        const pattern = /^[a-zA-Z0-9._]+@[a-zA-Z0-9.]+\.[a-zA-Z]{2,4}$/;
        this.emailInput.valid = pattern.test(next);
      });
    
    // listener for last name
    this.lastNameInput.listener
      .pipe(debounceTime(1000))
      .subscribe(next => {
        const pattern = /^[\p{L} ]{1,100}$/u;
        this.lastNameInput.valid = pattern.test(next);
      });
    
    // listener for first name
    this.firstNameInput.listener
      .pipe(debounceTime(1000))
      .subscribe(next => {
        const pattern = /^[\p{L} ]{1,100}$/u;
        this.firstNameInput.valid = pattern.test(next);
      });

    // listener for username
    this.userNameInput.listener
      .pipe(debounceTime(1000))
      .subscribe(next => {
        const pattern = /^[a-zA-Z0-9]{2,16}$/;
        this.userNameInput.valid = pattern.test(next);
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
  }

  public submit() {
    if(
      !this.emailInput.value || 
      !this.lastNameInput.value || 
      !this.firstNameInput.value || 
      !this.userNameInput.value || 
      !this.passInput.value || 
      !this.confirmPassInput.value 
    ) {
      this.componentService.displayError("Vui lòng nhập đủ thông tin!", []);
      return;
    }

    this.authService.register({
      userName: this.userNameInput.value,
      email: this.emailInput.value,
      password: this.passInput.value,
      confirmPassword: this.confirmPassInput.value,
      lastName: this.lastNameInput.value,
      firstName: this.firstNameInput.value
    }).subscribe({
      next: res => {
        this.show = false;
        this.componentService.$showLoadingStatus.next(false);
        this.componentService.$showRegisterDialog.next(false);
        this.componentService.displayMessage("Đăng kí thành công! Để đăng nhập, vui lòng kiểm tra email để nhận link xác thực");
      }
    });
  }

  redirectToLogin() {
    this.componentService.$showLoginDialog.next(true);
    this.componentService.$showRegisterDialog.next(false);
  }
}
