import { Player } from "./player";

export interface Match {
  id: string;
  date: string;
  opponent: string;
  location: string;
  matchType: string;
  teamScore?: number;
  opponentScore?: number;
  result: string;
  notes?: string;
  teamId: string;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateMatch {
  date: string;
  opponent: string;
  location: string;
  matchType: string;
  teamScore?: number;
  opponentScore?: number;
  notes?: string;
  teamId: string;
}

export interface UpdateMatch {
  date: string;
  opponent: string;
  location: string;
  matchType: string;
  teamScore?: number;
  opponentScore?: number;
  result: string;
  notes?: string;
}

// ✅ CORRIGIDO: MatchWithPlayers deve extender Match e adicionar matchPlayers
export interface MatchWithPlayers extends Match {
  matchPlayers: MatchPlayer[];
}

export interface MatchPlayer {
  id: string;
  matchId: string;
  playerId: string;
  isStarter: boolean;
  position?: string;
  minutesPlayed?: number;
  goals?: number;
  assists?: number;
  yellowCards?: number;
  redCards?: number;
  notes?: string;
  player?: Player;
}