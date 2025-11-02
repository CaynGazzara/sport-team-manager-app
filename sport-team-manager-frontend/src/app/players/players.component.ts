import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PlayerService } from '../players/player.service';
import { Player, CreatePlayer, UpdatePlayer } from '../../app/models/player';

@Component({
  standalone: false,
  selector: 'app-player',
  templateUrl: './players.component.html',
  styleUrls: ['./players.component.scss']
})
export class PlayerComponent implements OnInit {
  playerForm: FormGroup;
  isEditMode = false;
  playerId: string | null = null;
  loading = false;
  submitting = false;

  positions = [
    'Goalkeeper',
    'Defender', 
    'Midfielder',
    'Forward'
  ];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private playerService: PlayerService
  ) {
    this.playerForm = this.createForm();
  }

  ngOnInit(): void {
    this.playerId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.playerId;

    if (this.isEditMode && this.playerId) {
      this.loadPlayer(this.playerId);
    }
  }

  createForm(): FormGroup {
    return this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      age: ['', [Validators.required, Validators.min(16), Validators.max(50)]],
      position: ['', Validators.required],
      jerseyNumber: ['', [Validators.required, Validators.pattern('^[0-9]+$')]],
      birthDate: ['', Validators.required],
      height: ['', [Validators.required, Validators.min(1.50), Validators.max(2.20)]],
      weight: ['', [Validators.required, Validators.min(50), Validators.max(120)]],
      nationality: ['', Validators.required],
      joinDate: ['', Validators.required],
      isActive: [true]
    });
  }

  loadPlayer(id: string): void {
    this.loading = true;
    this.playerService.getPlayer(id).subscribe({
      next: (player) => {
        this.playerForm.patchValue({
          name: player.name,
          age: player.age,
          position: player.position,
          jerseyNumber: player.jerseyNumber,
          birthDate: this.formatDateForInput(player.birthDate),
          height: player.height,
          weight: player.weight,
          nationality: player.nationality,
          joinDate: this.formatDateForInput(player.joinDate),
          isActive: player.isActive
        });
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading player:', error);
        this.loading = false;
        alert('Erro ao carregar jogador');
      }
    });
  }

  onSubmit(): void {
    if (this.playerForm.valid) {
      this.submitting = true;
      
      const formValue = this.playerForm.value;
      const playerData = {
        ...formValue,
        birthDate: new Date(formValue.birthDate).toISOString(),
        joinDate: new Date(formValue.joinDate).toISOString()
      };

      if (this.isEditMode && this.playerId) {
        const updateData: UpdatePlayer = {
          name: playerData.name,
          position: playerData.position,
          jerseyNumber: playerData.jerseyNumber,
          height: playerData.height,
          weight: playerData.weight,
          isActive: playerData.isActive
        };

        this.playerService.updatePlayer(this.playerId, updateData).subscribe({
          next: () => {
            this.submitting = false;
            this.router.navigate(['/players']);
          },
          error: (error) => {
            console.error('Error updating player:', error);
            this.submitting = false;
            alert('Erro ao atualizar jogador');
          }
        });
      } else {
        const createData: CreatePlayer = {
          name: playerData.name,
          age: playerData.age,
          position: playerData.position,
          jerseyNumber: playerData.jerseyNumber,
          birthDate: playerData.birthDate,
          height: playerData.height,
          weight: playerData.weight,
          nationality: playerData.nationality,
          joinDate: playerData.joinDate
        };

        this.playerService.createPlayer(createData).subscribe({
          next: () => {
            this.submitting = false;
            this.router.navigate(['/players']);
          },
          error: (error) => {
            console.error('Error creating player:', error);
            this.submitting = false;
            alert('Erro ao criar jogador');
          }
        });
      }
    } else {
      this.markFormGroupTouched();
    }
  }

  private markFormGroupTouched(): void {
    Object.keys(this.playerForm.controls).forEach(key => {
      this.playerForm.get(key)?.markAsTouched();
    });
  }

  private formatDateForInput(dateString: string): string {
    const date = new Date(dateString);
    return date.toISOString().split('T')[0];
  }

  cancel(): void {
    this.router.navigate(['/players']);
  }

  get title(): string {
    return this.isEditMode ? 'Editar Jogador' : 'Novo Jogador';
  }
}