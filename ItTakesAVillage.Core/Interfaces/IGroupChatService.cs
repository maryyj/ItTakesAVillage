namespace ItTakesAVillage.Core.Interfaces;

public interface IGroupChatService
{
    Task<List<Models.GroupChat>> Get(int groupId);
    Task<bool> Add(Models.GroupChat message);
}
