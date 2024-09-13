import { Location } from '@angular/common';
import { Component, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { Router } from '@angular/router';
import { ComponentService } from '../../../services/component.service';

@Component({
  selector: 'app-notfound404',
  templateUrl: './notfound404.component.html',
  styleUrls: ['./notfound404.component.css'],
  standalone: true
})
export class Notfound404Component {
  constructor(
    private router: Router
  ) {
  }

  goToHome() {
    this.router.navigateByUrl("/");
  }

  back() {
    history.back();
  }
}