import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Match, CreateMatch, UpdateMatch, MatchWithPlayers } from '../models/match';

@Injectable({
  providedIn: 'root'
})
export class MatchService {
  private apiUrl = 'https://localhost:7279/api/matches';

  constructor(private http: HttpClient) { }

  getMatches(): Observable<Match[]> {
    return this.http.get<Match[]>(this.apiUrl);
  }

  getMatch(id: string): Observable<Match> {
    return this.http.get<Match>(`${this.apiUrl}/${id}`);
  }

  createMatch(match: CreateMatch): Observable<Match> {
    return this.http.post<Match>(this.apiUrl, match);
  }

  updateMatch(id: string, match: UpdateMatch): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, match);
  }

  deleteMatch(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getMatchWithPlayers(id: string): Observable<MatchWithPlayers> {
    return this.http.get<MatchWithPlayers>(`${this.apiUrl}/${id}/players`);
  }

  getUpcomingMatches(): Observable<Match[]> {
    return this.http.get<Match[]>(`${this.apiUrl}/upcoming`);
  }
}