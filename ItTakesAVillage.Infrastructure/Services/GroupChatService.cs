namespace ItTakesAVillage.Infrastructure.Services;
public class GroupChatService(IRepository<GroupChat> groupChatRepository) : IGroupChatService
{
    private readonly IRepository<GroupChat> _groupChatRepository = groupChatRepository;

    public async Task<bool> Add(GroupChat message)
    {
        if (message == null)
            return false;

        await _groupChatRepository.AddAsync(message);
        return true;
    }

    public async Task<List<GroupChat>> Get(int groupId) => await _groupChatRepository.GetByFilterAsync(x => x.GroupId == groupId);
}
