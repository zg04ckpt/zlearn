import { Component, OnInit } from '@angular/core';
import { UserDetail } from '../../entities/user/user-detail.entity';
import { UserService } from '../../services/user.service';
import { ComponentService } from '../../services/component.service';
import { UserInfoDTO } from '../../dtos/user/user-info.dto';
import { DatePipe } from '@angular/common';
import { UserInfo } from '../../entities/user/user-info.entity';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-user-info',
  standalone: true,
  imports: [
    DatePipe
  ],
  templateUrl: './user-info.component.html',
  styleUrl: './user-info.component.css'
})
export class UserInfoComponent {
  show: boolean = false;
  userInfo: UserInfo|null = null;
  isYourself: boolean = false;
  defaultImageUrl = environment.defaultAvtUrl

  constructor(
    private userService: UserService,
    private componentService: ComponentService
  ) {
    userService.$showInfo.subscribe(next => this.showInfo(next));
  }
  
  showInfo(userId: string) {
    this.userService.getUserProfile(userId).subscribe(next => {
      this.componentService.$showLoadingStatus.next(false);
      this.userInfo = next;

      this.isYourself = this.userService.getLoggedInUser()?.id == next.id;

      this.show = true;
    });
  }

  likeUser(userId: string) {
    this.userService.like(userId).subscribe(next => {
      this.componentService.$showLoadingStatus.next(false);
      this.userInfo!.isLiked = true;
      this.userInfo!.likes += 1;
    });
  }
}
