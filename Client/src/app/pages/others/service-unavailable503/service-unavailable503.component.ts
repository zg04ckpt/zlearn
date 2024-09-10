import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ComponentService } from '../../../services/component.service';

@Component({
  selector: 'app-service-unavailable503',
  standalone: true,
  imports: [],
  templateUrl: './service-unavailable503.component.html',
  styleUrl: './service-unavailable503.component.css'
})
export class ServiceUnavailable503Component {
  show: boolean = false;
  constructor(
    private location: Location,
    private router: Router,
    private componentService: ComponentService
  ) { 
    componentService.$show503.subscribe(next => this.show = next);
  }

  goToHome() {
    this.show = false;
    this.router.navigateByUrl("/");
  }

  reload() {
    this.componentService.$show503.next(false);
    window.location.reload();
    this.show = false;
  }
}
