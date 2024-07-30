import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './core/auth/components/login/login.component';
import { AppComponent } from './app.component';
import { RegisterComponent } from './core/auth/components/register/register.component';
import { EmailConfirmComponent } from './core/auth/components/email-confirm/email-confirm.component';
import { DetailComponent } from './core/auth/components/detail/detail.component';

const routes: Routes = [
  { path: "user/register", component: RegisterComponent },
  { path: "user/login", component: LoginComponent },
  { path: "user/email-confirm", component: EmailConfirmComponent},
  { path: "user/detail/:id", component: DetailComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
