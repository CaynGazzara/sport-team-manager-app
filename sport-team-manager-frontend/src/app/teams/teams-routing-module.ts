import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TeamListComponent } from './team-list/team-list.component';
import { TeamComponent } from './team/team.component';
import { TeamPlayersComponent } from './team-players/team-players.component'; 

const routes: Routes = [
  { path: '', component: TeamListComponent },
  { path: 'new', component: TeamComponent },
  { path: 'edit/:id', component: TeamComponent },
  { path: ':id/players', component: TeamPlayersComponent } // ✅ Nova rota
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TeamsRoutingModule { }