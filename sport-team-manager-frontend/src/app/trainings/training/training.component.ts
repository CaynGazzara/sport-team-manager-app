import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TrainingService } from '../training.service';
import { TeamService } from '../../teams/team.service'; // ✅ Importar TeamService
import { Training, CreateTraining, UpdateTraining } from '../../models/training';
import { Team } from '../../models/team'; // ✅ Importar Team

@Component({
  standalone: false,
  selector: 'app-training',
  templateUrl: './training.component.html',
  styleUrls: ['./training.component.scss']
})
export class TrainingComponent implements OnInit {
  trainingForm: FormGroup;
  isEditMode = false;
  trainingId: string | null = null;
  loading = false;
  submitting = false;
  teams: Team[] = []; // ✅ Array para armazenar os times

  focusTypes = [
    'Technical',
    'Tactical', 
    'Physical',
    'Strategic',
    'Recovery'
  ];

  durationOptions = [
    { value: '01:00:00', label: '1 hora' },
    { value: '01:30:00', label: '1 hora e 30 minutos' },
    { value: '02:00:00', label: '2 horas' },
    { value: '02:30:00', label: '2 horas e 30 minutos' },
    { value: '03:00:00', label: '3 horas' }
  ];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private trainingService: TrainingService,
    private teamService: TeamService // ✅ Injeta TeamService
  ) {
    this.trainingForm = this.createForm();
  }

  ngOnInit(): void {
    this.trainingId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.trainingId;

    // ✅ Carrega os times do banco
    this.loadTeams();

    if (this.isEditMode && this.trainingId) {
      this.loadTraining(this.trainingId);
    }
  }

  // ✅ Método para carregar times
  loadTeams(): void {
    this.teamService.getTeams().subscribe({
      next: (teams) => {
        this.teams = teams;
        
        // Se não estiver editando e houver times, seleciona o primeiro
        if (!this.isEditMode && teams.length > 0) {
          this.trainingForm.patchValue({
            teamId: teams[0].id
          });
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
      duration: ['02:00:00', Validators.required],
      location: ['', Validators.required],
      focus: ['Technical', Validators.required],
      notes: [''],
      teamId: ['', Validators.required] // ✅ Agora é obrigatório
    });
  }

  loadTraining(id: string): void {
    this.loading = true;
    this.trainingService.getTraining(id).subscribe({
      next: (training) => {
        this.trainingForm.patchValue({
          date: this.formatDateForInput(training.date),
          duration: training.duration,
          location: training.location,
          focus: training.focus,
          notes: training.notes,
          teamId: training.teamId // ✅ Usa o teamId real do banco
        });
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading training:', error);
        this.loading = false;
        alert('Erro ao carregar treino');
      }
    });
  }

  onSubmit(): void {
    if (this.trainingForm.valid) {
      this.submitting = true;
      
      const formValue = this.trainingForm.value;
      
      const trainingData = {
        ...formValue,
        date: new Date(formValue.date).toISOString()
        // ✅ teamId já vem do formulário (selecionado pelo usuário)
      };

      if (this.isEditMode && this.trainingId) {
        const updateData: UpdateTraining = {
          date: trainingData.date,
          duration: trainingData.duration,
          location: trainingData.location,
          focus: trainingData.focus,
          notes: trainingData.notes
        };

        this.trainingService.updateTraining(this.trainingId, updateData).subscribe({
          next: () => {
            this.submitting = false;
            this.router.navigate(['/trainings']);
          },
          error: (error) => {
            console.error('Error updating training:', error);
            this.submitting = false;
            alert('Erro ao atualizar treino');
          }
        });
      } else {
        const createData: CreateTraining = {
          date: trainingData.date,
          duration: trainingData.duration,
          location: trainingData.location,
          focus: trainingData.focus,
          notes: trainingData.notes,
          teamId: trainingData.teamId // ✅ TeamId correto do banco
        };

        this.trainingService.createTraining(createData).subscribe({
          next: () => {
            this.submitting = false;
            this.router.navigate(['/trainings']);
          },
          error: (error) => {
            console.error('Error creating training:', error);
            this.submitting = false;
            alert('Erro ao criar treino');
          }
        });
      }
    } else {
      this.markFormGroupTouched();
    }
  }

  private markFormGroupTouched(): void {
    Object.keys(this.trainingForm.controls).forEach(key => {
      this.trainingForm.get(key)?.markAsTouched();
    });
  }

  private formatDateForInput(dateString: string): string {
    const date = new Date(dateString);
    return date.toISOString().split('T')[0];
  }

  cancel(): void {
    this.router.navigate(['/trainings']);
  }

  get title(): string {
    return this.isEditMode ? 'Editar Treino' : 'Novo Treino';
  }
}