import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ComponentService } from '../../../services/component.service';

@Component({
  selector: 'app-service-unavailable503',
  standalone: true,
  imports: [],
  templateUrl: './service-unavailable503.component.html',
  styleUrl: './service-unavailable503.component.css'
})
export class ServiceUnavailable503Component implements OnInit {
  show: boolean = false;
  constructor(
    private location: Location,
    private router: Router,
    private componentService: ComponentService
  ) { 
    this.componentService.$show503.subscribe(next => this.show = next);
  }

  ngOnInit(): void {
    
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
