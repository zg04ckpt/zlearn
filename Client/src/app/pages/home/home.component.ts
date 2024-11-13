import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BreadcrumbService } from '../../services/breadcrumb.service';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  title: string = 'Trang chủ';

  constructor(
    private router: Router,
    private breadcrumbService: BreadcrumbService,
    private titleService: Title
  ) {}

  ngOnInit(): void {
    this.breadcrumbService.addBreadcrumb("", "/");
    this.titleService.setTitle(this.title + " - ZLEARN");
  }
}
