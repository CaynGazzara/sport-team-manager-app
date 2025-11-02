import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MatchListComponent } from './match-list/match-list.component';
import { MatchComponent } from './match/match.component';
import { MatchDetailsComponent } from './match-details/match-details.component'; // ✅ Nova importação

const routes: Routes = [
  { path: '', component: MatchListComponent },
  { path: 'new', component: MatchComponent },
  { path: 'edit/:id', component: MatchComponent },
  { path: ':id/details', component: MatchDetailsComponent } // ✅ Nova rota
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MatchesRoutingModule { }