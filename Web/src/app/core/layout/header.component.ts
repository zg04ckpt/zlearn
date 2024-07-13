import { Component } from "@angular/core";
import { UserService } from "../auth/services/user.service";
import { User } from "../auth/models/user.model";
import { MessageService } from "src/app/shared/services/message.service";
import { ToastService } from "src/app/shared/services/toast.service";
import { Route, Router } from "@angular/router";

@Component({
    selector: "app-layout-header",
    templateUrl: "./header.component.html",
    imports: [

    ],
    standalone: true
})
export class HeaderComponent {
    user: User|null = null;

    constructor(
        private readonly userService: UserService,
        private readonly messageService: MessageService,
        private readonly toastService: ToastService,
        private readonly router: Router
    ) {
        this.userService.currentUser.subscribe(
            user => {
                this.user = user
                console.log(this.user);
            }
        );
    }

    logout() {
        this.userService.logout().subscribe({
            next: res => {
                this.userService.purgeAuth();
                this.toastService.showToast("Đã đăng xuất");
                this.router.navigate(['']);
            },
            error: res => this.messageService.showMessage({
                show: true,
                title: "Lỗi",
                msg: res.error?.message || res.status + ": " + res.statusText,
                acts: []
            })
        })
    }
}