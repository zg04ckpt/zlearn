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
export const routes: Routes = [
    { path: "", component: HomeComponent },
    { path: "user/profile", component: UserProfileComponent },
    { path: "auth/email-confirm", component: EmailValidationComponent},
    { path: "management" , component: OverviewComponent},
    { path: "management/users", component: UsersListComponent },
    { path: "management/roles", component: RoleComponent },
    { path: "tests", component: ListTestComponent },
    { path: "tests/create", component: CreateTestComponent },
    { path: "tests/:id", component: TestDetailComponent },
    { path: "tests/:id/:option", component: TestComponent },
];
