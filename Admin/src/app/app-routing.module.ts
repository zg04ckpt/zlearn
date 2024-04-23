import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { QuestionSetsComponent } from './components/question-sets/question-sets.component';
import { QuestionSetsCreateComponent } from './components/question-sets-create/question-sets-create.component';

const routes: Routes = [
  { path: "question-sets", component: QuestionSetsComponent },
  { path: "question-sets-create", component: QuestionSetsCreateComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
