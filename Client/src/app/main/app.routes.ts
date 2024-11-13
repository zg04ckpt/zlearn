import { HomeComponent } from '../pages/home/home.component';
import { UserProfileComponent } from '../pages/user/user-profile/user-profile.component';
import { OverviewComponent } from '../pages/management/overview/overview.component';
import { UsersListComponent } from '../pages/management/users-list/users-list.component';
import { RoleComponent } from '../pages/management/role/role.component';
import { TestManagementComponent } from '../pages/management/test-management/test-management.component';
import { ListTestComponent } from '../pages/test/list-test/list-test.component';
import { CreateTestComponent } from '../pages/test/create-test/create-test.component';
import { UpdateTestComponent } from '../pages/test/update-test/update-test.component';
import { MyTestsComponent } from '../pages/test/my-tests/my-tests.component';
import { Notfound404Component } from '../pages/others/notfound404/notfound404.component';
import { canDeactivateGuard } from '../guards/can-deactivate.guard';
import { EmailValidationComponent } from '../pages/others/email-validation/email-validation.component';
import { TestDetailComponent } from '../pages/test/test-detail/test-detail.component';
import { TestComponent } from '../pages/test/test/test.component';
import { Routes } from '@angular/router';

export const routes: Routes = [
    { path: "", component: HomeComponent, data: { breadcrumb: 'Trang chủ'}},
    
    
    { path: "management", component: OverviewComponent, data: { breadcrumb: 'Tổng quan' }},
    { path: "management/users", component: UsersListComponent, data: { breadcrumb: 'Quản lý người dùng' }},
    { path: "management/roles", component: RoleComponent, data: { breadcrumb: 'Quản lý quyền' }},
    { path: "management/tests", component: TestManagementComponent, data: { breadcrumb: 'Quản lý đề' }},

    { path: "tests", component: ListTestComponent, data: { breadcrumb: 'Trắc nghiệm' }},
    { path: "tests/my-tests", component: MyTestsComponent, data: { breadcrumb: 'Quản lý đề' }},
    { path: "tests/create", component: CreateTestComponent, canDeactivate: [canDeactivateGuard], data: { breadcrumb: 'Tạo đề mới' }},
    { path: "tests/update/:id", component: UpdateTestComponent, canDeactivate: [canDeactivateGuard], data: { breadcrumb: 'Cập nhật đề' }},
    { path: "tests/:id", component: TestDetailComponent, data: { breadcrumb: 'Chi tiết' }},
    { path: "tests/:id/:option", component: TestComponent, canDeactivate: [canDeactivateGuard], data: { breadcrumb: 'Làm đề' }},

    { path: "user/profile", component: UserProfileComponent, data: { breadcrumb: 'Thông tin cá nhân' }},
    { path: "auth/email-confirm", component: EmailValidationComponent, data: { breadcrumb: 'Xác thực email' }},

    { path: "**", component: Notfound404Component }
];