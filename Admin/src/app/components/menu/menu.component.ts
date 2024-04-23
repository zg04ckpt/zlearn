import { DOCUMENT } from '@angular/common';
import { Component, Inject, Renderer2 } from '@angular/core';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent {
  isExpand: boolean = true

  constructor(private renderer: Renderer2, @Inject(DOCUMENT) private document: Document)
  {

  }

  hideMenu() 
  {
    this.renderer.removeClass(this.document.documentElement, 'layout-menu-expanded');
  }

  onClick() 
  {
    this.isExpand = !this.isExpand
  }
}
