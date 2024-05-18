namespace ItTakesAVillage.Services;

public class ToolLoanService(
    IRepository<ToolLoan> toolLoanRepository) : IEventService<ToolLoan>
{
    private readonly IRepository<ToolLoan> _toolLoanRepository = toolLoanRepository;

    public async Task<bool> Create(ToolLoan toolLoan)
    {
        if (toolLoan.FromDate < DateOnly.FromDateTime(DateTime.Today) || toolLoan.ToDate < DateOnly.FromDateTime(DateTime.Today))
            return false;
        if (toolLoan.ToolPool is null)
            return false;

        toolLoan.ToolPool.IsBorrowed = true;
        await _toolLoanRepository.AddAsync(toolLoan);
        return true;
    }
    public async Task<List<ToolLoan>> GetAllOfGroup(string id) => await _toolLoanRepository.GetByFilterAsync(x => x.BorrowerId == id && x.IsReturned == false);

    public async Task<bool> Update(int id) //TODO: Change services and interfaces maybe only toolpool services
    {
        var loan = await _toolLoanRepository.GetAsync(id);
        if (loan == null) 
            return false;
        loan.IsReturned = true;
        loan.ToolPool.IsBorrowed = false;
        await _toolLoanRepository.UpdateAsync(loan);
        return true;
    }
    public Task<bool> Delete(int eventId, string userId)
    {
        throw new NotImplementedException();
    }

    public Task<List<ToolLoan>> GetAll()
    {
        throw new NotImplementedException();
    }
}
