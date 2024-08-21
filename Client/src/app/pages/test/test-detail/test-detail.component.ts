import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ComponentService } from '../../../services/component.service';

@Component({
  selector: 'app-test',
  standalone: true,
  imports: [
    RouterLink
  ],
  templateUrl: './test-detail.component.html',
  styleUrl: './test-detail.component.css'
})
export class TestDetailComponent implements OnInit {
  id: string|null = null;
  constructor(
    private activatedRoute: ActivatedRoute,
    private componentService: ComponentService
  ) {}

  ngOnInit(): void {
    // this.id = this.activatedRoute.snapshot.paramMap.get('id');
    // if(this.id == null) {
    //   this.componentService.displayMessage("Không tìm thấy bài test");
    // }
  }

}
