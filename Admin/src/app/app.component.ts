import { Component } from '@angular/core';
import { NavigationEnd, Route, Router } from '@angular/router';
import { filter } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Admin';
  isLogin: boolean = false;
  constructor() {
  }
  ngOnInit() {
    this.isLogin = (localStorage.getItem('token') === null);
  }
}
