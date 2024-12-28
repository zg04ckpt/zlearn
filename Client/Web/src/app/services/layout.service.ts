import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class LayoutService {
    public $showSidebar = new Subject<boolean>();
    public $isLoggedIn = new Subject();
}