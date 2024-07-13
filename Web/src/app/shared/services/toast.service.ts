import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

@Injectable({providedIn: 'root'})
export class ToastService {
    private displaySrc = new BehaviorSubject<string|null>(null);
    public displayStatus = this.displaySrc.asObservable();

    public showToast(msg: string) {
        this.displaySrc.next(msg);
    }
}