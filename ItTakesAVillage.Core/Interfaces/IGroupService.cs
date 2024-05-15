namespace ItTakesAVillage.Core.Interfaces;

public interface IGroupService
{
    Task<int> SaveAsync(Models.Group group, string userId);
    Task<bool> AddUserAsync(string userId, int groupId);
    Task<bool> RemoveUserAsync(string userId, int groupId);
    Task<Models.Group> GetAsync(int groupId);
    Task<List<Models.ItTakesAVillageUser?>> GetMembersAsync(int groupId);
    Task<List<Models.UserGroup?>> GetUsersAndGroupsAsync(int groupId);
    Task<List<Models.Group?>> GetGroupsByUserIdAsync(string userId);
}
