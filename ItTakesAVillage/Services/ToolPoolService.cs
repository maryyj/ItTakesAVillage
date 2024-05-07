namespace ItTakesAVillage.Services;

public class ToolPoolService(
    IRepository<ToolPool> toolPoolRepository,
    IRepository<ToolLoan> toolLoanRepository,
    IRepository<UserGroup> userGroupRepository) : IEventService<ToolPool>
{
    private readonly IRepository<ToolPool> _toolPoolRepository = toolPoolRepository;
    private readonly IRepository<ToolLoan> _toolLoanRepository = toolLoanRepository;
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
        await ValidateReturnDate();
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
        List<ToolPool> toolsOfUserGroups = [];
        foreach (var group in groupsOfUser)
        {
            toolsOfUserGroups = tools.Where(x => x.GroupId == group.GroupId).ToList();
        }
        return toolsOfUserGroups;
    }
    private async Task ValidateReturnDate()
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        var loans = await _toolLoanRepository.GetAsync();
        foreach (var loan in loans)
        {
            if (loan.ToDate < today)
            {
                loan.ToolPool.IsBorrowed = false;
            }
        }
    }
}
