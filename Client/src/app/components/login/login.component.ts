import { Component, NgModule } from '@angular/core';
import { Location, NgClass } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { ComponentService } from '../../services/component.service';
import { FormControl, FormGroup, FormsModule, NgModel, ReactiveFormsModule, Validator, Validators } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { RouterEvent, RouterLink } from '@angular/router';

interface LoginForm {
  email: FormControl;
  password: FormControl;
  remember: FormControl;
}

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    NgClass,
    FormsModule,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  show: boolean = false;
  form: FormGroup<LoginForm>;
  showEmailError: boolean = false;
  showPasswordError: boolean = false;

  constructor(
    private componentService: ComponentService,
    private authService: AuthService,
    private userService: UserService,
    private location: Location
  ) {
    componentService.$showLoginDialog.subscribe(value => this.show = value);
    this.form = new FormGroup<LoginForm>({
      email: new FormControl<string>(
        "",
        [
          Validators.required,
          Validators.email
        ]
      ),
      password: new FormControl<string>(
        "",
        [
          Validators.required
        ]
      ),
      remember: new FormControl<boolean>(false)
    });
  }

  emailInvalidMessage(): string|null {
    const emailProp = this.form.controls.email;

    if(emailProp.errors == null)
      return null;

    if(emailProp.errors['required'])
      return "Email không thể bỏ trống";

    if(emailProp.errors['email'])
      return "Email không hợp lệ";

    return "Lỗi không xác định";
  }

  passwordInvalidMessage(): string|null {
    const passwordProp = this.form.controls.password;
    if(passwordProp.errors == null)
      return null;

    if(passwordProp.errors['required'])
      return "Mật khẩu không thể bỏ trống";

    return "Lỗi không xác định";
  }

  login() {
    this.componentService.$showLoadingStatus.next(true);
    this.authService.login({
      email: this.form.controls.email.value,
      password: this.form.controls.password.value,
      remember: this.form.controls.remember.value
    }).subscribe({
      next: res => {
        debugger
        this.componentService.$showLoadingStatus.next(false);
        this.componentService.$showLoginDialog.next(false);
        this.componentService.$showToast.next(`Xin chào ${res.fullName || res.userName}!`);
        this.userService.$currentUser.next(res);
        this.authService.setLoginSessionTimer();
      }
    });
  }

  redirectToRegister() {
    this.componentService.$showRegisterDialog.next(true);
    this.componentService.$showLoginDialog.next(false);
  }

  closeLoginDialog() {
    this.componentService.$showLoginDialog.next(false);
  }
}
