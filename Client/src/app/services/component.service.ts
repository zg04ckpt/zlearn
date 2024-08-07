import { Injectable } from "@angular/core";
import { Subject } from "rxjs";
import { MessageModule } from "../components/message/message.component";
import { Route, Router } from "@angular/router";
import { AuthService } from "./auth.service";

@Injectable({
    providedIn: 'root'
})
export class ComponentService {
    public $showLoginDialog = new Subject<boolean>();
    public $showRegisterDialog = new Subject<boolean>();
    public $showLoadingStatus = new Subject<boolean>();
    public $showMessage = new Subject<MessageModule>();
    public $showToast = new Subject<string>();

    constructor(
        private router: Router,
    ) { }

    public displayMessage(msg: string) {
        this.$showMessage.next({message: msg, buttons: []});
    }

    public displayMessageWithActions(
        msg: string, 
        buttons: {
            name: string,
            action: () => void
        }[]
    ) {
        this.$showMessage.next({message: msg, buttons: buttons});
    }
}