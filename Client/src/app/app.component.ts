import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, interval } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(private router: Router, private activedRoute : ActivatedRoute) {}
  title = 'Client';
  sub: Subscription = new Subscription();
  showMess: boolean = true;
  ngOnInit() {
    this.sub = interval(3000).subscribe(() => {
      this.showMess = false;
    })
  }
}
