import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TeamService } from '../team.service';
import { PlayerService } from '../../players/player.service';
import { Team } from '../../models/team';
import { Player } from '../../models/player';

@Component({
  standalone: false,
  selector: 'app-team-players',
  templateUrl: './team-players.component.html',
  styleUrls: ['./team-players.component.scss']
})
export class TeamPlayersComponent implements OnInit {
  team: Team | null = null;
  players: Player[] = [];
  loading = true;
  error = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private teamService: TeamService,
    private playerService: PlayerService
  ) { }

  ngOnInit(): void {
    const teamId = this.route.snapshot.paramMap.get('id');
    if (teamId) {
      this.loadTeamAndPlayers(teamId);
    } else {
      this.error = 'ID do time não encontrado';
      this.loading = false;
    }
  }

  loadTeamAndPlayers(teamId: string): void {
    this.loading = true;
    
    // Carrega o time
    this.teamService.getTeam(teamId).subscribe({
      next: (team) => {
        this.team = team;
        this.loadPlayersByTeam(teamId);
      },
      error: (error) => {
        console.error('Error loading team:', error);
        this.error = 'Erro ao carregar time';
        this.loading = false;
      }
    });
  }

  loadPlayersByTeam(teamId: string): void {
    this.playerService.getPlayers().subscribe({
      next: (allPlayers) => {
        // Filtra jogadores pelo teamId
        this.players = allPlayers.filter(player => player.teamId === teamId);
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading players:', error);
        this.error = 'Erro ao carregar jogadores';
        this.loading = false;
      }
    });
  }

  getPositionBadgeClass(position: string): string {
    const positionClasses: { [key: string]: string } = {
      'Goalkeeper': 'bg-danger',
      'Defender': 'bg-primary',
      'Midfielder': 'bg-success',
      'Forward': 'bg-warning'
    };
    return positionClasses[position] || 'bg-secondary';
  }

  goBack(): void {
    this.router.navigate(['/teams']);
  }

  addPlayerToTeam(): void {
    if (this.team) {
      this.router.navigate(['/players/new'], { 
        queryParams: { teamId: this.team.id } 
      });
    }
  }

  editPlayer(playerId: string): void {
    this.router.navigate(['/players/edit', playerId]);
  }

  deletePlayer(playerId: string): void {
    if (confirm('Tem certeza que deseja remover este jogador do time?')) {
      this.playerService.deletePlayer(playerId).subscribe({
        next: () => {
          this.players = this.players.filter(p => p.id !== playerId);
        },
        error: (error) => {
          console.error('Error deleting player:', error);
          alert('Erro ao remover jogador');
        }
      });
    }
  }
}