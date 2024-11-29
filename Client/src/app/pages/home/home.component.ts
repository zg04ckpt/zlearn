import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { BreadcrumbService } from '../../services/breadcrumb.service';
import { Title } from '@angular/platform-browser';
import { HomeService } from '../../services/home.service';
import { TestItem } from '../../entities/test/test-item.entity';
import { NgClass } from '@angular/common';
import { ComponentService } from '../../services/component.service';
import { UserDetail } from '../../entities/user/user-detail.entity';
import { UserInfo } from '../../entities/user/user-info.entity';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    NgClass,
    RouterLink
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  title: string = 'Trang chủ';
  randomTests: TestItem[] = [];
  topUsers: UserInfo[] = [];
  topTests: TestItem[] = [];

  constructor(
    private router: Router,
    private breadcrumbService: BreadcrumbService,
    private titleService: Title,
    private componentService: ComponentService,
    private homeService: HomeService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.breadcrumbService.addBreadcrumb("", "/");
    this.titleService.setTitle(this.title + " - ZLEARN");

    // Random 10 tests
    this.homeService.getRandomTest(10).subscribe(res => {
      debugger
      this.randomTests = res;
      this.componentService.$showLoadingStatus.next(false);
    });

    // Top 10 users
    this.homeService.getTopUsers(10).subscribe(res => {
      debugger
      this.topUsers = res;
      this.componentService.$showLoadingStatus.next(false);
    });

    // Top10 tests
    this.homeService.getTopTest(10).subscribe(res => {
      debugger
      this.topTests = res;
      this.componentService.$showLoadingStatus.next(false);
    });
  }

  showInfo(userId: string) {
    this.userService.$showInfo.next(userId);
  }
}
