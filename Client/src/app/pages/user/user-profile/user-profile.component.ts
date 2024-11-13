import { Component, NgModule, OnInit } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { StorageKey, StorageService } from '../../../services/storage.service';
import { User } from '../../../entities/user/user.entity';
import { ComponentService } from '../../../services/component.service';
import { UserDetail } from '../../../entities/user/user-detail.entity';
import { CommonModule } from '@angular/common';
import { FormsModule, NgModel } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';
import { BreadcrumbService } from '../../../services/breadcrumb.service';
import { Title } from '@angular/platform-browser';

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
  avtPreviewLink: string|null = null;
  title: string = "";

  constructor(
    private userService: UserService,
    private componentService: ComponentService,
    private authService: AuthService,
    private breadcrumbService: BreadcrumbService,
    private router: Router,
    private titleService: Title
  ) { }

  ngOnInit(): void {
    this.title = "Thông tin cá nhân";
    this.user = this.userService.getLoggedInUser();
    if(this.user != null) {
      this.userService.getProfile().subscribe(res => {
        debugger;
        this.loading = false;
        this.userDetail = res;
        this.avtPreviewLink = res.imageUrl;
        this.reset();
        this.componentService.$showLoadingStatus.next(false);
      });
    } else {
      this.componentService.$showLoadingStatus.next(false);
      this.authService.showLoginRequirement();
    }
    this.breadcrumbService.addBreadcrumb(this.title, this.router.url);
    this.titleService.setTitle(this.title);
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
        { name: "Xác nhận", action: async () => {

          this.componentService.$showLoadingStatus.next(true);
          await this.userService.updateProfile({
            firstName: this.editingData!.firstName,
            lastName: this.editingData!.lastName,
            email: this.editingData!.email,
            phoneNum: this.editingData!.phoneNum,
            gender: this.editingData!.gender,
            dayOfBirth: this.editingData!.dayOfBirth,
            address: this.editingData!.address,
            intro: this.editingData!.intro,
            socialLinks: this.convertLinksToString(),
            image: this.editingData!.image,
            imageUrl: this.editingData!.imageUrl
          });
          this.updating = false;
          this.componentService.$showLoadingStatus.next(false);
          this.userDetail = this.editingData;
          this.componentService.displayMessage("Lưu thay đổi thành công!");
        }}
      ]
    );
    
  }

  changeAvatar(event: any) {
    const image:File = event.target.files[0];
    if(image != null) {
      this.editingData!.image = image;
      const reader = new FileReader();
      reader.onload = e => this.avtPreviewLink = reader.result as string
      debugger
      reader.readAsDataURL(image);
    }
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
