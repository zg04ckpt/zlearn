import { BehaviorSubject } from 'rxjs';
import { Injectable } from '@angular/core';
import { MessageManageModule } from '../components/message/message.component';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private displaySrc = new BehaviorSubject<MessageManageModule>({
    show: false,
    title: "Tiêu đề",
    msg: "Thông báo",
    acts: []
  });
  public displayStatus = this.displaySrc.asObservable();

  constructor() { }

  public showMessage(data: MessageManageModule): void {
    this.displaySrc.next(data);
  }
}
