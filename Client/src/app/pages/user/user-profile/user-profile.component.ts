import { Component, NgModule, OnInit } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { StorageKey, StorageService } from '../../../services/storage.service';
import { User } from '../../../entities/user/user.entity';
import { ComponentService } from '../../../services/component.service';
import { UserDetail } from '../../../entities/user/user-detail.entity';
import { CommonModule } from '@angular/common';
import { FormsModule, NgModel } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-user-detail',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent implements OnInit {
  loading: boolean = true;
  user: User|null = null;
  userDetail: UserDetail|null = null;
  editingData: UserDetail|null = null;
  updating: boolean = false;
  constructor(
    private userService: UserService,
    private componentService: ComponentService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    debugger;
    this.componentService.$showLoadingStatus.next(true);
    this.user = this.userService.getLoggedInUser();
    if(this.user != null)
    {
      this.userService.getProfile(this.user.id).subscribe({

        next: res => {
          debugger;
          this.loading = false;
          this.userDetail = res;
          this.reset();
          this.componentService.$showLoadingStatus.next(false);
        },


        error: res => {
          debugger;
          this.componentService.displayMessage(res.error?.message || res.statusText);
          this.componentService.$showLoadingStatus.next(false);
        }
      });
    }
    else
    {
      this.componentService.$showLoadingStatus.next(false);
      this.authService.showLoginRequirement();
    }
  }

  reset() {
    this.updating = false;
    this.editingData! = {...this.userDetail!};
  }

  save() {
    this.componentService.displayMessageWithActions(
      "Xác nhận lưu thay đổi?",
      [
        { name: "Hủy", action: () => {} },
        { name: "Xác nhận", action: () => {

          this.componentService.$showLoadingStatus.next(true);
          this.userService.updateProfile(this.user!.id, {
            firstName: this.editingData!.firstName,
            lastName: this.editingData!.lastName,
            email: this.editingData!.email,
            phoneNum: this.editingData!.phoneNum,
            gender: this.editingData!.gender,
            dayOfBirth: this.editingData!.dayOfBirth,
            address: this.editingData!.address,
            intro: this.editingData!.intro,
            socialLinks: this.convertLinksToString()
          }).subscribe({
            next: res => { 
              this.updating = false;
              this.componentService.$showLoadingStatus.next(false);
              this.userDetail = this.editingData;
              this.componentService.displayMessage("Lưu thay đổi thành công!");
            },

            error: res => {
              this.componentService.$showLoadingStatus.next(false);
              this.componentService.displayMessage(`Lỗi: ${res.error?.message || res.statusText}`);
            }
          });
        }}
      ]
    );
    
  }

  changeAvatar() {

  }

  convertLinksToString(): string {
    return this.editingData!.socialLinks.map(e => `${e.name},${e.url}`).join('|');
  }

  removeLink(item: any) {
    this.editingData!.socialLinks = this.editingData!.socialLinks.filter(e => e.name !== item.name || e.url !== item.url);
  }

  addLink(name: string, url: string) {
    this.editingData!.socialLinks.push({name, url});
  }
}
