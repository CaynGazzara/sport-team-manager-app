import { Player } from "./player";

export interface Team {
  id: string;
  name: string;
  sportType: string;
  coachName: string;
  homeField: string;
  foundedDate: string;
  colors: string;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateTeam {
  name: string;
  sportType: string;
  coachName: string;
  homeField: string;
  foundedDate: string;
  colors: string;
}

export interface UpdateTeam {
  name: string;
  coachName: string;
  homeField: string;
  colors: string;
}

export interface TeamWithPlayers {
  team: Team;
  players: Player[];
}