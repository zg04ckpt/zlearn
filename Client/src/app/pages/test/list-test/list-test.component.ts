import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list-test',
  standalone: true,
  imports: [],
  templateUrl: './list-test.component.html',
  styleUrl: './list-test.component.css'
})
export class ListTestComponent {
  constructor(private router: Router) {}
  navigate(url: string) {
    this.router.navigateByUrl("tests/" + url);
  }
}
