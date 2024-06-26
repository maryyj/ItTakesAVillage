﻿namespace ItTakesAVillage.Infrastructure.Services;

public class GroupService(IRepository<Group> groupRepository,
    IRepository<ItTakesAVillageUser> userRepository,
    IRepository<UserGroup> userGroupRepository) : IGroupService
{
    private readonly IRepository<Group> _groupRepository = groupRepository;
    private readonly IRepository<ItTakesAVillageUser> _userRepository = userRepository;
    private readonly IRepository<UserGroup> _userGroupRepository = userGroupRepository;

    public async Task<Group> GetAsync(int groupId) => await _groupRepository.GetAsync(groupId) ?? throw new ArgumentNullException($"Could not find group {groupId}");
    public async Task<bool> CreateAsync(Group group, string userId)
    {
        var groupsByUserId = await GetGroupsByUserIdAsync(userId);
        var groupNameExists = ExistsWithSimilarName(groupsByUserId, group.Name);
        if (groupNameExists)
            return false;

        await _groupRepository.AddAsync(group);
        await AddUserAsync(userId, group.Id);
        return true;
    }
    public async Task<bool> AddUserAsync(string userId, int groupId)
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
    public async Task<bool> DeleteUserAsync(string userId, int groupId)
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
    public async Task<bool> DeleteAsync(string userId, int groupId)
    {
        var usergroups = await _userGroupRepository.GetAsync();

        if (usergroups == null)
            return false;

        var usersInGroup = usergroups.Where(x => x.GroupId == groupId).Count();
        if (usersInGroup > 1)
            return false;

        var userGroup = usergroups.FirstOrDefault(x => x.GroupId == groupId && x.UserId == userId);
        if (userGroup == null)
            return false;

        if (userGroup.Group?.CreatorId != userId)
            return false;

        await _userGroupRepository.DeleteAsync(userGroup);
        return true;
    }

    public async Task<List<ItTakesAVillageUser?>> GetMembersAsync(int groupId)
    {
        var userGroups = await _userGroupRepository.GetByFilterAsync(x => x.GroupId == groupId);

        return userGroups.Select(x => x.User).ToList();
    }
    public async Task<List<UserGroup?>> GetUsersAndGroupsAsync(int groupId)
    {
        var groupsAndUsers = await _userGroupRepository.GetAsync();
        return groupsAndUsers.Where(x => x.GroupId == groupId).ToList();
    }
    public async Task<List<Group?>> GetGroupsByUserIdAsync(string userId)
    {
        var userGroups = await _userGroupRepository.GetByFilterAsync(x => x.UserId == userId);
        return userGroups.Select(x => x.Group).ToList();
    }
    private static bool ExistsWithSimilarName(List<Group?> groups, string name)
    {
        var exists = groups.Exists(x => x != null && Helper.Validate.NormalizeName(x.Name) == Helper.Validate.NormalizeName(name));

        return exists;
    }
}
