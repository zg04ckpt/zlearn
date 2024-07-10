import { Component, input, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from '../../services/message.service';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css'],
  standalone: true,
  imports: [
    NgIf
  ]
})
export class MessageComponent implements OnInit {
  
  data: MessageManageModule = {
    show: false,
    title: "Tiêu đề",
    msg: "Thông báo",
    acts: []
  }

  constructor(
    private route: Router,
    private msgService: MessageService
  ) { }


  ngOnInit(): void {
    this.msgService.displayStatus.subscribe(data => this.data = data);
  }

  redirect(url: string) {
    this.route.navigate([url]);
  }
}

export interface MessageManageModule {
  show: boolean
  title: string,
  msg: string,
  acts: {
    label: string,
    url: string
  }[]
}
