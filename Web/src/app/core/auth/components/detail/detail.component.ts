import { Component, ElementRef, NgModule, ViewChild } from '@angular/core';
import { UserService } from '../../services/user.service';
import { UserDetail } from '../../models/user-detail.model';
import { MessageService } from 'src/app/shared/services/message.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule, NgModel } from '@angular/forms';
import { CommonModule, DatePipe } from '@angular/common';
import { LoadingComponent } from 'src/app/shared/components/loading/loading.component';

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.css'],
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    LoadingComponent
  ]
})
export class DetailComponent {
  loading = true;
  updating = false;
  data: UserDetail|null = null;
  editingData: UserDetail|null = null;
  userId: string;
  links: {
    name: string,
    url: string
  }[] = [];

  constructor(
    private userService: UserService,
    private messageService: MessageService,
    private router: ActivatedRoute,
    private route: Router
  ) {
    this.userId = router.snapshot.paramMap.get('id')!;
    userService.getDetail(this.userId)
      .subscribe({
        next: res => {
          this.loading = false;
          this.data = res.data!;
          this.reset();
        },
        error: err => messageService.showMessage({
          show: true,
          title: "Lỗi",
          msg: err.error.message || err.status,
          acts: [
            { label: "Đăng nhập lại", act: () => route.navigate(['user/login'])},
            { label: "Về trang chủ", act: () => route.navigate([''])}
          ]
        })
      });
  }

  reset() {
    this.updating = false;
    this.editingData =  {...this.data!} ;
    //chuyển sang yyyy-MM-dd
    this.editingData!.dob = this.editingData!.dob.split("T")[0];
    //convert link
    this.convertStringToLinks();
  }

  save() {
    this.messageService.showMessage({
      show: true,
      title: "Xác nhận",
      msg: "Bạn có muốn lưu những thay đổi này?",
      acts: [
        { label: "Lưu thay đổi", act: () => {
          this.loading = true;
          this.convertLinksToString();
          this.userService.updateDetail(this.userId, this.editingData!)
          .subscribe({
            next: res => {
              this.data = {...this.editingData!};
              this.loading = false;
              this.updating = false;
            },
            error: err => {
              this.messageService.showMessage({
                show: true,
                title: "Lỗi",
                msg: err.error.message || err.status,
                acts: []
              });
              this.loading = false;
            }
          });
        }}
      ]
    })
  }

  changeAvatar() {

  }

  convertStringToLinks() {
    this.links = [];
    this.editingData?.links.split('|').forEach(e => {
      this.links.push({
        name: e.split(',')[0],
        url: e.split(',')[1]
      });
    });
  }

  convertLinksToString() {
    this.editingData!.links = this.links.map(e => `${e.name},${e.url}`).join('|');
  }

  removeLink(item: any) {
    console.log('remove');
    this.links = this.links.filter(e => e.name !== item.name || e.url !== item.url);
  }

  addLink(name: string, url: string) {
    this.links.push({name, url});
  }
}
