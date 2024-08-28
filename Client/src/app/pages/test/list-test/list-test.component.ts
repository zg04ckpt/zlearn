import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { TestItem } from '../../../entities/test/test-item.entity';
import { TestService } from '../../../services/test.service';
import { ComponentService } from '../../../services/component.service';

@Component({
  selector: 'app-list-test',
  standalone: true,
  imports: [
    RouterLink
  ],
  templateUrl: './list-test.component.html',
  styleUrl: './list-test.component.css'
})
export class ListTestComponent implements OnInit {
  list: TestItem[] = [];
  pageSize: number = 10;
  pageIndex: number = 1;
  total: number = 0;

  constructor(
    private router: Router,
    private testService: TestService,
    private componentService: ComponentService
  ) {}

  ngOnInit(): void {
    this.search("");
  }

  search(key: string) {
    this.componentService.$showLoadingStatus.next(true);
    this.testService.getAll(this.pageIndex, this.pageSize, key).subscribe({
      next: res => {
        debugger;
        this.componentService.$showLoadingStatus.next(false);
        this.total = res.total;
        this.list = res.data;
      },

      error: res => {
        this.componentService.$showLoadingStatus.next(false);
        this.componentService.displayAPIError(res);
      }
    });
  }

  navigate(url: string) {
    this.router.navigateByUrl("tests/" + url);
  }
}
