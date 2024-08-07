import { Routes } from '@angular/router';
import { HomeComponent } from '../pages/home/home.component';
import { ApiNotResponseComponent } from '../pages/errors/api-not-response/api-not-response.component';
import { EmailValidationComponent } from '../pages/email-validation/email-validation.component';
import { UserProfileComponent } from '../pages/user-profile/user-profile.component';

export const routes: Routes = [
    { path: "", component: HomeComponent },
    { path: "user/profile", component: UserProfileComponent },
    { path: "error/api-not-response", component: ApiNotResponseComponent},
    { path: "auth/email-confirm", component: EmailValidationComponent},
];
