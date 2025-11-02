import { Component, OnInit } from '@angular/core';
import { Player } from '../../models/player';
import { PlayerService } from '../player.service';

@Component({
  standalone: false,
  selector: 'app-player-list',
  templateUrl: './player-list.component.html',
  styleUrls: ['./player-list.component.scss']
})
export class PlayerListComponent implements OnInit {
  players: Player[] = [];
  loading = true;
  error = '';

  constructor(private playerService: PlayerService) { }

  ngOnInit(): void {
    this.loadPlayers();
  }

  loadPlayers(): void {
    this.loading = true;
    this.playerService.getPlayers().subscribe({
      next: (players) => {
        this.players = players;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Erro ao carregar jogadores';
        this.loading = false;
        console.error('Error loading players:', error);
      }
    });
  }

  deletePlayer(id: string): void {
    if (confirm('Tem certeza que deseja excluir este jogador?')) {
      this.playerService.deletePlayer(id).subscribe({
        next: () => {
          this.players = this.players.filter(p => p.id !== id);
        },
        error: (error) => {
          console.error('Error deleting player:', error);
          alert('Erro ao excluir jogador');
        }
      });
    }
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
}