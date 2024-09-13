import { Routes } from '@angular/router';
import { HomeComponent } from '../pages/home/home.component';
import { EmailValidationComponent } from '../pages/others/email-validation/email-validation.component';
import { UserProfileComponent } from '../pages/user/user-profile/user-profile.component';
import { OverviewComponent } from '../pages/management/overview/overview.component';
import { UsersListComponent } from '../pages/management/users-list/users-list.component';
import { RoleComponent } from '../pages/management/role/role.component';
import { ListTestComponent } from '../pages/test/list-test/list-test.component';
import { TestDetailComponent } from '../pages/test/test-detail/test-detail.component';
import { TestComponent } from '../pages/test/test/test.component';
import { CreateTestComponent } from '../pages/test/create-test/create-test.component';
import { MyTestsComponent } from '../pages/test/my-tests/my-tests.component';
import { Notfound404Component } from '../pages/others/notfound404/notfound404.component';
import { Forbidden403Component } from '../pages/others/forbidden403/forbidden403.component';
import { ServiceUnavailable503Component } from '../pages/others/service-unavailable503/service-unavailable503.component';
import { canDeactivateGuard } from '../guards/can-deactivate.guard';
import { UpdateTestComponent } from '../pages/test/update-test/update-test.component';
export const routes: Routes = [
    { path: "", component: HomeComponent },
    { path: "user/profile", component: UserProfileComponent },
    { path: "auth/email-confirm", component: EmailValidationComponent},
    { path: "management" , component: OverviewComponent},
    { path: "management/users", component: UsersListComponent },
    { path: "management/roles", component: RoleComponent },
    { path: "tests", component: ListTestComponent },
    { path: "tests/create", component: CreateTestComponent, canDeactivate: [canDeactivateGuard] },
    { path: "tests/update/:id", component: UpdateTestComponent, canDeactivate: [canDeactivateGuard] },
    { path: "tests/my-tests", component: MyTestsComponent },
    { path: "tests/:id", component: TestDetailComponent },
    { path: "tests/:id/:option", component: TestComponent, canDeactivate: [canDeactivateGuard] },
    { path: "**", component: Notfound404Component}
];
