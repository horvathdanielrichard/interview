namespace Interview.Infrastructure.Notifications;

public interface INotificationClient
{
    Task MissionStatusChanged(MissionNotification notification);
}

public record MissionNotification(Guid MissionId, string MissionName, string Status, DateTimeOffset Timestamp);
