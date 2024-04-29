import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListQuestionSetComponent } from './components/list-question-set/list-question-set.component';

const routes: Routes = [
  { path: '', component: ListQuestionSetComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
