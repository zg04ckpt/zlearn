import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { BreadcrumbService } from '../../../services/breadcrumb.service';
import { FormsModule } from '@angular/forms';
import { ManagementService } from '../../../services/management.service';
import { ComponentService } from '../../../services/component.service';
import { Summary } from '../../../entities/management/summary.entity';
import { LogDTO } from '../../../dtos/management/log.dto';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../../../environments/environment';


@Component({
  selector: 'app-overview',
  standalone: true,
  imports: [
    FormsModule,
  ],
  templateUrl: './overview.component.html',
  styleUrl: './overview.component.css'
})
export class OverviewComponent implements OnInit {
  title: string = "Quản lý";
  isCustom = false;
  filter = {
    start: "",
    end: ""
  }
  summary: Summary = {
    accessCount: 0,
    testCompletionCount: 0,
    commentCount: 0,
    userCount: 0
  };

  logs: LogDTO[] = [];
  logHubConnection = new signalR.HubConnectionBuilder()
    .withUrl(environment.baseUrl + '/hubs/log').build();
  maxLogLines = 10;

  constructor(
    private router: Router,
    private titleService: Title,
    private breadcrumbService: BreadcrumbService,
    private managementService: ManagementService,
    private componentService: ComponentService
  ) {}

  ngOnInit(): void {
    this.titleService.setTitle(this.title);
    this.breadcrumbService.addBreadcrumb(this.title, this.router.url);
    this.getToday();

    //log
    this.logHubConnection.start().then(() => {
      this.managementService.listenSystemLog(this.logHubConnection.connectionId!).subscribe(res => {
        this.componentService.$showLoadingStatus.next(false);
        this.logHubConnection.on('onHasLog', data => {
          this.logs.splice(0, 0, {time: data.time, type: data.type, desc: data.desc, detail: null});
          if(this.logs.length > this.maxLogLines) {
            this.logs.pop();
          }
        });
      });
    });
  }

  public getToday() {
    this.managementService.getOverviewToday().subscribe(next => {
      this.componentService.$showLoadingStatus.next(false);
      this.summary = next;
    });
  }

  public getRange() {
    this.managementService.getOverviewByRange(this.filter.start, this.filter.end).subscribe(next => {
      this.componentService.$showLoadingStatus.next(false);
      this.summary = next;
    });
  }
}


