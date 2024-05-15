namespace ItTakesAVillage.Core.Interfaces;

public interface IGroupChatService
{
    Task<List<GroupChat>> GetAsync(int groupId);
    Task<bool> AddAsync(GroupChat message);
}
