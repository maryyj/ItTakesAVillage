namespace ItTakesAVillage.Core.Interfaces;

public interface INotificationService
{
    Task<int> GetCountAsync(string userId);
    Task<List<Notification>> GetAllAsync(string userId);
    Task<Notification> GetAsync(int Id);
    Task UpdateIsReadAsync(Notification notification);
    Task CreateNotificationAsync<TEvent>(TEvent invitation) where TEvent : BaseEvent;
    Task DeleteAsync(int id);
}
