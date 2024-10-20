import { Injectable } from "@angular/core";
import { Subject } from "rxjs";
import { MessageModule } from "../components/message/message.component";
import { Route, Router } from "@angular/router";
import { AuthService } from "./auth.service";
import { APIError } from "../dtos/common/api-result.dto";

@Injectable({
    providedIn: 'root'
})
export class ComponentService {
    public $showLoginDialog = new Subject<boolean>();
    public $showRegisterDialog = new Subject<boolean>();
    public $showLoadingStatus = new Subject<boolean>();
    public $showMessage = new Subject<MessageModule>();
    public $showToast = new Subject<string>();
    public $show503 = new Subject<boolean>();
    public $show403 = new Subject<boolean>();
    public $show404 = new Subject<boolean>();

    constructor(
        private router: Router,
    ) { }

    public closeAllComponent() {
        this.$showLoginDialog.next(false);
        this.$showRegisterDialog.next(false);
        this.$showLoadingStatus.next(false);
    }

    public displayMessage(msg: string) {
        this.$showMessage.next({message: msg, isInfo: true, buttons: [
            { name: "OK", action: () => {} }
        ]});
    }

    public displayMessageWithActions(
        msg: string, 
        buttons: {
            name: string,
            action: () => void
        }[]
    ) {
        this.$showMessage.next({message: msg, isInfo: true, buttons: buttons});
    }

    public displayConfirmMessage(msg: string, onConfirm: () => void) {
        this.displayMessageWithActions(
            msg,
            [
              { name: "Hủy", action: () => {} },
              { name: "Xác nhận", action: onConfirm }
            ]
          );
    }

    public displayYesNoConfirmMessage(msg: string, onConfirm: () => void, onCancel: () => void) {
        this.displayMessageWithActions(
            msg,
            [
              { name: "Không", action: onCancel },
              { name: "OK", action: onConfirm }
            ]
        );
    }

    public displayAPIError(error: any) {
        debugger
        this.$showMessage.next({
            message: error.Message || Object.values(error.errors).flat().join(', ') || "Lỗi không xác định", 
            isInfo: false, 
            buttons: [
                { name: "Đóng", action: () => {} }
            ]
        });
    }

    public displayError(
        msg: string,
        buttons: {
            name: string,
            action: () => void
        }[]
    ) {
        this.$showMessage.next({message: msg, isInfo: false, buttons: buttons});
    }
}