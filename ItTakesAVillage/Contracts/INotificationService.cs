using ItTakesAVillage.Models;
using System.Text.RegularExpressions;

namespace ItTakesAVillage.Contracts
{
    public interface INotificationService
    {
        Task<int> CountAsync(string userId);
        Task<List<Notification>> GetAsync(string userId);
        Task UpdateIsReadAsync(int notificationId);
        Task NotifyGroupAsync<TEvent>(TEvent invitation) where TEvent : BaseEvent;
    }
}
