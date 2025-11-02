import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TrainingListComponent } from './training-list/training-list.component';
import { TrainingComponent } from './training/training.component';

const routes: Routes = [
  { path: '', component: TrainingListComponent },
  { path: 'new', component: TrainingComponent },
  { path: 'edit/:id', component: TrainingComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TrainingsRoutingModule { }