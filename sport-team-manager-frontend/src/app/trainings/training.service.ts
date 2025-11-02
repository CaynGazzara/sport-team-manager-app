import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Training, CreateTraining, UpdateTraining, TrainingWithAttendances } from '../models/training';

@Injectable({
  providedIn: 'root'
})
export class TrainingService {
  private apiUrl = 'https://localhost:7279/api/trainings';

  constructor(private http: HttpClient) { }

  getTrainings(): Observable<Training[]> {
    return this.http.get<Training[]>(this.apiUrl);
  }

  getTraining(id: string): Observable<Training> {
    return this.http.get<Training>(`${this.apiUrl}/${id}`);
  }

  createTraining(training: CreateTraining): Observable<Training> {
    return this.http.post<Training>(this.apiUrl, training);
  }

  updateTraining(id: string, training: UpdateTraining): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, training);
  }

  deleteTraining(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getTrainingWithAttendances(id: string): Observable<TrainingWithAttendances> {
    return this.http.get<TrainingWithAttendances>(`${this.apiUrl}/${id}/attendances`);
  }

  getUpcomingTrainings(): Observable<Training[]> {
    return this.http.get<Training[]>(`${this.apiUrl}/upcoming`);
  }
}