import { Component, OnInit } from '@angular/core';
import { ManagementService } from '../../../services/management.service';
import { LogDTO } from '../../../dtos/management/log.dto';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { BreadcrumbService } from '../../../services/breadcrumb.service';
import { ComponentService } from '../../../services/component.service';

@Component({
  selector: 'app-log',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './log.component.html',
  styleUrl: './log.component.css'
})
export class LogComponent implements OnInit {
  constructor(
    private router: Router,
    private titleService: Title,
    private breadcrumbService: BreadcrumbService,
    private managementService: ManagementService,
    private componentService: ComponentService
  ) {}

  logs: LogDTO[] = [];
  isShowLogDetail = false;
  detail = "<trống>";

  ngOnInit(): void {
    this.titleService.setTitle("Quản lý log");
    this.breadcrumbService.addBreadcrumb("Quản lý log", this.router.url);
  }

  getLogs(value: string) {
    //value: yyyy-MM-dd convert to yyyyMMdd
    value = value.replaceAll('-', '');
    this.managementService.getLogsOfDate(value).subscribe(res => {
      this.componentService.$showLoadingStatus.next(false);
      this.logs = res;
    })
  }

  showDetail(value: string|null) {
    this.detail = value?.trim().replaceAll('\n', '<br> --> ') || "<trống>";
    this.isShowLogDetail = true;
  }
}
