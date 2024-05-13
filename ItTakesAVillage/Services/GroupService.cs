using ItTakesAVillage.Contracts;
using ItTakesAVillage.Data;
using ItTakesAVillage.Helper;
using ItTakesAVillage.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ItTakesAVillage.Services
{
    public class GroupService(IRepository<Group> groupRepository,
        IRepository<ItTakesAVillageUser> userRepository,
        IRepository<UserGroup> userGroupRepository) : IGroupService
    {
        private readonly IRepository<Group> _groupRepository = groupRepository;
        private readonly IRepository<ItTakesAVillageUser> _userRepository = userRepository;
        private readonly IRepository<UserGroup> _userGroupRepository = userGroupRepository;

        public async Task<Group> GetGroup(int groupId) => await _groupRepository.GetAsync(groupId) ?? throw new ArgumentNullException($"Could not find group {groupId}");
        public async Task<int> Save(Group group, string userId)
        {
            var groupsByUserId = await GetGroupsByUserId(userId);
            var groupNameExists = ExistsWithSimilarName(groupsByUserId, group.Name);
            if (groupNameExists)
                return 0;

            await _groupRepository.AddAsync(group);

            return group.Id;
        }
        public async Task<bool> AddUser(string userId, int groupId)
        {
            var user = await _userRepository.GetAsync(userId);
            var usergroups = await _userGroupRepository.GetAsync();

            if (user == null)
                return false;

            bool userExistsInList = usergroups.Exists(x => x.UserId == user.Id && x.GroupId == groupId);
            if (userExistsInList)
                return false;

            var userGroup = new UserGroup
            {
                UserId = userId,
                GroupId = groupId
            };

            await _userGroupRepository.AddAsync(userGroup);

            return true;
        }
        public async Task<bool> RemoveUser(string userId, int groupId)
        {
            var usergroups = await _userGroupRepository.GetAsync();

            if (usergroups == null)
                return false;

            var userGroup = usergroups.FirstOrDefault(x => x.GroupId == groupId && x.UserId == userId);

            if (userGroup == null)
                return false;

            await _userGroupRepository.DeleteAsync(userGroup);
            return true;
        }
        public async Task<List<ItTakesAVillageUser?>> GetMembers(int groupId)
        {
            var userGroups = await _userGroupRepository.GetByFilterAsync(x => x.GroupId == groupId);

            return userGroups.Select(x => x.User).ToList();
        }
        public async Task<List<UserGroup?>> GetUsersAndGroups(int groupId)
        {
            var groupsAndUsers = await _userGroupRepository.GetAsync();
            return groupsAndUsers.Where(x => x.GroupId == groupId).ToList();
        }
        public async Task<List<Group?>> GetGroupsByUserId(string userId)
        {
            var userGroups = await _userGroupRepository.GetByFilterAsync(x => x.UserId == userId);

            return userGroups.Select(x => x.Group).ToList();
        }
        public bool ExistsWithSimilarName(List<Group?> groups, string name)
        {
            var exists = groups.Exists(x => x != null && Validate.NormalizeName(x.Name) == Validate.NormalizeName(name));

            return exists;
        }
    }
}
