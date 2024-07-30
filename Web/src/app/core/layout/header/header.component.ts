import { Component, Input } from "@angular/core";
import { UserService } from "../../auth/services/user.service";
import { User } from "../../auth/models/user.model";
import { MessageService } from "src/app/shared/services/message.service";
import { ToastService } from "src/app/shared/services/toast.service";
import { Router, RouterLink } from "@angular/router";
import { NgStyle } from "@angular/common";

@Component({
    selector: "app-layout-header",
    templateUrl: "./header.component.html",
    styleUrl: "./header.component.css",
    imports: [
        NgStyle,
        RouterLink
    ],
    standalone: true
})
export class HeaderComponent {
    user: User|null = null;
    @Input() sidebarIsShowing: boolean = true;

    constructor(
        private readonly userService: UserService,
        private readonly messageService: MessageService,
        private readonly toastService: ToastService,
        private readonly router: Router
    ) {
        this.userService.currentUser.subscribe(
            user => {
                this.user = user
                // console.log(this.user);
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