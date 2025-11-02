import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { PlayersRoutingModule } from './players-routing-module';
import { PlayerListComponent } from './player-list/player-list.component';
import { PlayerComponent } from './players.component';


@NgModule({
  declarations: [
    PlayerListComponent,
    PlayerComponent
  ],
  imports: [
    CommonModule,
    PlayersRoutingModule,
    ReactiveFormsModule
  ]
})
export class PlayersModule { }