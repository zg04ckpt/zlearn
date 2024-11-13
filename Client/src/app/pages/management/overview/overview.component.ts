import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { BreadcrumbService } from '../../../services/breadcrumb.service';

@Component({
  selector: 'app-overview',
  standalone: true,
  imports: [],
  templateUrl: './overview.component.html',
  styleUrl: './overview.component.css'
})
export class OverviewComponent implements OnInit {
  title: string = "Quản lý";
  constructor(
    private router: Router,
    private titleService: Title,
    private breadcrumbService: BreadcrumbService
  ) {}
  ngOnInit(): void {
    this.titleService.setTitle(this.title);
    this.breadcrumbService.addBreadcrumb(this.title, this.router.url);
  }


}
