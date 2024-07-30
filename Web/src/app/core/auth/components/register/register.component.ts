import { NgClass, NgIf } from "@angular/common";
import { Component, DestroyRef, EventEmitter, inject, OnInit, Output } from "@angular/core";
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { UserService } from "../../services/user.service";
import { MessageComponent } from "src/app/shared/components/message/message.component";
import { MessageService } from "src/app/shared/services/message.service";
import { Router } from "@angular/router";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { LoadingComponent } from "src/app/shared/components/loading/loading.component";

interface RegisterForm {
    email: FormControl<string>,
    password: FormControl<string>,
    confirmPassword: FormControl<string>,
}

@Component({
    selector: 'app-auth-register',
    templateUrl: './register.component.html',
    standalone: true,
    imports: [
        NgIf,
        ReactiveFormsModule,
        MessageComponent,
        NgClass,
        LoadingComponent
    ]
})
export class RegisterComponent implements OnInit {
    showPass: boolean = false;
    showConfirmPass: boolean = false;
    isSubmitting: boolean = false;
    form: FormGroup<RegisterForm>;
    destroyRef = inject(DestroyRef);
    @Output() initEvent = new EventEmitter();

    constructor(
        private userService: UserService,
        private msgService: MessageService,
        private router: Router
    ) {
        this.form = new FormGroup<RegisterForm>({
            email: new FormControl(
                "",
                {
                    validators: [
                        Validators.required,
                        Validators.email
                    ],
                    nonNullable: true
                }
            ),
            password: new FormControl(
                "",
                {
                    validators: [
                        Validators.required,
                        Validators.pattern(/(?=.*[!@#.$])/),
                        Validators.pattern(/(?=.*[a-z])/),
                        Validators.pattern(/(?=.*[A-Z])/),
                        Validators.pattern(/(?=.*[0-9])/),
                        Validators.pattern(/[a-zA-z0-9!@#.$]*/),
                        Validators.maxLength(16),
                        Validators.minLength(8)
                    ],
                    nonNullable: true
                }
            ),
            confirmPassword: new FormControl(
                "",
                {
                    validators: [
                        Validators.required
                    ],
                    nonNullable: true
                }
            ),
        });
    }

    ngOnInit(): void {
        this.initEvent.emit();
    }

    emailError() {
        const prop = this.form.controls.email;
        if(prop.valid)  
            return null;

        //chưa click điền hoặc chưa thay đổi giá trị thì chưa check lỗi
        if(!this.form.touched && !this.form.dirty)
            return null;

        let error = prop.errors!;
        if(error['email'])
            return "Sai định dạng email";

        if(error['required'])
            return "Không thể bỏ trống email";

        return null;
    }

    passError(): string | null {
        const prop = this.form.controls.password;
        if(prop.valid)  
            return null;

        //chưa click điền hoặc chưa thay đổi giá trị thì chưa check lỗi
        if(!this.form.touched && !this.form.dirty && !this.isSubmitting)
            return null;

        let error = prop.errors!;
        if(error['required'])
            return "Không thể bỏ trống mật khẩu";

        if(error['pattern'])
        {
            let pattern = error['pattern'].requiredPattern;
            if(pattern == "/(?=.*[!@#.$])/")
                return "Mật khẩu phải chứa ít nhất 1 kí tự đặc biệt: !@#.$"
    
            if(pattern == "/(?=.*[a-z])/")
                return "Mật khẩu phải chứa ít nhất 1 kí tự thường"

            if(pattern == "/(?=.*[A-Z])/")
                return "Mật khẩu phải chứa ít nhất 1 kí tự in hoa"

            if(pattern == "/(?=.*[0-9])/")
                return "Mật khẩu phải chứa ít nhất 1 kí tự số 0-9"
            
            if(pattern == "/[a-zA-z0-9!@#.$]*/")
                return "Mật khẩu chỉ chứa các chữ số, kí tự chữ cái và !@#.$"
        }

        if(error['maxlength'])
            return `Độ dài tối đa: ${error['maxlength'].requiredLength}`;
        
        if(error['minlength'])
            return `Độ dài tối thiểu: ${error['minlength'].requiredLength}`;

        return null;
    }

    confirmPassError() {
        const prop = this.form.controls.confirmPassword;

        //chưa click điền hoặc chưa thay đổi giá trị thì chưa check lỗi
        if(!this.form.touched && !this.form.dirty)
            return null;

        if(!prop.value || prop.value != this.form.controls.password.value) {
            return "Mật khẩu xác nhận không đúng";
        }

        return null;
    }

    onSubmit() {
        this.isSubmitting = true;
        this.userService.register({
            email: this.form.controls.email.value,
            password: this.form.controls.password.value,
            confirmPassword: this.form.controls.confirmPassword.value
        })
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe({
            next: res => {
                this.isSubmitting = false;
                this.msgService.showMessage({
                    show: true,
                    title: "Hoàn tất đăng kí tài khoản",
                    msg: `<div class="test-center fs-6">Vui lòng kiểm tra <b>${this.form.controls.email.value}</b> để nhận được link xác thực email.</div>`,
                    acts: [
                        { label: "Trang chủ", act: () => this.router.navigate([''])}
                    ]
                });
            },
            error: res => {
                this.isSubmitting = false;
                this.msgService.showMessage({
                    show: true,
                    title: "Lỗi",
                    msg: res.error?.message || `${res.status}: ${res.statusText}`,
                    acts: [ ]
                })
            }
        })
    }

    navigate(url: string) {
        this.router.navigate([url]);
    }
}