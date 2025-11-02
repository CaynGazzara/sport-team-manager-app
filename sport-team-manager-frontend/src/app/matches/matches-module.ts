import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { MatchesRoutingModule } from './matches-routing-module';
import { MatchListComponent } from './match-list/match-list.component';
import { MatchComponent } from './match/match.component';

@NgModule({
  declarations: [
    MatchListComponent,
    MatchComponent
  ],
  imports: [
    CommonModule,
    MatchesRoutingModule,
    ReactiveFormsModule
  ]
})
export class MatchesModule { }