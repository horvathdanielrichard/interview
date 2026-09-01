using Interview.Application.Common;
using Microsoft.AspNetCore.SignalR;

namespace Interview.Infrastructure.Notifications;

public class SignalRMissionNotifier(IHubContext<NotificationHub, INotificationClient> hubContext) : IMissionNotifier
{
    public Task NotifyMissionStatusChangedAsync(Guid missionId, string missionName, string status, CancellationToken cancellationToken = default)
    {
        var notification = new MissionNotification(missionId, missionName, status, DateTimeOffset.UtcNow);
        return hubContext.Clients.All.MissionStatusChanged(notification);
    }
}
