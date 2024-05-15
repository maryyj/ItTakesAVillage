namespace ItTakesAVillage.Infrastructure.Services;

public class ToolPoolService(
    IRepository<ToolPool> toolPoolRepository,
    IEventService<ToolLoan> toolLoanService,
    IRepository<UserGroup> userGroupRepository) : IEventService<ToolPool>
{
    private readonly IRepository<ToolPool> _toolPoolRepository = toolPoolRepository;
    private readonly IEventService<ToolLoan> _toolLoanService = toolLoanService;
    private readonly IRepository<UserGroup> _userGroupRepository = userGroupRepository;

    public async Task<bool> CreateAsync(ToolPool tool)
    {
        if (tool.DateTime.Date < DateTime.Now.Date)
            return false;
        await _toolPoolRepository.AddAsync(tool);
        return true;
    }

    public async Task<List<ToolPool>> GetAllAsync()
    {
        return await _toolPoolRepository.GetOfTypeAsync<BaseEvent>();
    }

    public async Task<List<ToolPool>> GetAllForUserGroupsAsync(string id)
    {
        var groupsOfUser = await GetUserGroups(id);
        await ValidateReturnDate();
        return await GetTools(groupsOfUser);
    }
    public async Task<bool> DeleteAsync(int toolId)
    {
        var tool = await _toolPoolRepository.GetAsync(toolId);

        if (tool == null)
            return false;

        if(tool.IsBorrowed)
            return false;

        await _toolLoanService.DeleteAsync(toolId);
        await _toolPoolRepository.DeleteAsync(tool);

        return true;
    }
    public Task<bool> UpdateAsync(int t)
    {
        throw new NotImplementedException();
    }
    private async Task<List<UserGroup>> GetUserGroups(string id)
    {
        var groupsAndUsers = await _userGroupRepository.GetAsync();
        return groupsAndUsers.Where(x => x.UserId == id).ToList();
    }
    private async Task<List<ToolPool>> GetTools(List<UserGroup> groupsOfUser)
    {
        var tools = await _toolPoolRepository.GetOfTypeAsync<BaseEvent>();
        List<ToolPool> sharedTools = [];
        foreach (var group in groupsOfUser)
        {
            List<ToolPool> toolsForGroup= tools.Where(x => x.GroupId == group.GroupId).ToList();
            sharedTools.AddRange(toolsForGroup);
        }
        return sharedTools;
    }
    private async Task ValidateReturnDate()
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        var loans = await _toolLoanService.GetAllAsync();
        foreach (var loan in loans)
        {
            if (loan.ToDate < today && loan.IsReturned == false)
            {
                loan.ToolPool.IsBorrowed = false;
                loan.IsReturned = true;
            }
        }
    }
}
