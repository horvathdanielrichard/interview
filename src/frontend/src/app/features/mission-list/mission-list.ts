import { Component, OnInit, effect, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MissionService } from '../../core/services/mission.service';
import { NotificationService } from '../../core/services/notification.service';
import { Mission } from '../../core/models/mission.model';
import {
  MissionFormDialog,
  MissionFormDialogData,
} from '../mission-form-dialog/mission-form-dialog';

@Component({
  selector: 'app-mission-list',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatCardModule,
    MatChipsModule,
    MatDialogModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatTooltipModule,
  ],
  templateUrl: './mission-list.html',
  styleUrl: './mission-list.scss',
})
export class MissionList implements OnInit {
  private readonly missionService = inject(MissionService);
  private readonly notificationService = inject(NotificationService);
  private readonly dialog = inject(MatDialog);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  readonly missions = signal<Mission[]>([]);
  readonly loading = signal(true);

  constructor() {
    // Refresh the list whenever a mission's status changes so cards stay up to date live.
    effect(() => {
      if (this.notificationService.latest()) {
        this.loadMissions();
      }
    });
  }

  ngOnInit(): void {
    this.loadMissions();
  }

  loadMissions(): void {
    this.missionService.getMissions().subscribe({
      next: (missions) => {
        this.missions.set(missions);
        this.loading.set(false);
      },
      error: () => this.loading.set(false),
    });
  }

  openMission(mission: Mission): void {
    this.router.navigate(['/missions', mission.id]);
  }

  createMission(): void {
    const data: MissionFormDialogData = { mission: null };
    this.dialog
      .open(MissionFormDialog, { data })
      .afterClosed()
      .subscribe((request) => {
        if (!request) {
          return;
        }

        this.missionService.createMission(request).subscribe({
          next: () => {
            this.snackBar.open('Mission created', 'Dismiss', { duration: 3000 });
            this.loadMissions();
          },
          error: () => this.snackBar.open('Failed to create mission', 'Dismiss', { duration: 3000 }),
        });
      });
  }

  editMission(event: Event, mission: Mission): void {
    event.stopPropagation();
    const data: MissionFormDialogData = { mission };
    this.dialog
      .open(MissionFormDialog, { data })
      .afterClosed()
      .subscribe((request) => {
        if (!request) {
          return;
        }

        this.missionService.updateMission(mission.id, request).subscribe({
          next: () => {
            this.snackBar.open('Mission updated', 'Dismiss', { duration: 3000 });
            this.loadMissions();
          },
          error: () => this.snackBar.open('Failed to update mission', 'Dismiss', { duration: 3000 }),
        });
      });
  }

  deleteMission(event: Event, mission: Mission): void {
    event.stopPropagation();
    if (!confirm(`Delete mission "${mission.name}"?`)) {
      return;
    }

    this.missionService.deleteMission(mission.id).subscribe({
      next: () => {
        this.snackBar.open('Mission deleted', 'Dismiss', { duration: 3000 });
        this.loadMissions();
      },
      error: () => this.snackBar.open('Failed to delete mission', 'Dismiss', { duration: 3000 }),
    });
  }

  statusColor(status: string): string {
    switch (status) {
      case 'Succeeded':
        return 'primary';
      case 'Failed':
        return 'warn';
      case 'InProgress':
        return 'accent';
      default:
        return '';
    }
  }
}
