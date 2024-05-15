namespace ItTakesAVillage.Infrastructure.Services;

public class ToolLoanService(
    IRepository<ToolLoan> toolLoanRepository) : IEventService<ToolLoan>
{
    private readonly IRepository<ToolLoan> _toolLoanRepository = toolLoanRepository;

    public async Task<bool> CreateAsync(ToolLoan toolLoan)
    {
        if (toolLoan.FromDate < DateOnly.FromDateTime(DateTime.Today) || toolLoan.ToDate < DateOnly.FromDateTime(DateTime.Today))
            return false;
        if (toolLoan.ToolPool is null)
            return false;

        toolLoan.ToolPool.IsBorrowed = true;
        await _toolLoanRepository.AddAsync(toolLoan);
        return true;
    }
    public async Task<List<ToolLoan>> GetAllForUserGroupsAsync(string id) => await _toolLoanRepository.GetByFilterAsync(x => x.BorrowerId == id && x.IsReturned == false);
    public async Task<List<ToolLoan>> GetAllAsync() => await _toolLoanRepository.GetAsync();
    public async Task<bool> UpdateAsync(int id) //TODO: Change services and interfaces maybe only toolpool services
    {
        var loan = await _toolLoanRepository.GetAsync(id);
        if (loan == null) return false;
        loan.IsReturned = true;
        loan.ToolPool.IsBorrowed = false;
        await _toolLoanRepository.UpdateAsync(loan);
        return true;
    }
    public async Task<bool> DeleteAsync(int eventId)
    {
        var tool = await _toolLoanRepository.GetOneByFilterAsync(x => x.ToolId == eventId);
        
        if (tool == null)
            return false;

        await _toolLoanRepository.DeleteAsync(tool);
        return true;
    }
}
