import { Player } from "./player";

export interface Training {
  id: string;
  date: string;
  duration: string;
  location: string;
  focus: string;
  notes?: string;
  teamId: string;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateTraining {
  date: string;
  duration: string;
  location: string;
  focus: string;
  notes?: string;
  teamId: string;
}

export interface UpdateTraining {
  date: string;
  duration: string;
  location: string;
  focus: string;
  notes?: string;
}

export interface TrainingWithAttendances extends Training {
  attendances: TrainingAttendance[];
}

export interface TrainingAttendance {
  id: string;
  trainingId: string;
  playerId: string;
  attended: boolean;
  notes?: string;
  rating?: number;
  player?: Player;
}