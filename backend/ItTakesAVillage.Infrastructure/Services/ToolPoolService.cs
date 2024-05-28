namespace ItTakesAVillage.Infrastructure.Services;

public class ToolPoolService(
    IRepository<ToolPool> toolPoolRepository,
    IRepository<UserGroup> userGroupRepository,
    IEventService<ToolLoan> toolLoanService) : IEventService<ToolPool>
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
    public async Task<ToolPool> GetAsync(int id)
    {
        return await _toolPoolRepository.GetAsync(id);
    }
    public async Task<List<ToolPool>> GetAllAsync()
    {
        return await _toolPoolRepository.GetOfTypeAsync<BaseEvent>();
    }

    public async Task<List<ToolPool>> GetAllOfGroupAsync(object id)
    {
        if (id is string userId)
        {
            var groupsOfUser = await GetUserGroups(userId);
            var toolLoans = await _toolLoanService.GetAllAsync();
            ValidateReturnDate(toolLoans);
            return await GetTools(groupsOfUser);
        }
        else
            throw new ArgumentException("Parameter must be string");
    }
    public async Task<bool> UpdateAsync(ToolPool tool)
    {
        if (tool == null)
            return false;
        await _toolPoolRepository.UpdateAsync(tool);
        return true;
    }
    public async Task<bool> DeleteAsync(int toolId)
    {
        var tool = await _toolPoolRepository.GetAsync(toolId);
        if (tool == null || tool.IsBorrowed == true)
            return false;
        await _toolLoanService.DeleteAsync(toolId);
        await _toolPoolRepository.DeleteAsync(tool);

        return true;
    }
    public static void ValidateReturnDate(List<ToolLoan> loans)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        foreach (var loan in loans)
        {
            if (loan.ToDate < today && loan.IsReturned == false)
            {
                loan.ToolPool.IsBorrowed = false;
                loan.IsReturned = true;
            }
        }
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
}
