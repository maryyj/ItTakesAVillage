namespace ItTakesAVillage.Core.Interfaces;

public interface IGroupService
{
    Task<int> Save(Models.Group group, string userId);
    Task<bool> AddUser(string userId, int groupId);
    Task<bool> RemoveUser(string userId, int groupId);
    Task<Models.Group> Get(int groupId);
    Task<List<Models.ItTakesAVillageUser?>> GetMembers(int groupId);
    Task<List<Models.UserGroup?>> GetUsersAndGroups(int groupId);
    Task<List<Models.Group?>> GetGroupsByUserId(string userId);
}
