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
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { BreadcrumbService } from '../../../services/breadcrumb.service';

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
  title: string = "Quản lý người dùng";

  searchKeys = {
    lastName: "",
    firstName: "",
    username: "",
    email: "",
    createdDate: ""
  }
  isSearching: boolean = false;

  constructor(
    private managementService: ManagementService,
    private componentService: ComponentService,
    private router: Router,
    private titleService: Title,
    private breadcrumbService: BreadcrumbService
  ) { }

  ngOnInit(): void {
    this.titleService.setTitle(this.title);
    this.get(this.pageIndex);

    //load default role
    this.managementService.getAllRoles().subscribe({
      next: res => {
        res.data!.forEach(r => this.defaultRoles.push({
          name: r.name,
          selected: false
        }));
        this.componentService.$showLoadingStatus.next(false);
      }
    });
  }

  switchPage(change: number) {
    this.pageIndex += change;
    this.get(this.pageIndex);
  }

  cancelSearch() {
    this.searchKeys = {
      lastName: "",
      firstName: "",
      username: "",
      email: "",
      createdDate: ""
    }
    this.get(1);
  }

  get(i: number) {
    this.pageIndex = i;
    this.managementService.getAllUsers(this.pageIndex, this.pageSize, this.searchKeys)
    .subscribe({
      next: res => {
        this.users = res.data;
        this.totalUsers = res.total;
        this.pagination = [];
        const totalPages = Math.ceil(this.totalUsers / this.pageSize);
        for(let i=1; i<=totalPages; i++) this.pagination.push(i);
        this.componentService.$showLoadingStatus.next(false);
      }
    });
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
        this.managementService.updateUser(this.selectedUser!)
        .subscribe({
          next: res => {
            this.componentService.displayMessage("Cập nhật thành công!");
            this.componentService.$showLoadingStatus.next(false);
            this.updating = false;
            this.get(this.pageIndex);
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
        this.componentService.$showLoadingStatus.next(false);
        this.showAssignRole = true;
      }
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
        }
      })
    );
    
  }

  deleteUser(id: string) {
    this.componentService.displayConfirmMessage("Xác nhận xóa user?", () => {
      this.managementService.deleteUser(id)
      .subscribe({
        next: res => {
          this.componentService.displayMessage("Xóa thành công!");
          this.componentService.$showLoadingStatus.next(false);
          this.get(this.pageIndex);
        }
      });
    });
  }
}
