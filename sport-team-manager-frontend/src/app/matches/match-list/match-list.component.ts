import { Component, OnInit } from '@angular/core';
import { Match } from '../../models/match';
import { MatchService } from '../match.service';

@Component({
  standalone: false,
  selector: 'app-match-list',
  templateUrl: './match-list.component.html',
  styleUrls: ['./match-list.component.scss']
})
export class MatchListComponent implements OnInit {
  matches: Match[] = [];
  loading = true;
  error = '';

  constructor(private matchService: MatchService) { }

  ngOnInit(): void {
    this.loadMatches();
  }

  loadMatches(): void {
    this.loading = true;
    this.matchService.getMatches().subscribe({
      next: (matches) => {
        this.matches = matches;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Erro ao carregar partidas';
        this.loading = false;
        console.error('Error loading matches:', error);
      }
    });
  }

  deleteMatch(id: string): void {
    if (confirm('Tem certeza que deseja excluir esta partida?')) {
      this.matchService.deleteMatch(id).subscribe({
        next: () => {
          this.matches = this.matches.filter(m => m.id !== id);
        },
        error: (error) => {
          console.error('Error deleting match:', error);
          alert('Erro ao excluir partida');
        }
      });
    }
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

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('pt-BR');
  }

  formatScore(match: Match): string {
    if (match.teamScore !== null && match.opponentScore !== null) {
      return `${match.teamScore} - ${match.opponentScore}`;
    }
    return 'A definir';
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
}