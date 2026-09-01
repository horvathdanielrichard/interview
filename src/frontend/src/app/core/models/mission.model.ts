export type MissionStatus = 'Pending' | 'InProgress' | 'Succeeded' | 'Failed';

export interface MissionHero {
  heroId: string;
  name: string;
}

export interface Mission {
  id: string;
  name: string;
  description: string;
  requiredStrength: number;
  requiredSpeed: number;
  requiredIntelligence: number;
  requiredDurability: number;
  requiredEnergy: number;
  duration: string;
  status: MissionStatus;
  createdAt: string;
  startedAt: string | null;
  completedAt: string | null;
  assignedHeroes: MissionHero[];
}

export interface CreateMissionRequest {
  name: string;
  description: string;
  requiredStrength: number;
  requiredSpeed: number;
  requiredIntelligence: number;
  requiredDurability: number;
  requiredEnergy: number;
  duration: string;
}

export type UpdateMissionRequest = CreateMissionRequest;

export interface AssignHeroesRequest {
  heroIds: string[];
}
