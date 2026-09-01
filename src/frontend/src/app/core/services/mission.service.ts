import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  AssignHeroesRequest,
  CreateMissionRequest,
  Mission,
  UpdateMissionRequest,
} from '../models/mission.model';

@Injectable({ providedIn: 'root' })
export class MissionService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/missions`;

  getMissions(): Observable<Mission[]> {
    return this.http.get<Mission[]>(this.baseUrl);
  }

  getMission(id: string): Observable<Mission> {
    return this.http.get<Mission>(`${this.baseUrl}/${id}`);
  }

  createMission(request: CreateMissionRequest): Observable<Mission> {
    return this.http.post<Mission>(this.baseUrl, request);
  }

  updateMission(id: string, request: UpdateMissionRequest): Observable<Mission> {
    return this.http.put<Mission>(`${this.baseUrl}/${id}`, request);
  }

  deleteMission(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  assignHeroes(id: string, request: AssignHeroesRequest): Observable<Mission> {
    return this.http.post<Mission>(`${this.baseUrl}/${id}/assign`, request);
  }
}
