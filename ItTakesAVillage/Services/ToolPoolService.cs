namespace ItTakesAVillage.Services;

public class ToolPoolService(IRepository<ToolPool> toolPoolRepository, IRepository<UserGroup> userGroupRepository) : IEventService<ToolPool>
{
    private readonly IRepository<ToolPool> _toolPoolRepository = toolPoolRepository;
    private readonly IRepository<UserGroup> _userGroupRepository = userGroupRepository;

    public async Task<bool> Create(ToolPool tool)
    {
        if (tool.DateTime.Date < DateTime.Now.Date)
            return false;
        await _toolPoolRepository.AddAsync(tool);
        return true;
    }

    public async Task<List<ToolPool>> GetAll()
    {
        return await _toolPoolRepository.GetOfTypeAsync<BaseEvent>();
    }

    public async Task<List<ToolPool>> GetAllOfGroup(string id)
    {
        var groupsOfUser = await GetUserGroups(id);

        return await GetTools(groupsOfUser);
    }
    public async Task<bool> Delete(int toolId, string userId)
    {
        var groupsOfUser = await GetUserGroups(userId);
        var tools = await GetTools(groupsOfUser);
        var tool = tools.Find(x => x.Id == toolId);
        await _toolPoolRepository.DeleteAsync(tool);

        return true;
    }
    private async Task<List<UserGroup>> GetUserGroups(string id)
    {
        var groupsAndUsers = await _userGroupRepository.GetAsync();
        return groupsAndUsers.Where(x => x.UserId == id).ToList();
    }
    private async Task<List<ToolPool>> GetTools(List<UserGroup> groupsOfUser)
    {
        var tools = await _toolPoolRepository.GetOfTypeAsync<BaseEvent>();
        var toolsOfUserGroups = new List<ToolPool>();
        foreach (var group in groupsOfUser)
        {
            toolsOfUserGroups = tools.Where(x => x.GroupId == group.GroupId).ToList();
        }
        return toolsOfUserGroups;
    }
}
