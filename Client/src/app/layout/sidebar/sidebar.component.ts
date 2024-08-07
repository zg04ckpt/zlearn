import { animate, state, style, transition, trigger } from '@angular/animations';
import { CommonModule, NgClass, NgStyle } from '@angular/common';
import { Component } from '@angular/core';
import { LayoutService } from '../../services/layout.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [
    NgStyle,
    NgClass,
    CommonModule,
  ],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  show: boolean = true;

  constructor(private layoutService: LayoutService) {
    layoutService.$showSidebar.subscribe(next => this.show = next);
  }

  toggleStatus() {
    this.show = !this.show;
    this.layoutService.$showSidebar.next(this.show);
  }
}
