import { Component, OnInit } from '@angular/core';
import { Role } from '../../../entities/management/role.entity';
import { NgClass } from '@angular/common';
import { ManagementService } from '../../../services/management.service';
import { ComponentService } from '../../../services/component.service';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { BreadcrumbService } from '../../../services/breadcrumb.service';

@Component({
  selector: 'app-role',
  standalone: true,
  imports: [
    NgClass,
    FormsModule
  ],
  templateUrl: './role.component.html',
  styleUrl: './role.component.css'
})
export class RoleComponent implements OnInit {
  roles: Role[] = [];
  selectedRole: Role|null = null;
  title: string = "Quản lý quyền";
  constructor(
    private managementService: ManagementService,
    private componentService: ComponentService,
    private router: Router,
    private titleService: Title,
    private breadcrumbService: BreadcrumbService
  ) {}

  ngOnInit(): void {
    this.load();
    this.titleService.setTitle(this.title);
  }

  load() {
    this.managementService.getAllRoles().subscribe({
      next: res => this.roles = res.data!,
      complete: () => this.componentService.$showLoadingStatus.next(false)
    });
  }

  selectRole(role: Role) {
    this.selectedRole = {... role};
  }

  addRole(name: string, desc: string) {
    this.componentService.displayConfirmMessage(
      "Xác nhận tạo quyền mới?",
      () => {
        this.managementService.createRole(name, desc).subscribe({
          next: res => {
            this.componentService.displayMessage("Thêm thành công!");
            this.load();
            this.componentService.$showLoadingStatus.next(false);
          }
        })
      }
    )
  }

  deleteRole(id: string) {
    this.componentService.displayConfirmMessage(
      "Xác nhận xóa quyền này?",
      () => {
        this.componentService.$showLoadingStatus.next(true);
        this.managementService.deleteRole(id).subscribe({
          next: res => {
            this.componentService.displayMessage("Xóa thành công!");
            this.load();
            this.componentService.$showLoadingStatus.next(false);
          }
        })
      }
    )
  }

  updateRole() {
    this.componentService.displayConfirmMessage(
      "Xác nhận cập nhật quyền này?",
      () => {
        this.componentService.$showLoadingStatus.next(true);
        this.managementService.updateRole(this.selectedRole!).subscribe({
          next: res => {
            this.componentService.displayMessage("Cập nhật thành công!");
            this.load();
            this.componentService.$showLoadingStatus.next(false);
            this.selectedRole = null;
          }
        })
      }
    )
  }
}
