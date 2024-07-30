import { Component, EventEmitter, Input, Output } from "@angular/core";
import { UserService } from "../../auth/services/user.service";
import { User } from "../../auth/models/user.model";
import { MessageService } from "src/app/shared/services/message.service";
import { ToastService } from "src/app/shared/services/toast.service";
import { Router } from "@angular/router";
import { NgClass, NgIf, NgStyle } from "@angular/common";
import { animate, state, style, transition, trigger } from "@angular/animations";

@Component({
    selector: "app-layout-sidebar",
    templateUrl: "./sidebar.component.html",
    styleUrl: "./sidebar.component.css",
    imports: [
        NgIf,
        NgStyle,
        NgClass
    ],
    standalone: true,
    animations: [
        trigger('toggleAnimation', [
            state('visible', style({
                transform: 'translateX(0%)'
            })),
            state('hide', style({
                transform: 'translateX(-100%)'
            })),
            transition('visible => hide', [
                animate('0.3s 0s ease-out')
            ]),
            transition('hide => visible', [
                animate('0.3s 0s ease-in')
            ])
        ])
    ]
})
export class SidebarComponent {
    @Input() show: boolean = true;

    //truyền sự kiện ẩn hiện slidebar
    @Output() toggle = new EventEmitter<boolean>();

    constructor() {}

    toggleStatus() {
        this.show = !this.show;
        this.toggle.emit(this.show);
    }
}