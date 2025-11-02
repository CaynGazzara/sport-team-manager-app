import { Component, OnInit } from '@angular/core';
import { Team } from '../../models/team';
import { TeamService } from '../team.service';

@Component({
  standalone: false,
  selector: 'app-team-list',
  templateUrl: './team-list.component.html',
  styleUrls: ['./team-list.component.scss']
})
export class TeamListComponent implements OnInit {
  teams: Team[] = [];
  loading = true;
  error = '';

  constructor(private teamService: TeamService) { }

  ngOnInit(): void {
    this.loadTeams();
  }

  loadTeams(): void {
    this.loading = true;
    this.teamService.getTeams().subscribe({
      next: (teams) => {
        this.teams = teams;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Erro ao carregar times';
        this.loading = false;
        console.error('Error loading teams:', error);
      }
    });
  }

  deleteTeam(id: string): void {
    if (confirm('Tem certeza que deseja excluir este time?')) {
      this.teamService.deleteTeam(id).subscribe({
        next: () => {
          this.teams = this.teams.filter(t => t.id !== id);
        },
        error: (error) => {
          console.error('Error deleting team:', error);
          alert('Erro ao excluir time');
        }
      });
    }
  }

  getSportTypeBadgeClass(sportType: string): string {
    const sportClasses: { [key: string]: string } = {
      'Football': 'bg-success',
      'Basketball': 'bg-primary',
      'Volleyball': 'bg-warning',
      'Handball': 'bg-info'
    };
    return sportClasses[sportType] || 'bg-secondary';
  }

  getFoundedYears(foundedDate: string): number {
    const founded = new Date(foundedDate);
    const now = new Date();
    return now.getFullYear() - founded.getFullYear();
  }
}