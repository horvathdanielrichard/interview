namespace Interview.Application.Common;

/// <summary>Sends real-time mission notifications to connected clients. Implemented in Infrastructure using SignalR.</summary>
public interface IMissionNotifier
{
    Task NotifyMissionStatusChangedAsync(Guid missionId, string missionName, string status, CancellationToken cancellationToken = default);
}
