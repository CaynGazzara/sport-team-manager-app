import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TeamService } from '../team.service';
import { Team, CreateTeam, UpdateTeam } from '../../models/team';

@Component({
  standalone: false,
  selector: 'app-team',
  templateUrl: './team.component.html',
  styleUrls: ['./team.component.scss']
})
export class TeamComponent implements OnInit {
  teamForm: FormGroup;
  isEditMode = false;
  teamId: string | null = null;
  loading = false;
  submitting = false;

  sportTypes = [
    'Football',
    'Basketball',
    'Volleyball',
    'Handball',
    'Rugby',
    'Hockey'
  ];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private teamService: TeamService
  ) {
    this.teamForm = this.createForm();
  }

  ngOnInit(): void {
    this.teamId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.teamId;

    if (this.isEditMode && this.teamId) {
      this.loadTeam(this.teamId);
    }
  }

  createForm(): FormGroup {
    return this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      sportType: ['Football', Validators.required],
      coachName: ['', [Validators.required, Validators.minLength(2)]],
      homeField: ['', Validators.required],
      foundedDate: ['', Validators.required],
      colors: ['', Validators.required]
    });
  }

  loadTeam(id: string): void {
    this.loading = true;
    this.teamService.getTeam(id).subscribe({
      next: (team) => {
        this.teamForm.patchValue({
          name: team.name,
          sportType: team.sportType,
          coachName: team.coachName,
          homeField: team.homeField,
          foundedDate: this.formatDateForInput(team.foundedDate),
          colors: team.colors
        });
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading team:', error);
        this.loading = false;
        alert('Erro ao carregar time');
      }
    });
  }

  onSubmit(): void {
    if (this.teamForm.valid) {
      this.submitting = true;
      
      const formValue = this.teamForm.value;
      const teamData = {
        ...formValue,
        foundedDate: new Date(formValue.foundedDate).toISOString()
      };

      if (this.isEditMode && this.teamId) {
        const updateData: UpdateTeam = {
          name: teamData.name,
          coachName: teamData.coachName,
          homeField: teamData.homeField,
          colors: teamData.colors
        };

        this.teamService.updateTeam(this.teamId, updateData).subscribe({
          next: () => {
            this.submitting = false;
            this.router.navigate(['/teams']);
          },
          error: (error) => {
            console.error('Error updating team:', error);
            this.submitting = false;
            alert('Erro ao atualizar time');
          }
        });
      } else {
        const createData: CreateTeam = {
          name: teamData.name,
          sportType: teamData.sportType,
          coachName: teamData.coachName,
          homeField: teamData.homeField,
          foundedDate: teamData.foundedDate,
          colors: teamData.colors
        };

        this.teamService.createTeam(createData).subscribe({
          next: () => {
            this.submitting = false;
            this.router.navigate(['/teams']);
          },
          error: (error) => {
            console.error('Error creating team:', error);
            this.submitting = false;
            alert('Erro ao criar time');
          }
        });
      }
    } else {
      this.markFormGroupTouched();
    }
  }

  private markFormGroupTouched(): void {
    Object.keys(this.teamForm.controls).forEach(key => {
      this.teamForm.get(key)?.markAsTouched();
    });
  }

  private formatDateForInput(dateString: string): string {
    const date = new Date(dateString);
    return date.toISOString().split('T')[0];
  }

  cancel(): void {
    this.router.navigate(['/teams']);
  }

  get title(): string {
    return this.isEditMode ? 'Editar Time' : 'Novo Time';
  }
}