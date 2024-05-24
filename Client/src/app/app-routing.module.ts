import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListQuestionSetComponent } from './components/list-question-set/list-question-set.component';
import { TestComponent } from './components/test/test.component';
import { AboutComponent } from './components/about/about.component';
import { PraticeComponent } from './components/pratice/pratice.component';

const routes: Routes = [
  { path: '', component: ListQuestionSetComponent },
  { path: 'test/:id', component: TestComponent },
  { path: 'practice/:id', component: PraticeComponent },
  { path: 'about', component: AboutComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
