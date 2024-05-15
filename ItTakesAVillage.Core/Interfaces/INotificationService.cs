namespace ItTakesAVillage.Core.Interfaces;

public interface INotificationService
{
    Task<int> CountAsync(string userId);
    Task<List<Models.Notification>> GetAsync(string userId);
    Task UpdateIsReadAsync(int notificationId);
    Task NotifyGroupAsync<TEvent>(TEvent invitation) where TEvent : Models.BaseEvent;
}
