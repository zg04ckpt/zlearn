import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ComponentService } from '../../../services/component.service';

@Component({
  selector: 'app-forbidden403',
  standalone: true,
  imports: [],
  templateUrl: './forbidden403.component.html',
  styleUrl: './forbidden403.component.css'
})
export class Forbidden403Component {
  show: boolean = false;
  constructor(
    private router: Router,
    private componentService: ComponentService
  ) { 
    componentService.$show403.subscribe(next => this.show = next);
  }

  goToHome() {
    this.show = false;
    this.router.navigateByUrl("/");
  }

  back() {
    this.show = false;
    history.go(-1);
  }
}
