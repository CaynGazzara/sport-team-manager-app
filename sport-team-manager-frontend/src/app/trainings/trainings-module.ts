import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { TrainingsRoutingModule } from './trainings-routing-module';
import { TrainingListComponent } from './training-list/training-list.component';
import { TrainingComponent } from './training/training.component';

@NgModule({
  declarations: [
    TrainingListComponent,
    TrainingComponent
  ],
  imports: [
    CommonModule,
    TrainingsRoutingModule,
    ReactiveFormsModule
  ]
})
export class TrainingsModule { }