import { Injectable, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../../environments/environment';
import { MissionNotification } from '../models/notification.model';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private readonly connection: signalR.HubConnection;

  /** Most recent notifications first, capped to a reasonable history size. */
  readonly notifications = signal<MissionNotification[]>([]);

  /** Emits each time a new notification arrives, for one-off UI reactions (e.g. toasts). */
  readonly latest = signal<MissionNotification | null>(null);

  constructor() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(environment.notificationsHubUrl)
      .withAutomaticReconnect()
      .build();

    this.connection.on('MissionStatusChanged', (notification: MissionNotification) => {
      this.notifications.update((current) => [notification, ...current].slice(0, 50));
      this.latest.set(notification);
    });

    this.connection.start().catch((error) => console.error('SignalR connection failed', error));
  }
}
