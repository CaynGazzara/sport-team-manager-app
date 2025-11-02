import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatchService } from '../match.service';
import { TeamService } from '../../teams/team.service';
import { Match, CreateMatch, UpdateMatch } from '../../models/match';
import { Team } from '../../models/team';

@Component({
  standalone: false,
  selector: 'app-match',
  templateUrl: './match.component.html',
  styleUrls: ['./match.component.scss']
})
export class MatchComponent implements OnInit {
  matchForm: FormGroup;
  isEditMode = false;
  matchId: string | null = null;
  loading = false;
  submitting = false;
  teams: Team[] = []; // ✅ Todos os times
  opponentTeams: Team[] = []; // ✅ Times disponíveis como adversários

  matchTypes = [
    'Friendly',
    'League',
    'Cup',
    'Championship',
    'Playoff'
  ];

  results = [
    'Scheduled',
    'Win',
    'Loss',
    'Draw',
    'Postponed',
    'Cancelled'
  ];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private matchService: MatchService,
    private teamService: TeamService
  ) {
    this.matchForm = this.createForm();
  }

  ngOnInit(): void {
    this.matchId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.matchId;

    this.loadTeams();

    if (this.isEditMode && this.matchId) {
      this.loadMatch(this.matchId);
    }

    // ✅ Observa mudanças no campo teamId para atualizar adversários
    this.matchForm.get('teamId')?.valueChanges.subscribe(selectedTeamId => {
      this.updateOpponentTeams(selectedTeamId);
    });
  }

  // ✅ Método para atualizar a lista de adversários
  updateOpponentTeams(selectedTeamId: string): void {
    if (selectedTeamId) {
      // Filtra todos os times, removendo o time selecionado
      this.opponentTeams = this.teams.filter(team => team.id !== selectedTeamId);
      
      // Se estiver editando e o adversário atual for o time selecionado, limpa o campo
      const currentOpponent = this.matchForm.get('opponent')?.value;
      if (currentOpponent === selectedTeamId) {
        this.matchForm.patchValue({ opponent: '' });
      }
    } else {
      this.opponentTeams = [...this.teams]; // Mostra todos os times se nenhum time for selecionado
    }
  }

  loadTeams(): void {
    this.teamService.getTeams().subscribe({
      next: (teams) => {
        this.teams = teams;
        this.opponentTeams = [...teams]; // Inicialmente mostra todos como adversários
        
        if (!this.isEditMode && teams.length > 0) {
          const firstTeamId = teams[0].id;
          this.matchForm.patchValue({
            teamId: firstTeamId
          });
          // ✅ Já atualiza os adversários baseado no time selecionado
          this.updateOpponentTeams(firstTeamId);
        }
      },
      error: (error) => {
        console.error('Error loading teams:', error);
        alert('Erro ao carregar times');
      }
    });
  }

  createForm(): FormGroup {
    return this.fb.group({
      date: ['', Validators.required],
      opponent: ['', Validators.required], // ✅ Agora é select, não input text
      location: ['', Validators.required],
      matchType: ['Friendly', Validators.required],
      teamScore: [null],
      opponentScore: [null],
      result: ['Scheduled', Validators.required],
      notes: [''],
      teamId: ['', Validators.required]
    });
  }

  loadMatch(id: string): void {
    this.loading = true;
    this.matchService.getMatch(id).subscribe({
      next: (match) => {
        this.matchForm.patchValue({
          date: this.formatDateForInput(match.date),
          opponent: match.opponent,
          location: match.location,
          matchType: match.matchType,
          teamScore: match.teamScore,
          opponentScore: match.opponentScore,
          result: match.result,
          notes: match.notes,
          teamId: match.teamId
        });
        this.loading = false;
        
        // ✅ Atualiza adversários baseado no teamId carregado
        this.updateOpponentTeams(match.teamId);
      },
      error: (error) => {
        console.error('Error loading match:', error);
        this.loading = false;
        alert('Erro ao carregar partida');
      }
    });
  }

  onSubmit(): void {
    if (this.matchForm.valid) {
      this.submitting = true;
      
      const formValue = this.matchForm.value;
      
      // ✅ Busca o nome do time adversário baseado no ID selecionado
      const opponentTeam = this.teams.find(team => team.id === formValue.opponent);
      const opponentName = opponentTeam ? opponentTeam.name : 'Time Desconhecido';

      const matchData = {
        ...formValue,
        date: new Date(formValue.date).toISOString(),
        opponent: opponentName // ✅ Usa o nome do time, não o ID
      };

      if (this.isEditMode && this.matchId) {
        const updateData: UpdateMatch = {
          date: matchData.date,
          opponent: matchData.opponent,
          location: matchData.location,
          matchType: matchData.matchType,
          teamScore: matchData.teamScore,
          opponentScore: matchData.opponentScore,
          result: matchData.result,
          notes: matchData.notes
        };

        this.matchService.updateMatch(this.matchId, updateData).subscribe({
          next: () => {
            this.submitting = false;
            this.router.navigate(['/matches']);
          },
          error: (error) => {
            console.error('Error updating match:', error);
            this.submitting = false;
            alert('Erro ao atualizar partida');
          }
        });
      } else {
        const createData: CreateMatch = {
          date: matchData.date,
          opponent: matchData.opponent,
          location: matchData.location,
          matchType: matchData.matchType,
          teamScore: matchData.teamScore,
          opponentScore: matchData.opponentScore,
          notes: matchData.notes,
          teamId: matchData.teamId
        };

        this.matchService.createMatch(createData).subscribe({
          next: () => {
            this.submitting = false;
            this.router.navigate(['/matches']);
          },
          error: (error) => {
            console.error('Error creating match:', error);
            this.submitting = false;
            alert('Erro ao criar partida');
          }
        });
      }
    } else {
      this.markFormGroupTouched();
    }
  }

  private markFormGroupTouched(): void {
    Object.keys(this.matchForm.controls).forEach(key => {
      this.matchForm.get(key)?.markAsTouched();
    });
  }

  private formatDateForInput(dateString: string): string {
    const date = new Date(dateString);
    return date.toISOString().split('T')[0];
  }

  cancel(): void {
    this.router.navigate(['/matches']);
  }

  get title(): string {
    return this.isEditMode ? 'Editar Partida' : 'Nova Partida';
  }

  get showScoreFields(): boolean {
    const result = this.matchForm.get('result')?.value;
    return result !== 'Scheduled' && result !== 'Postponed' && result !== 'Cancelled';
  }

  // ✅ Método para obter o nome do time selecionado
  getSelectedTeamName(): string {
    const teamId = this.matchForm.get('teamId')?.value;
    const team = this.teams.find(t => t.id === teamId);
    return team ? team.name : 'Time não selecionado';
  }

  // ✅ Método para obter o nome do adversário selecionado
  getSelectedOpponentName(): string {
    const opponentId = this.matchForm.get('opponent')?.value;
    const opponent = this.teams.find(t => t.id === opponentId);
    return opponent ? opponent.name : 'Adversário não selecionado';
  }
}