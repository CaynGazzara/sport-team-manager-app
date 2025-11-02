import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Team, CreateTeam, UpdateTeam, TeamWithPlayers } from '../models/team';

@Injectable({
  providedIn: 'root'
})
export class TeamService {
  private apiUrl = 'https://localhost:7279/api/teams';

  constructor(private http: HttpClient) { }

  getTeams(): Observable<Team[]> {
    return this.http.get<Team[]>(this.apiUrl);
  }

  getTeam(id: string): Observable<Team> {
    return this.http.get<Team>(`${this.apiUrl}/${id}`);
  }

  createTeam(team: CreateTeam): Observable<Team> {
    return this.http.post<Team>(this.apiUrl, team);
  }

  updateTeam(id: string, team: UpdateTeam): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, team);
  }

  deleteTeam(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getTeamWithPlayers(id: string): Observable<TeamWithPlayers> {
    return this.http.get<TeamWithPlayers>(`${this.apiUrl}/${id}/players`);
  }
}