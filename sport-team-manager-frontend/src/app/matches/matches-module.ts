import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { MatchesRoutingModule } from './matches-routing-module';
import { MatchListComponent } from './match-list/match-list.component';
import { MatchComponent } from './match/match.component';
import { MatchDetailsComponent } from './match-details/match-details.component'; // ✅ Nova importação
    
@NgModule({
  declarations: [
    MatchListComponent,
    MatchComponent,
    MatchDetailsComponent
  ],
  imports: [
    CommonModule,
    MatchesRoutingModule,
    ReactiveFormsModule
  ]
})
export class MatchesModule { }