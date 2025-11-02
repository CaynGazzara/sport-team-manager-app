import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PlayerListComponent } from './player-list/player-list.component';
import { PlayerComponent } from './players.component';

const routes: Routes = [
  { path: '', component: PlayerListComponent },
  { path: 'new', component: PlayerComponent },
  { path: 'edit/:id', component: PlayerComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PlayersRoutingModule { }