import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TestListComponent } from './components/test-list/test-list.component';
import { TestComponent } from './components/test/test.component';
import { AboutComponent } from './components/about/about.component';
import { PracticeComponent } from './components/practice/practice.component';
import { LoginComponent } from './components/login/login.component';

const routes: Routes = [
  { path: '', component: TestListComponent },
  { path: 'test/:id', component: TestComponent },
  { path: 'practice/:id', component: PracticeComponent },
  { path: 'about', component: AboutComponent },
  { path: 'login', component: LoginComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
