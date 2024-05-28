namespace ItTakesAVillage.Infrastructure.Services;

public class ToolLoanService(
    IRepository<ToolLoan> toolLoanRepository,
    IRepository<ToolPool> toolPoolRepository) : IEventService<ToolLoan>
{
    private readonly IRepository<ToolLoan> _toolLoanRepository = toolLoanRepository;
    private readonly IRepository<ToolPool> _toolPoolRepository = toolPoolRepository;

    public async Task<bool> CreateAsync(ToolLoan loan)
    {
        if (loan.FromDate < DateOnly.FromDateTime(DateTime.Today) ||
            loan.ToDate < DateOnly.FromDateTime(DateTime.Today) ||
            loan.FromDate > loan.ToDate)
            return false;

        loan.ToolPool = await _toolPoolRepository.GetAsync(loan.ToolId);

        if (loan.ToolPool == null)
            return false;

        loan.ToolPool.IsBorrowed = true;
        await _toolLoanRepository.AddAsync(loan);
        return true;
    }
    public async Task<ToolLoan> GetAsync(int toolId)
    {
        return await _toolLoanRepository.GetOneByFilterAsync(x => x.ToolId == toolId && x.IsReturned == false);
    }
    public async Task<List<ToolLoan>> GetAllAsync() => await _toolLoanRepository.GetAsync();
    public async Task<List<ToolLoan>> GetAllOfGroupAsync(object id)
    {
        if (id is string userId)
            return await _toolLoanRepository.GetByFilterAsync(x => x.BorrowerId == userId && x.IsReturned == false);
        else
            throw new ArgumentException("Parameter must be string");
    }
    public async Task<bool> UpdateAsync(ToolLoan loan)
    {
        if (loan.FromDate < DateOnly.FromDateTime(DateTime.Today) ||
            loan.ToDate < DateOnly.FromDateTime(DateTime.Today) ||
            loan.FromDate > loan.ToDate)
            return false;

        if (loan == null || loan.ToolPool == null)
            return false;

        loan.IsReturned = true;
        loan.ToolPool.IsBorrowed = false;
        await _toolLoanRepository.UpdateAsync(loan);
        await _toolPoolRepository.UpdateAsync(loan.ToolPool);
        return true;
    }
    public async Task<bool> DeleteAsync(int toolId)
    {
        List<ToolLoan> toolLoans = await _toolLoanRepository.GetByFilterAsync(x => x.ToolId == toolId);
        if (toolLoans == null || toolLoans.Any())
            return false;
        foreach (var loan in toolLoans)
        {
            await _toolLoanRepository.DeleteAsync(loan);
        }
        return true;
    }
}
