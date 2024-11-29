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
    public $breadcrumb = new Subject<Breadcrumb|null>();

    public addBreadcrumb(name: string, url: string) {
        this.$breadcrumb.next({name, url});
    }

    public popBreadcrumb() {
        this.$breadcrumb.next(null);
    }
}