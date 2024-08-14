import { Routes } from '@angular/router';
import { HomeComponent } from '../pages/home/home.component';
import { ApiNotResponseComponent } from '../pages/errors/api-not-response/api-not-response.component';
import { EmailValidationComponent } from '../pages/email-validation/email-validation.component';
import { UserProfileComponent } from '../pages/user-profile/user-profile.component';
import { OverviewComponent } from '../pages/management/overview/overview.component';
import { UsersListComponent } from '../pages/management/users-list/users-list.component';
import { RoleComponent } from '../pages/management/role/role.component';

export const routes: Routes = [
    { path: "", component: HomeComponent },
    { path: "user/profile", component: UserProfileComponent },
    { path: "error/api-not-response", component: ApiNotResponseComponent},
    { path: "auth/email-confirm", component: EmailValidationComponent},
    { path: "management" , component: OverviewComponent},
    { path: "management/users", component: UsersListComponent },
    { path: "management/roles", component: RoleComponent },
];
