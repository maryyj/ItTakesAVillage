namespace ItTakesAVillage.Infrastructure.Services;

public class NotificationService(IGroupService groupService,
    IRepository<ItTakesAVillageUser> userRepository,
    IRepository<Notification> notificationRepository) : INotificationService
{
    private readonly IGroupService _groupService = groupService;
    private readonly IRepository<ItTakesAVillageUser> _userRepository = userRepository;
    private readonly IRepository<Notification> _notificationRepository = notificationRepository;

    public async Task<List<Notification>> GetAllAsync(string userId)
    {
        return await _notificationRepository.GetByFilterAsync(x => x.UserId == userId && x.RelatedEvent != null && x.RelatedEvent.DateTime.Date >= DateTime.Now.Date);
    }
    public async Task<Notification> GetAsync(int id) => await _notificationRepository.GetAsync(id);
    public async Task<int> GetCountAsync(string userId)
    {
        var result = await _notificationRepository.GetByFilterAsync(x => x.UserId == userId && !x.IsRead);

        return result.Count;
    }
    public async Task CreateNotificationAsync<TEvent>(TEvent invitation) where TEvent : BaseEvent
    {
        var groupMembers = await _groupService.GetMembersAsync(invitation.GroupId);

        if (!groupMembers.IsNullOrEmpty())
        {
            foreach (var member in groupMembers)
            {
                if (member != null)
                {
                    await CreateAsync(invitation, member.Id, invitation.CreatorId);
                }
            }
        }
    }
    public async Task UpdateIsReadAsync(Notification notification)
    {
        if (notification != null)
        {
            notification.IsRead = true;

            await _notificationRepository.UpdateAsync(notification);
        }
    }
    public async Task CreateAsync<TEvent>(TEvent invitation, string userId, string creatorId) where TEvent : BaseEvent
    {
        var creator = await _userRepository.GetAsync(creatorId);
        var title = SetTitleOfNotification(invitation, creator);

        var newNotification = new Notification
        {
            UserId = userId,
            Title = title,
            IsRead = false,
        };

        await _notificationRepository.AddAsync(newNotification);
        newNotification.RelatedEvent = invitation;
        await _notificationRepository.UpdateAsync(newNotification);
    }
    private static string SetTitleOfNotification<TEvent>(TEvent invitation, ItTakesAVillageUser creator) where TEvent : BaseEvent
    {
        string eventtype = string.Empty;
        string title = string.Empty;

        if (invitation is DinnerInvitation)
            eventtype = Resources.DinnerInvitation;
        if (invitation is PlayDate)
            eventtype = Resources.PlayDate;
        if (invitation is ToolPool)
            eventtype = Resources.Tool;
        if (invitation is ToolPool)
        {
            title = creator == null ? $"{eventtype} {Resources.Of} {Resources.Unknown}" : $"{eventtype} {Resources.Of} {creator.FirstName} {creator.LastName}";
        }
        else
        {
            title = creator == null ? $"{eventtype} {Resources.At} {Resources.Unknown}" : $"{eventtype} {Resources.At} {creator.FirstName} {creator.LastName}";
        }
        return title;
    }
}
