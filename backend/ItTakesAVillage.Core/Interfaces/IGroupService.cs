namespace ItTakesAVillage.Core.Interfaces;

public interface IGroupService
{
    Task<bool> CreateAsync(Group group, string userId);
    Task<bool> AddUserAsync(string userId, int groupId);
    Task<bool> DeleteUserAsync(string userId, int groupId);
    Task<Group> GetAsync(int groupId);
    Task<List<ItTakesAVillageUser?>> GetMembersAsync(int groupId);
    Task<List<UserGroup?>> GetUsersAndGroupsAsync(int groupId);
    Task<List<Group?>> GetGroupsByUserIdAsync(string userId);
}
