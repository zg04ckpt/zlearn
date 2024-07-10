import { NgIf } from "@angular/common";
import { Component, DestroyRef, inject } from "@angular/core";
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { UserService } from "../services/user.service";
import { MessageComponent } from "src/app/shared/components/message/message.component";
import { MessageService } from "src/app/shared/services/message.service";
import { Router } from "@angular/router";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";

interface LoginForm {
    email: FormControl<string>,
    password: FormControl<string>,
    remember: FormControl<boolean | null>
}

@Component({
    selector: 'app-auth-login',
    templateUrl: './login.component.html',
    standalone: true,
    imports: [
        NgIf,
        ReactiveFormsModule,
        MessageComponent
    ]
})
export class LoginComponent {
    isSubmitting: boolean = false;
    form: FormGroup;
    destroyRef = inject(DestroyRef);

    constructor(
        private userService: UserService,
        private msgService: MessageService,
        private router: Router
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
                false
            ),
        });
    }

    onSubmit() {

        this.userService.login({
            userName: this.form.controls['email'].value,
            password: this.form.controls['password'].value,
            remember: this.form.controls['remember'].value,
        })
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe({
            next: res => {
                this.router.navigate([""]);
            },
            error: err => {
                this.msgService.showMessage({
                    show: true,
                    title: "Lỗi",
                    msg: err.message || "null",
                    acts: [
                        { label: "Quay về trang chủ", url: "" }
                    ]
                });
            }
        })
    }
}