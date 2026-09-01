using Microsoft.AspNetCore.SignalR;

namespace Interview.Infrastructure.Notifications;

/// <summary>Server-push hub for mission status notifications. Clients only listen; no client-invoked methods are needed.</summary>
public class NotificationHub : Hub<INotificationClient>;
