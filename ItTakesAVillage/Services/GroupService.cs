using ItTakesAVillage.Contracts;
using ItTakesAVillage.Data;
using ItTakesAVillage.Helper;
using ItTakesAVillage.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ItTakesAVillage.Services
{
    public class GroupService : IGroupService
    {
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<ItTakesAVillageUser> _userRepository;
        private readonly IRepository<UserGroup> _userGroupRepository;
        public GroupService(IRepository<Group> groupRepository,
            IRepository<ItTakesAVillageUser> userRepository,
            IRepository<UserGroup> userGroupRepository)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            _userGroupRepository = userGroupRepository;
        }

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
        public async Task<List<ItTakesAVillageUser?>> GetMembers(int groupId)
        {
            var userGroups = await _userGroupRepository.GetByFilterAsync(x => x.GroupId == groupId);

            return userGroups.Select(x => x.User).ToList();
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
