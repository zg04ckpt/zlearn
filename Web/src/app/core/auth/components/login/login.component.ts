import { NgClass, NgIf } from "@angular/common";
import { Component, DestroyRef, inject } from "@angular/core";
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { UserService } from "../../services/user.service";
import { MessageComponent } from "src/app/shared/components/message/message.component";
import { MessageService } from "src/app/shared/services/message.service";
import { Router } from "@angular/router";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { LoadingComponent } from "src/app/shared/components/loading/loading.component";
import { ToastService } from "src/app/shared/services/toast.service";

interface LoginForm {
    email: FormControl<string>,
    password: FormControl<string>,
    remember: FormControl<boolean>
}

@Component({
    selector: 'app-auth-login',
    templateUrl: './login.component.html',
    standalone: true,
    imports: [
        NgIf,
        ReactiveFormsModule,
        MessageComponent,
        LoadingComponent,
        NgClass
    ]
})
export class LoginComponent {
    isSubmitting: boolean = false;
    form: FormGroup<LoginForm>;
    destroyRef = inject(DestroyRef);
    showPass: boolean = false;
    constructor(
        private userService: UserService,
        private msgService: MessageService,
        private router: Router,
        private readonly toast: ToastService
    ) {
        this.form = new FormGroup<LoginForm>({
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
                    ],
                    nonNullable: true
                }
            ),
            remember: new FormControl(
                false,
                { nonNullable: true }
            ),
        });
    }

    onSubmit() {
        this.isSubmitting = true;
        this.userService.login({
            userName: this.form.controls['email'].value,
            password: this.form.controls['password'].value,
            remember: this.form.controls['remember'].value,
        })
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe({
            next: res => {
                this.isSubmitting = false;
                this.router.navigate([""]);
                this.toast.showToast(`Xin chào, ${res.data?.userName}`)
            },
            error: res => {
                this.isSubmitting = false;
                this.msgService.showMessage({
                    show: true,
                    title: "Lỗi",
                    msg: res.error?.message || `${res.status}: ${res.statusText}`,
                    acts: []
                });
            }
        })
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

    passError() {
        const prop = this.form.controls.password;
        if(prop.valid)  
            return null;

        //chưa click điền hoặc chưa thay đổi giá trị thì chưa check lỗi
        if(!this.form.touched && !this.form.dirty)
            return null;

        let error = prop.errors!;
        if(error['required'])
            return "Không thể bỏ trống mật khẩu";

        return null;
    }

    navigate(url: string) {
        this.router.navigate([url]);
    }
}