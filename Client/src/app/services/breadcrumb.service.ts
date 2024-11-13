import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

export interface Breadcrumb {
    name: string;
    url: string;
}

@Injectable({
    providedIn: 'root'
})
export class BreadcrumbService {
    public $breadcrumb = new Subject<Breadcrumb>();

    public addBreadcrumb(name: string, url: string) {
        this.$breadcrumb.next({name, url});
    }
}