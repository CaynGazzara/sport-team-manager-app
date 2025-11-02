import { Component, OnInit } from '@angular/core';
import { Training } from '../../models/training';
import { TrainingService } from '../training.service';

@Component({
  standalone: false,
  selector: 'app-training-list',
  templateUrl: './training-list.component.html',
  styleUrls: ['./training-list.component.scss']
})
export class TrainingListComponent implements OnInit {
  trainings: Training[] = [];
  loading = true;
  error = '';

  constructor(private trainingService: TrainingService) { }

  ngOnInit(): void {
    this.loadTrainings();
  }

  loadTrainings(): void {
    this.loading = true;
    this.trainingService.getTrainings().subscribe({
      next: (trainings) => {
        this.trainings = trainings;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Erro ao carregar treinos';
        this.loading = false;
        console.error('Error loading trainings:', error);
      }
    });
  }

  deleteTraining(id: string): void {
    if (confirm('Tem certeza que deseja excluir este treino?')) {
      this.trainingService.deleteTraining(id).subscribe({
        next: () => {
          this.trainings = this.trainings.filter(t => t.id !== id);
        },
        error: (error) => {
          console.error('Error deleting training:', error);
          alert('Erro ao excluir treino');
        }
      });
    }
  }

  getFocusBadgeClass(focus: string): string {
    const focusClasses: { [key: string]: string } = {
      'Technical': 'bg-primary',
      'Tactical': 'bg-success',
      'Physical': 'bg-warning',
      'Strategic': 'bg-info',
      'Recovery': 'bg-secondary'
    };
    return focusClasses[focus] || 'bg-dark';
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('pt-BR');
  }

  formatTime(duration: string): string {
    // Converte "02:00:00" para "2h"
    const [hours, minutes, seconds] = duration.split(':');
    const hoursNum = parseInt(hours);
    const minutesNum = parseInt(minutes);

    if (minutesNum > 0) {
      return `${hoursNum}h${minutesNum}min`;
    }
    return `${hoursNum}h`;
  }
}