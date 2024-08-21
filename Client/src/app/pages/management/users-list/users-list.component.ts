import { Component, OnInit } from '@angular/core';
import { UserManagement } from '../../../entities/management/user-management.entity';
import { ManagementService } from '../../../services/management.service';
import { ComponentService } from '../../../services/component.service';
import { CommonModule, NgClass } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AssignRoleDTO } from '../../../dtos/management/assign-role.dto';
import { Role } from '../../../entities/management/role.entity';
import { User } from '../../../entities/user/user.entity';
import { use } from 'marked';

@Component({
  selector: 'app-users-list',
  standalone: true,
  imports: [
    CommonModule,
    NgClass,
    FormsModule
  ],
  templateUrl: './users-list.component.html',
  styleUrl: './users-list.component.css'
})
export class UsersListComponent implements OnInit {
  users: UserManagement[] = [];
  currentIndex: number = 0;
  selectedUser: UserManagement|null = null;
  pageIndex: number = 1;
  pageSize: number = 3;
  totalUsers: number = 0;
  pagination: number[] = [];
  showUserDetail: boolean = false;
  updating: boolean = false;
  showAssignRole: boolean = false;
  defaultRoles: {
    name: string;
    selected: boolean;
  }[] = [];
  isFiltering: boolean = false;

  constructor(
    private managementService: ManagementService,
    private componentService: ComponentService,
  ) { }

  ngOnInit(): void {
    this.get(this.pageIndex);

    //load default role
    this.managementService.getAllRoles().subscribe({
      next: res => {
        res.forEach(r => this.defaultRoles.push({
          name: r.name,
          selected: false
        }));
      },
      error: res => this.componentService.displayMessage("Lỗi tải quyền mặc định")
    });
  }

  switchPage(change: number) {
    this.pageIndex += change;
    this.get(this.pageIndex);
  }

  get(i: number) {
    this.pageIndex = i;
    this.managementService.getAllUsers(this.pageIndex, this.pageSize)
    .subscribe({
      next: res => {
        this.users = res.data;
        this.totalUsers = res.total;

        this.pagination = [];
        const totalPages = Math.ceil(this.totalUsers / this.pageSize);
        for(let i=1; i<=totalPages; i++) this.pagination.push(i);
      },

      error: res => this.componentService.displayAPIError(res)
    });
  }

  getByKey(url: string, key: string) {
    if(key == "") return;
    this.pageIndex = 1;
    this.managementService.getUsersByKey(url, key, this.pageIndex, this.pageSize).subscribe({
      next: res => {
        this.users = res.data;
        this.totalUsers = res.total;
        debugger
        this.pagination = [];
        const totalPages = Math.ceil(this.totalUsers / this.pageSize);
        for(let i=1; i<=totalPages; i++) this.pagination.push(i);
      },

      error: res => this.componentService.displayAPIError(res)
    })
  }

  selectUser(user: UserManagement) {
    this.selectedUser = {... user};
  }

  resetDetail() {
    this.selectedUser = {... this.users.find(e => e.id == this.selectedUser!.id)!};
    this.updating = false;
  }

  saveDetail() {
    this.componentService.displayConfirmMessage(
      "Xác nhận cập nhật user?",
      () => {
        this.componentService.$showLoadingStatus.next(true);
        this.managementService.updateUser(this.selectedUser!)
        .subscribe({
          next: res => {
            this.componentService.displayMessage("Cập nhật thành công!");
            this.componentService.$showLoadingStatus.next(false);
            this.updating = false;
            this.get(this.pageIndex);
          },

          error: res => {
            this.componentService.displayAPIError(res);
            this.componentService.$showLoadingStatus.next(false);
            this.updating = false;
          }
        });
      }
    );
  }

  getUserRoles(user: UserManagement) {
    this.selectUser(user);
    this.managementService.getAllUserRoles(user.id).subscribe({
      next: res => {

        //set if role is selected
        const map: Map<string, boolean> = new Map;
        res.forEach(name => map.set(name, true));
        this.defaultRoles.forEach(e => {
          if(map.has(e.name)) 
            e.selected = true;
          else 
            e.selected = false;
        })

        this.showAssignRole = true;
      },
      error: res => this.componentService.displayMessage("Lỗi tải quyền của user")
    });
  }

  assignRole() {
    const data: AssignRoleDTO = { roles: this.defaultRoles };
    this.componentService.displayConfirmMessage(
      "Xác nhận lưu?", 
      () => this.managementService.assignRole(this.selectedUser!.id, data).subscribe({
        next: res => {
          this.componentService.$showToast.next("Lưu quyền thành công");
          this.showAssignRole = false; 
          this.get(this.pageIndex);
        },
        error: res => this.componentService.displayAPIError(res)
      })
    );
    
  }

  deleteUser(id: string) {
    this.componentService.displayConfirmMessage("Xác nhận xóa user?", () => {
      this.componentService.$showLoadingStatus.next(true);
      this.managementService.deleteUser(id)
      .subscribe({
        next: res => {
          this.componentService.displayMessage("Xóa thành công!");
          this.componentService.$showLoadingStatus.next(false);
          this.get(this.pageIndex);
        },

        error: res => {
          this.componentService.displayAPIError(res);
          this.componentService.$showLoadingStatus.next(false);
        }
      });
    });
  }
}
