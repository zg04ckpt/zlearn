import { Component } from "@angular/core";
import { UserService } from "../auth/services/user.service";
import { User } from "../auth/models/user.model";

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
        private readonly userService: UserService
    ) {
        this.userService.currentUser.subscribe(
            user => {
                this.user = user
                console.log(this.user);
            }
        );
    }
}