import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Subject } from "rxjs";
import { APIResult } from "../dtos/common/api-result.dto";
import { BreadcrumbItem } from "../entities/common/breadcrumb.entity";
import { ComponentService } from "./component.service";

export interface Breadcrumb {
    name: string;
    url: string;
}

@Injectable({
    providedIn: 'root'
})
export class BreadcrumbService {
    public $data = new Subject<BreadcrumbItem[]>();

    constructor(
        private httpClient: HttpClient,
        private componentService: ComponentService
    ) {}

    public getBreadcrumb(slug: string) {
        this.httpClient
            .get<APIResult<BreadcrumbItem[]>>(`categories/breadcrumb?currentSlug=${slug}`)
            .subscribe(res => {
                this.componentService.$showLoadingStatus.next(false);
                this.$data.next(res.data!);
            });
    }
}