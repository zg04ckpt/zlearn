import { NgClass } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Breadcrumb, BreadcrumbService } from '../../../services/breadcrumb.service';
import { BreadcrumbItem } from '../../../entities/common/breadcrumb.entity';

@Component({
  selector: 'app-breadcrumb',
  standalone: true,
  imports: [
    NgClass,
    RouterLink
  ],
  templateUrl: './breadcrumb.component.html',
  styleUrl: './breadcrumb.component.css'
})
export class BreadcrumbComponent {
  breadcrumbs: BreadcrumbItem[] = []

  constructor(
    private breadcrumbService: BreadcrumbService
  ) {
    breadcrumbService.$data.subscribe(next => this.breadcrumbs = next);
  }
}
