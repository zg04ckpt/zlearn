import { Component, OnInit } from '@angular/core';
import { CreateNotificationDTO } from '../../../dtos/notification/create-notification.dto';
import { Notification, NotificationType } from '../../../entities/notification/notification.dto';
import { FormsModule } from '@angular/forms';
import { NotificationService } from '../../../services/notification.service';
import { ComponentService } from '../../../services/component.service';
import { Title } from '@angular/platform-browser';
import { BreadcrumbService } from '../../../services/breadcrumb.service';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { ManagementService } from '../../../services/management.service';
import { ShareFunction } from '../../../utilities/share-function.uti';
import { FindDataDTO } from '../../../dtos/user/find-data.dto';
import { debounceTime, Subject } from 'rxjs';

@Component({
  selector: 'app-noti-management',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './noti-management.component.html',
  styleUrl: './noti-management.component.css'
})
export class NotiManagementComponent implements OnInit {
  createData: CreateNotificationDTO = {
    title: '',
    message: '',
    type: NotificationType.System,
    userId: null
  }
  updateDialog = {
    id: 0,
    title: '',
    message: '',
    isShow: false
  }
  notifications: Notification[] = []
  paging = {
    page: 1,
    size: 20,
    total: 1,
    next: () => {
      if(this.paging.page < this.paging.total) {
        this.paging.page++;
        this.getAllNotifications();
      }
    },
    prev: () => {
      if(this.paging.page > 1) {
        this.paging.page--;
        this.getAllNotifications();
      }
    },
    end: () => {
      this.paging.page = this.paging.total
      this.getAllNotifications();
    },
    start: () => {
      this.paging.page = 1
      this.getAllNotifications();
    },
  }

  searchUserInput = {
    focus: false,
    listener: new Subject<string>,
    onblur: () => setTimeout(() => this.searchUserInput.focus = false, 200)
  }
  userFindDataSrc: FindDataDTO[] = []
  userFindData: FindDataDTO[] = []

  uti = new ShareFunction()

  constructor(
    private notificationService: NotificationService,
    private componentService: ComponentService,
    private userService: UserService,
    private titleService: Title,
    private breadcrumbService: BreadcrumbService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private managementService: ManagementService
  ) {}
  ngOnInit(): void {
    this.titleService.setTitle("Quản lý thông báo");
    this.getAllNotifications();

    //Get data for find user data
    this.managementService.getUserFindData().subscribe(res => {
      this.componentService.$showLoadingStatus.next(false);
      this.userFindDataSrc = res;
      this.userFindData = res;
    });
    this.searchUserInput.listener
      .pipe(debounceTime(500))
      .subscribe(val => {
        this.userFindData = this.userFindDataSrc.filter(e => e.userName.startsWith(val) || e.id.startsWith(val))
      });
  }

  showUserInfo(notificationId: number) {
    this.managementService.getUserIdOfNotification(notificationId).subscribe(res => {
      this.userService.$showInfo.next(res);
    });
  }

  showUpdateDialog(data: Notification) {
    this.updateDialog.id = data.id;
    this.updateDialog.title = data.title;
    this.updateDialog.message = data.message;
    this.updateDialog.isShow = true;
  }

  createNewNotification() {
    if(!this.createData.title) {
      this.componentService.displayError("Tiêu đề thông báo trống!", []);
      return;
    }
    if(!this.createData.message) {
      this.componentService.displayError("Nội dung thông báo trống!", []);
      return;
    }
    console.log(this.createData);
    
    this.componentService.displayConfirmMessage("Xác nhận tạo thông báo ?", () => {
      this.managementService.createNewNotification(this.createData).subscribe(res => {
        this.componentService.$showLoadingStatus.next(false);
        this.componentService.displayMessage("Tạo thông báo thành công");
        this.getAllNotifications()
      });
    });
  }

  getAllNotifications() {
    this.managementService.getNotifications(this.paging.page, this.paging.size).subscribe(res => {
      this.componentService.$showLoadingStatus.next(false);
      this.notifications = res.data;
      this.paging.total = Math.ceil(res.total / this.paging.size);
    });
  }

  deleteNotification(id: number) {
    this.componentService.displayConfirmMessage("Xác nhận xóa thông báo ?", () => {
      this.managementService.deleteNotification(id).subscribe(res => {
        this.componentService.$showLoadingStatus.next(false);
        this.componentService.displayMessage("Xóa thông báo thành công");
        this.getAllNotifications();
      });
    });
  }

  updateNotification() {
    if(!this.updateDialog.title) {
      this.componentService.displayError("Tiêu đề thông báo trống!", []);
      return;
    }
    if(!this.updateDialog.message) {
      this.componentService.displayError("Nội dung thông báo trống!", []);
      return;
    }
    this.componentService.displayConfirmMessage("Xác nhận sửa thông báo ?", () => {
      this.managementService.updateNotification(this.updateDialog.id, this.updateDialog.title, this.updateDialog.message).subscribe(res => {
        this.componentService.$showLoadingStatus.next(false);
        this.componentService.displayMessage("Cập nhật thông báo thành công");
        this.getAllNotifications();
        this.updateDialog.isShow = false;
      });
    });
  }
}
