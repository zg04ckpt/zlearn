import { Component, OnInit } from '@angular/core';
import { ToastComponent } from './shared/components/toast/toast.component';
import { ToastService } from './shared/services/toast.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  
  title = 'Web';

  constructor(
    
  ) { }

  ngOnInit(): void {
  }
}
