import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { QuestionSetsComponent } from './components/question-sets/question-sets.component';
import { QuestionSetsCreateComponent } from './components/question-sets-create/question-sets-create.component';
import { QuestionSetsUpdateComponent } from './components/question-sets-update/question-sets-update.component';

const routes: Routes = [
  { path: "question-sets", component: QuestionSetsComponent },
  { path: "question-sets-create", component: QuestionSetsCreateComponent },
  { path: "question-sets-update/:id", component: QuestionSetsUpdateComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
