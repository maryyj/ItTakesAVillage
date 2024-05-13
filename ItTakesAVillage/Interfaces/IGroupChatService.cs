namespace ItTakesAVillage.Interfaces;

public interface IGroupChatService
{
    Task<List<GroupChat>> Get(int groupId);
    Task<bool> Add(GroupChat message);
}
