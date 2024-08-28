import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ComponentService } from '../../../services/component.service';
import { TestService } from '../../../services/test.service';
import { TestDetail } from '../../../entities/test/test-detail.entity';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-test',
  standalone: true,
  imports: [
    RouterLink,
    FormsModule
  ],
  templateUrl: './test-detail.component.html',
  styleUrl: './test-detail.component.css'
})
export class TestDetailComponent implements OnInit {
  id: string|null = null;
  data: TestDetail|null = null;
  mode: string = "practice";

  constructor(
    private activatedRoute: ActivatedRoute,
    private componentService: ComponentService,
    private testService: TestService
  ) {}

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.paramMap.get('id');
    if(this.id == null) {
      this.componentService.displayMessage("Không tìm thấy bài test");
      return;
    }

    this.componentService.$showLoadingStatus.next(true);
    this.testService.getDetail(this.id).subscribe({
      next: res => {
        this.componentService.$showLoadingStatus.next(false);
        this.data = res;
      },

      error: res => {
        this.componentService.$showLoadingStatus.next(false);
        this.componentService.displayAPIError(res);
      }
    });
  }

}
