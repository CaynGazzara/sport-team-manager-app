import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatchService } from '../match.service';
import { TeamService } from '../../teams/team.service';
import { Match, MatchWithPlayers } from '../../models/match';
import { Team } from '../../models/team';

@Component({
  standalone: false,
  selector: 'app-match-details',
  templateUrl: './match-details.component.html',
  styleUrls: ['./match-details.component.scss']
})
export class MatchDetailsComponent implements OnInit {
  match: Match | null = null;
  matchWithPlayers: MatchWithPlayers | null = null;
  team: Team | null = null;
  loading = true;
  error = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private matchService: MatchService,
    private teamService: TeamService
  ) { }

  ngOnInit(): void {
    const matchId = this.route.snapshot.paramMap.get('id');
    if (matchId) {
      this.loadMatchDetails(matchId);
    } else {
      this.error = 'ID da partida não encontrado';
      this.loading = false;
    }
  }

  loadMatchDetails(matchId: string): void {
    this.loading = true;
    
    // Tenta carregar a partida com jogadores primeiro
    this.matchService.getMatchWithPlayers(matchId).subscribe({
      next: (matchWithPlayers) => {
        this.matchWithPlayers = matchWithPlayers;
        this.match = matchWithPlayers; // ✅ CORRIGIDO: matchWithPlayers JÁ É uma Match
        this.loadTeamInfo(matchWithPlayers.teamId);
      },
      error: (error) => {
        console.error('Error loading match with players:', error);
        // Se falhar, carrega apenas a partida básica
        this.loadBasicMatch(matchId);
      }
    });
  }

  loadBasicMatch(matchId: string): void {
    this.matchService.getMatch(matchId).subscribe({
      next: (match) => {
        this.match = match;
        this.loadTeamInfo(match.teamId);
      },
      error: (error) => {
        console.error('Error loading match:', error);
        this.error = 'Erro ao carregar partida';
        this.loading = false;
      }
    });
  }

  loadTeamInfo(teamId: string): void {
    this.teamService.getTeam(teamId).subscribe({
      next: (team) => {
        this.team = team;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading team:', error);
        this.loading = false;
        // Continua mesmo sem informações do time
      }
    });
  }

  getResultBadgeClass(result: string): string {
    const resultClasses: { [key: string]: string } = {
      'Win': 'bg-success',
      'Loss': 'bg-danger',
      'Draw': 'bg-warning',
      'Scheduled': 'bg-info',
      'Postponed': 'bg-secondary',
      'Cancelled': 'bg-dark'
    };
    return resultClasses[result] || 'bg-secondary';
  }

  getMatchTypeBadgeClass(matchType: string): string {
    const typeClasses: { [key: string]: string } = {
      'Friendly': 'bg-primary',
      'League': 'bg-success',
      'Cup': 'bg-warning',
      'Championship': 'bg-info',
      'Playoff': 'bg-danger'
    };
    return typeClasses[matchType] || 'bg-secondary';
  }

  getResultText(result: string): string {
    const resultTexts: { [key: string]: string } = {
      'Win': 'Vitória',
      'Loss': 'Derrota',
      'Draw': 'Empate',
      'Scheduled': 'Agendada',
      'Postponed': 'Adiada',
      'Cancelled': 'Cancelada'
    };
    return resultTexts[result] || result;
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('pt-BR', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  formatScore(): string {
    if (this.match && this.match.teamScore !== null && this.match.opponentScore !== null) {
      return `${this.match.teamScore} - ${this.match.opponentScore}`;
    }
    return 'A definir';
  }

  goBack(): void {
    this.router.navigate(['/matches']);
  }

  editMatch(): void {
    if (this.match) {
      this.router.navigate(['/matches/edit', this.match.id]);
    }
  }

  getTeamName(): string {
    return this.team ? this.team.name : 'Nosso Time';
  }

  hasLineup(): boolean {
    return this.matchWithPlayers !== null && 
           this.matchWithPlayers.matchPlayers.length > 0;
  }

  getStarters(): any[] {
    return this.matchWithPlayers ? 
      this.matchWithPlayers.matchPlayers.filter(mp => mp.isStarter) : [];
  }

  getSubstitutes(): any[] {
    return this.matchWithPlayers ? 
      this.matchWithPlayers.matchPlayers.filter(mp => !mp.isStarter) : [];
  }

  // ✅ NOVO: Método para obter o nome do jogador de forma segura
  getPlayerName(matchPlayer: any): string {
    return matchPlayer.player?.name || matchPlayer.playerName || 'Jogador';
  }

  // ✅ NOVO: Método para obter a inicial do jogador
  getPlayerInitial(matchPlayer: any): string {
    const name = this.getPlayerName(matchPlayer);
    return name.charAt(0).toUpperCase();
  }
}