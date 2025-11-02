export interface Player {
  id: string;
  name: string;
  age: number;
  position: string;
  jerseyNumber: string;
  birthDate: string;
  height: number;
  weight: number;
  nationality: string;
  joinDate: string;
  isActive: boolean;
  createdAt: string;
  updatedAt?: string;
}

export interface CreatePlayer {
  name: string;
  age: number;
  position: string;
  jerseyNumber: string;
  birthDate: string;
  height: number;
  weight: number;
  nationality: string;
  joinDate: string;
}

export interface UpdatePlayer {
  name: string;
  position: string;
  jerseyNumber: string;
  height: number;
  weight: number;
  isActive: boolean;
}