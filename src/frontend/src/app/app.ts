import { Component, effect, inject } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink, RouterOutlet } from '@angular/router';
import { MatBadgeModule } from '@angular/material/badge';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatToolbarModule } from '@angular/material/toolbar';
import { NotificationService } from './core/services/notification.service';

@Component({
  selector: 'app-root',
  imports: [
    DatePipe,
    RouterLink,
    RouterOutlet,
    MatBadgeModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatToolbarModule,
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly notificationService = inject(NotificationService);
  private readonly snackBar = inject(MatSnackBar);

  constructor() {
    // Surfaces mission status notifications as a toast on every page, in addition to the bell menu.
    effect(() => {
      const notification = this.notificationService.latest();
      if (!notification) {
        return;
      }

      const icon = notification.status === 'Succeeded' ? '✅' : notification.status === 'Failed' ? '❌' : 'ℹ️';
      this.snackBar.open(
        `${icon} Mission "${notification.missionName}" ${notification.status.toLowerCase()}`,
        'Dismiss',
        { duration: 5000 },
      );
    });
  }
}
