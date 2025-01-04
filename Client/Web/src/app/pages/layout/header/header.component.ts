import { DatePipe, NgClass } from "@angular/common";
import { Component, HostListener, OnInit } from "@angular/core";
import { Router, RouterLink } from "@angular/router";
import { User } from "../../../entities/user/user.entity";
import { Breadcrumb, BreadcrumbService } from "../../../services/breadcrumb.service";
import { environment } from "../../../../environments/environment";
import { UserService } from "../../../services/user.service";
import { AuthService } from "../../../services/auth.service";
import { ComponentService } from "../../../services/component.service";
import { NotificationService } from "../../../services/notification.service";
import { Notification } from "../../../entities/notification/notification.dto";
import { ShareFunction } from "../../../utilities/share-function.uti";
import * as SignalR from "@microsoft/signalr"
import { LayoutService } from "../../../services/layout.service";
import { StorageKey, StorageService } from "../../../services/storage.service";


@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    RouterLink,
    NgClass,
    DatePipe
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent implements OnInit {
  user: User|null = null;
  defaultAvtUrl = environment.defaultAvtUrl;
  currentTime = new Date();

  isShowNotification = false;
  notifications: Notification[] = [];
  start = 0;
  newNotificationsCount = 0;
  detailIndex = -1;
  notificationHubConnection: SignalR.HubConnection
  baseUrl = environment.baseUrl
  maxOfNotification = 20

  uti = new ShareFunction()

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private componentService: ComponentService,
    private router: Router,
    private breadcrumbService: BreadcrumbService,
    private notificationService: NotificationService,
    private layoutService: LayoutService,
    private storageService: StorageService
  ) {
    userService.$currentUser.subscribe(next => this.user = next);
    setInterval(() => this.currentTime = new Date(), 1000);

    this.notificationHubConnection = new SignalR
      .HubConnectionBuilder()
      .configureLogging(SignalR.LogLevel.None)
      .withUrl(this.baseUrl + '/hubs/notification', {
        accessTokenFactory: () => storageService.get(StorageKey.accessToken) || ''
      })
      .build();
    layoutService.$isLoggedIn.subscribe(() => this.getNotifications());
      //add listen for notification
    this.notificationHubConnection.start().then(() => {
      if(this.user) {
        this.notificationService.listenForUser(this.notificationHubConnection.connectionId!).subscribe(res => {
          this.componentService.$showLoadingStatus.next(false);
        });
      }
      //get notifications
      this.getNotifications();
      this.notificationHubConnection.on('onHasNewNotification', (data:Notification) => {
        data.createdAt = new Date(data.createdAt)
        this.notifications.splice(0, 0, data);
        this.componentService.$showToast.next(`1 thông báo mới!`);
      });
    }).catch(err => console.error('SignalR connection error: ', err));

    
  }

  ngOnInit(): void {
    //add close notification event
    window.onclick = (e: MouseEvent) => {
      const target = e.target as HTMLElement;
      const notificationElement = document.getElementById('noti');
      const bellIcon = target.closest('.label[title="Thông báo"]');

      if (notificationElement && !notificationElement.contains(target) && !bellIcon) {
        this.isShowNotification = false;
      }
    };
  }

  getNotifications() {
    this.notificationService.getNotifications(this.start, this.maxOfNotification).subscribe(res => {
      this.componentService.$showLoadingStatus.next(false);
      this.notifications = res;
      this.start = 0;
      this.newNotificationsCount = 0;
      this.notifications.forEach(e => {
        if(!e.isRead && this.user) {
          this.newNotificationsCount++;
        }
      });
      if(this.user && this.newNotificationsCount > 0) {
        this.componentService.$showToast.next(`Bạn có ${this.newNotificationsCount} thông báo mới!`)
      }
    });
  }

  showMoreNotifications() {
    this.start = this.notifications.length;
    this.notificationService.getNotifications(this.start, this.maxOfNotification).subscribe(res => {
      this.componentService.$showLoadingStatus.next(false);
      res.forEach(e => {
        if(!e.isRead) {
          this.newNotificationsCount++;
        }
      });

      this.notifications = this.notifications.concat(res);
    });
  }

  readNotification(idx: number) {
    // If logged in => mark this notification is read
    if(this.user && !this.notifications[idx].isRead) {
      this.notificationHubConnection.invoke('OnReadNotification', this.notifications[idx].id);
    }

    this.detailIndex = idx; 
    this.notifications[idx].isRead = true; 
    this.newNotificationsCount = this.newNotificationsCount-1; 
    this.isShowNotification = false;
  }

  showLogin() {
    this.componentService.$showLoginDialog.next(true);
    this.componentService.$showRegisterDialog.next(false);
  }

  showRegister() {
    this.componentService.$showRegisterDialog.next(true);
    this.componentService.$showLoginDialog.next(false);
  }

  logout() {
    this.authService.logout().subscribe({
      next: res => {
        this.getNotifications();
        this.componentService.$showToast.next("Đã đăng xuất");
        this.authService.purgeAuth();
        this.componentService.$showLoadingStatus.next(false);
        this.router.navigateByUrl("");
      }
    });
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: Event) {
    // console.log('Screen width:', window.innerWidth);
    // Thực hiện các hành động khác khi kích thước màn hình thay đổi
  }
}
