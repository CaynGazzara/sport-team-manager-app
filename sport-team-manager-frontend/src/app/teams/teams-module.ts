import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { TeamsRoutingModule } from './teams-routing-module';
import { TeamListComponent } from './team-list/team-list.component';
import { TeamComponent } from './team/team.component';
import { TeamPlayersComponent } from './team-players/team-players.component'; 


@NgModule({
  declarations: [
    TeamListComponent,
    TeamComponent,
    TeamPlayersComponent
  ],
  imports: [
    CommonModule,
    TeamsRoutingModule,
    ReactiveFormsModule
  ]
})
export class TeamsModule { }