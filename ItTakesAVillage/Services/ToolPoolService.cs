namespace ItTakesAVillage.Services;

public class ToolPoolService(
    IRepository<ToolPool> toolPoolRepository,
    IEventService<ToolLoan> toolLoanService,
    IRepository<UserGroup> userGroupRepository) : IEventService<ToolPool>
{
    private readonly IRepository<ToolPool> _toolPoolRepository = toolPoolRepository;
    private readonly IEventService<ToolLoan> _toolLoanService = toolLoanService;
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

    public async Task<List<ToolPool>> GetAllOfGroup(object id)
    {
        if (id is string userId)
        {
            var groupsOfUser = await GetUserGroups(userId);
            await ValidateReturnDate();
            return await GetTools(groupsOfUser);
        }
        else
            throw new ArgumentException("Parameter must be string");
    }
    public async Task<bool> Update(ToolPool tool)
    {
        if (tool == null)
            return false;
        await _toolPoolRepository.UpdateAsync(tool);
        return true;
    }
    public async Task<bool> Delete(int toolId)
    {
        var tool = await _toolPoolRepository.GetAsync(toolId);
        if (tool == null || tool.IsBorrowed == true)
            return false;
        await _toolLoanService.Delete(toolId);
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
        List<ToolPool> sharedTools = [];
        foreach (var group in groupsOfUser)
        {
            List<ToolPool> toolsForGroup = await _toolPoolRepository.GetByFilterAsync(x => x.GroupId == group.GroupId);
            sharedTools.AddRange(toolsForGroup);
        }
        return sharedTools;
    }
    private async Task ValidateReturnDate()
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        var loans = await _toolLoanService.GetAll();
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
