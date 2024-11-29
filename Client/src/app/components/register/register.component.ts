import { Component } from '@angular/core';
import { ComponentService } from '../../services/component.service';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgClass } from '@angular/common';
import { AuthService } from '../../services/auth.service';

interface RegisterForm {
  userName: FormControl;
  email: FormControl;
  password: FormControl;
  confirmPassword: FormControl;
}

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgClass,
    FormsModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  show: boolean = false;
  form: FormGroup<RegisterForm>;
  showUserNameError: boolean = false;
  showPasswordError: boolean = false;
  showConfirmPasswordError: boolean = false;
  showEmailError: boolean = false;
  userNameRegex: string = "^[a-zA-Z0-9]+$";
  passwordRegex: string = "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[.@!#*]).+$";
  
  showErrorBorder: boolean = false;

  constructor(
    private componentService: ComponentService,
    private authService: AuthService
  ) {
    componentService.$showRegisterDialog.subscribe(value => this.show = value);
    this.form = new FormGroup({
      userName: new FormControl<string>(
        "",
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(16),
          Validators.pattern(this.userNameRegex)
        ]
      ),
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
          Validators.required,
          Validators.maxLength(16),
          Validators.minLength(8),
          Validators.pattern(this.passwordRegex)
        ]
      ),
      confirmPassword: new FormControl<string>(
        "",
        [
          Validators.required
        ]
      )
    });
  }

  userNameInvalidMessage(): string|null {
    const userNameProp = this.form.controls.userName;
    if(userNameProp.errors == null)
      return null;

    if(userNameProp.errors['required'])
      return "Tên người dùng không thể bỏ trống";

    if(userNameProp.errors['maxlength'] || userNameProp.errors['minlength'])
      return "Độ dài phải từ 2 đến 16 kí tự";

    if(userNameProp.errors['pattern'].requiredPattern === this.userNameRegex)
      return "Tên người dùng không chứa kí tự đặc biệt (kể cả có dấu)";

    return "Lỗi không xác định";
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

    if(passwordProp.errors['maxlength'] || passwordProp.errors['minlength'])
      return "Độ dài phải từ 8 đến 16 kí tự";

    if(passwordProp.errors['pattern'].requiredPattern === this.passwordRegex)
      return "Mật khẩu phải chứa kí tự in thường, in hoa, chữ số và kí tự đặc biệt .@!#*";

    return "Lỗi không xác định";
  }

  confirmPasswordInvalidMessage(): string|null {
    const passwordConfirmProp = this.form.controls.confirmPassword;

    if(passwordConfirmProp.value !== this.form.controls.password.value)
      return "Mật khẩu xác nhận không khớp";

    return null;
  }

  register() {
    // this.authService.register({
    //   userName: this.form.controls.userName.value,
    //   email: this.form.controls.email.value,
    //   password: this.form.controls.password.value,
    //   confirmPassword: this.form.controls.confirmPassword.value
    // }).subscribe({
    //   next: res => {
    //     this.componentService.$showLoadingStatus.next(false);
    //     this.componentService.$showRegisterDialog.next(false);
    //     this.componentService.displayMessage("Đăng kí thành công! Để đăng nhập, vui lòng kiểm tra email để nhận link xác thực");
    //   }
    // });
  }

  redirectToLogin() {
    this.componentService.$showLoginDialog.next(true);
    this.componentService.$showRegisterDialog.next(false);
  }
}
