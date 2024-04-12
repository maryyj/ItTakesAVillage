using ItTakesAVillage.Models;

namespace ItTakesAVillage.Contracts
{
    public interface IGroupService
    {
        Task<int> Save(Group group, string userId);
        Task<bool> AddUser(string userId, int groupId);
        Task<List<ItTakesAVillageUser?>> GetMembers(int groupId);
        Task<List<Group?>> GetGroupsByUserId(string userId);
    }
}
