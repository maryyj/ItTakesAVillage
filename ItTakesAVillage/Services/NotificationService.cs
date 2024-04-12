using ItTakesAVillage.Contracts;
using ItTakesAVillage.Data;
using ItTakesAVillage.Models;
using ItTakesAVillage.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace ItTakesAVillage.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IGroupService _groupService;
        private readonly IRepository<ItTakesAVillageUser> _userRepository;
        private readonly IRepository<Notification> _notificationRepository;

        public NotificationService(IGroupService groupService,
            IRepository<ItTakesAVillageUser> userRepository,
            IRepository<Notification> notificationRepository)
        {
            _groupService = groupService;
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
        }

        public async Task<List<Notification>> GetAsync(string userId) 
            => await _notificationRepository.GetByFilterAsync(x => x.UserId == userId && x.RelatedEvent.DateTime.Date >= DateTime.Now.Date);
        public async Task<int> CountAsync(string userId)
        {
            var result = await _notificationRepository.GetByFilterAsync(x => x.UserId == userId && !x.IsRead);

            return result.Count;
        }
        public async Task NotifyGroupAsync<TEvent>(TEvent invitation) where TEvent : BaseEvent
        {
            var groupMembers = await _groupService.GetMembers(invitation.GroupId);

            if (!groupMembers.IsNullOrEmpty())
            {
                foreach (var member in groupMembers)
                {
                    if (member != null)
                    {
                        await CreateAsync(invitation, member.Id, GetCreatorId<TEvent>());
                    }
                }
            }
        }
        public async Task UpdateIsReadAsync(int notificationId)
        {
            var existingNotification = await _notificationRepository.GetAsync(notificationId);

            if (existingNotification != null)
            {
                existingNotification.IsRead = true;

                await _notificationRepository.UpdateAsync(existingNotification);
            }
        }
        public async Task CreateAsync<TEvent>(TEvent invitation, string userId, Func<TEvent, string> creatorIdFunc) where TEvent : BaseEvent
        {
            var creatorId = creatorIdFunc(invitation);
            var creator = await _userRepository.GetAsync(creatorId);
            var eventtype = "";

            if (invitation is DinnerInvitation)
                eventtype = "Matlag";
            if (invitation is PlayDate)
                eventtype = "Lekträff";

            var title = creator == null ? $"{eventtype} hos okänd" : $"{eventtype} hos {creator.FirstName} {creator.LastName}";

            var newNotification = new Notification
            {
                UserId = userId,
                Title = title,
                IsRead = false,
                RelatedEvent = invitation
            };

            await _notificationRepository.AddAsync(newNotification);
        }
        private static Func<TEvent, string> GetCreatorId<TEvent>() where TEvent : BaseEvent
        {
            if (typeof(TEvent) == typeof(DinnerInvitation))
            {
                return (TEvent di) => di.CreatorId;

            }
            else if (typeof(TEvent) == typeof(PlayDate))
            {
                return (TEvent pd) => pd.CreatorId;
            }
            else
            {
                throw new ArgumentException($"Unsupported event type: {typeof(TEvent)}");
            }
        }
    }
}
