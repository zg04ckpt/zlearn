import { Component } from '@angular/core';
import { ComponentService } from '../../services/component.service';

@Component({
  selector: 'app-message',
  standalone: true,
  imports: [],
  templateUrl: './message.component.html',
  styleUrl: './message.component.css'
})
export class MessageComponent {
  show: boolean = false;
  module: MessageModule = {
    message: "",
    isInfo: true,
    buttons: []
  };

  constructor(private componentService: ComponentService) {
    componentService.$showMessage.subscribe(next => {
      this.show = true;
      this.module = next;
    });
  }
}

export interface MessageModule {
  message: string;
  isInfo: boolean;
  buttons: {
    name: string,
    action: () => void
  }[];
}
