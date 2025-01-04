import { HomeComponent } from '../pages/home/home.component';
import { UserProfileComponent } from '../pages/user/user-profile/user-profile.component';
import { OverviewComponent } from '../pages/management/overview/overview.component';
import { UsersListComponent } from '../pages/management/users-list/users-list.component';
import { RoleComponent } from '../pages/management/role/role.component';
import { TestManagementComponent } from '../pages/management/test-management/test-management.component';
import { ListTestComponent } from '../pages/test/list-test/list-test.component';
import { MyTestsComponent } from '../pages/test/my-tests/my-tests.component';
import { Notfound404Component } from '../pages/others/notfound404/notfound404.component';
import { canDeactivateGuard } from '../guards/can-deactivate.guard';
import { EmailValidationComponent } from '../pages/others/email-validation/email-validation.component';
import { TestDetailComponent } from '../pages/test/test-detail/test-detail.component';
import { TestComponent } from '../pages/test/test/test.component';
import { Routes } from '@angular/router';
import { ForgotPasswordComponent } from '../pages/others/forgot-password/forgot-password.component';
import { CateManagementComponent } from '../pages/management/cate-management/cate-management.component';
import { CreateTest2Component } from '../pages/test/create-test-2/create-test-2.component';
import { UpdateTest2Component } from '../pages/test/update-test-2/update-test-2.component';
import { LogComponent } from '../pages/management/log/log.component';
import { ListDocumentsComponent } from '../pages/document/list-document/list-documents.component';
import { CreateDocumentComponent } from '../pages/document/create-document/create-document.component';
import { MyDocumentComponent } from '../pages/document/my-document/my-document.component';
import { UpdateDocumentComponent } from '../pages/document/update-document/update-document.component';
import { DocManagementComponent } from '../pages/management/doc-management/doc-management.component';
import { NotiManagementComponent } from '../pages/management/noti-management/noti-management.component';
import { SavedTestsComponent } from '../pages/test/saved-tests/saved-tests.component';
import { TestHistoryComponent } from '../pages/test/test-history/test-history.component';

export const routes: Routes = [
    { path: "", component: HomeComponent, data: { breadcrumb: 'Trang chủ'}},
    
    
    { path: "management", component: OverviewComponent, data: { breadcrumb: 'Tổng quan' }},
    { path: "management/users", component: UsersListComponent, data: { breadcrumb: 'Quản lý người dùng' }},
    { path: "management/roles", component: RoleComponent, data: { breadcrumb: 'Quản lý quyền' }},
    { path: "management/tests", component: TestManagementComponent, data: { breadcrumb: 'Quản lý đề' }},
    { path: "management/categories", component: CateManagementComponent, data: { breadcrumb: 'Quản lý danh mục' }},
    { path: "management/logs", component: LogComponent, data: { breadcrumb: 'Quản lý log' }},
    { path: "management/documents", component: DocManagementComponent, data: { breadcrumb: 'Quản lý tài liệu' }},
    { path: "management/notification", component: NotiManagementComponent, data: { breadcrumb: 'Quản lý thông báo' }},

    { path: "tests", component: ListTestComponent, data: { breadcrumb: 'Trắc nghiệm' }},
    { path: "tests/my-tests", component: MyTestsComponent, data: { breadcrumb: 'Đề đã tạo' }},
    { path: "tests/saved", component: SavedTestsComponent, data: { breadcrumb: 'Đề đã lưu' }},
    { path: "tests/history", component: TestHistoryComponent, data: { breadcrumb: 'Đề đã làm' }},
    { path: "tests/create", component: CreateTest2Component, canDeactivate: [canDeactivateGuard], data: { breadcrumb: 'Tạo đề mới' }},
    { path: "tests/update/:id", component: UpdateTest2Component, canDeactivate: [canDeactivateGuard], data: { breadcrumb: 'Cập nhật đề' }},
    { path: "tests/:id", component: TestDetailComponent, data: { breadcrumb: 'Chi tiết' }},
    { path: "tests/:id/:option", component: TestComponent, canDeactivate: [canDeactivateGuard], data: { breadcrumb: 'Làm đề' }},

    { path: "documents", component: ListDocumentsComponent, data: { breadcrumb: 'Tài liệu' }},
    { path: "documents/create", component: CreateDocumentComponent, data: { breadcrumb: 'Tạo tài liệu mới' }},
    { path: "documents/my-documents", component: MyDocumentComponent, data: { breadcrumb: 'Tài liệu đã tải lên' }},
    { path: "documents/:id/update", component: UpdateDocumentComponent, data: { breadcrumb: 'Cập nhật tài liệu' }},

    { path: "user/profile", component: UserProfileComponent, data: { breadcrumb: 'Thông tin cá nhân' }},
    { path: "auth/email-confirm", component: EmailValidationComponent, data: { breadcrumb: 'Xác thực email' }},
    { path: "auth/reset-password", component: ForgotPasswordComponent, data: { breadcrumb: 'Quên mật khẩu' }},

    { path: "**", component: Notfound404Component }
];