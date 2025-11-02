import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Player, CreatePlayer, UpdatePlayer } from '../models/player';

@Injectable({
  providedIn: 'root'
})
export class PlayerService {
  private apiUrl = 'https://localhost:7279/api/players';

  constructor(private http: HttpClient) { }

  getPlayers(): Observable<Player[]> {
    return this.http.get<Player[]>(this.apiUrl);
  }

  getPlayer(id: string): Observable<Player> {
    return this.http.get<Player>(`${this.apiUrl}/${id}`);
  }

  createPlayer(player: CreatePlayer): Observable<Player> {
    return this.http.post<Player>(this.apiUrl, player);
  }

  updatePlayer(id: string, player: UpdatePlayer): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, player);
  }

  deletePlayer(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getActivePlayers(): Observable<Player[]> {
    return this.http.get<Player[]>(`${this.apiUrl}/active`);
  }
}