import { DOCUMENT } from '@angular/common';
import { Component, Inject, Renderer2 } from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  constructor(private renderer: Renderer2, @Inject(DOCUMENT) private document: Document)
  {

  }

  expandLeftMenu() {
    this.renderer.addClass(this.document.documentElement, 'layout-menu-expanded');
  }
}
