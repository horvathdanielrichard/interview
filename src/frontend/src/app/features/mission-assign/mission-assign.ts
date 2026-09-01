import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { CdkDrag, CdkDragDrop, CdkDropList, CdkDropListGroup, transferArrayItem } from '@angular/cdk/drag-drop';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HeroService } from '../../core/services/hero.service';
import { MissionService } from '../../core/services/mission.service';
import { Hero } from '../../core/models/hero.model';
import { Mission } from '../../core/models/mission.model';

@Component({
  selector: 'app-mission-assign',
  standalone: true,
  imports: [
    CommonModule,
    CdkDrag,
    CdkDropList,
    CdkDropListGroup,
    MatButtonModule,
    MatCardModule,
    MatChipsModule,
    MatIconModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './mission-assign.html',
  styleUrl: './mission-assign.scss',
})
export class MissionAssign implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly heroService = inject(HeroService);
  private readonly missionService = inject(MissionService);
  private readonly snackBar = inject(MatSnackBar);

  readonly mission = signal<Mission | null>(null);
  readonly roster = signal<Hero[]>([]);
  readonly squad = signal<Hero[]>([]);
  readonly loading = signal(true);
  readonly sending = signal(false);

  readonly canAssign = computed(() => this.mission()?.status === 'Pending');

  readonly totals = computed(() => {
    const heroes = this.squad();
    return {
      strength: heroes.reduce((sum, h) => sum + h.strength, 0),
      speed: heroes.reduce((sum, h) => sum + h.speed, 0),
      intelligence: heroes.reduce((sum, h) => sum + h.intelligence, 0),
      durability: heroes.reduce((sum, h) => sum + h.durability, 0),
      energy: heroes.reduce((sum, h) => sum + h.energy, 0),
    };
  });

  ngOnInit(): void {
    const missionId = this.route.snapshot.paramMap.get('id')!;

    this.missionService.getMission(missionId).subscribe({
      next: (mission) => {
        this.mission.set(mission);
        this.loadRoster(mission);
      },
      error: () => {
        this.loading.set(false);
        this.snackBar.open('Mission not found', 'Dismiss', { duration: 3000 });
        this.router.navigate(['/']);
      },
    });
  }

  private loadRoster(mission: Mission): void {
    this.heroService.getHeroes().subscribe({
      next: (heroes) => {
        const assignedIds = new Set(mission.assignedHeroes.map((h) => h.heroId));
        this.squad.set(heroes.filter((h) => assignedIds.has(h.id)));
        this.roster.set(heroes.filter((h) => !assignedIds.has(h.id)));
        this.loading.set(false);
      },
      error: () => this.loading.set(false),
    });
  }

  drop(event: CdkDragDrop<Hero[]>): void {
    if (!this.canAssign()) {
      return;
    }

    if (event.previousContainer === event.container) {
      return;
    }

    transferArrayItem(
      event.previousContainer.data,
      event.container.data,
      event.previousIndex,
      event.currentIndex,
    );

    // Force signal change detection since transferArrayItem mutates in place.
    this.roster.set([...this.roster()]);
    this.squad.set([...this.squad()]);
  }

  sendOnMission(): void {
    const mission = this.mission();
    if (!mission || this.squad().length === 0) {
      return;
    }

    this.sending.set(true);
    this.missionService
      .assignHeroes(mission.id, { heroIds: this.squad().map((h) => h.id) })
      .subscribe({
        next: (updated) => {
          this.mission.set(updated);
          this.sending.set(false);
          this.snackBar.open(`${mission.name} is underway!`, 'Dismiss', { duration: 3000 });
        },
        error: () => {
          this.sending.set(false);
          this.snackBar.open('Failed to send heroes on mission', 'Dismiss', { duration: 3000 });
        },
      });
  }

  back(): void {
    this.router.navigate(['/']);
  }
}
