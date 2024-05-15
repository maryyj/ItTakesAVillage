namespace ItTakesAVillage.Core.Interfaces;

public interface IGroupChatService
{
    Task<List<Models.GroupChat>> GetAsync(int groupId);
    Task<bool> AddAsync(Models.GroupChat message);
}
