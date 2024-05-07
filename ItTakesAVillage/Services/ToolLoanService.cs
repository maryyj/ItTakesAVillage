
namespace ItTakesAVillage.Services;

public class ToolLoanService(
    IRepository<ToolLoan> toolLoanRepository) : IEventService<ToolLoan>
{
    private readonly IRepository<ToolLoan> _toolLoanRepository = toolLoanRepository;

    public async Task<bool> Create(ToolLoan toolLoan)
    {
        if (toolLoan.ToolPool is null)
            return false;

        toolLoan.ToolPool.IsBorrowed = true;
        await _toolLoanRepository.AddAsync(toolLoan);
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

    public Task<List<ToolLoan>> GetAllOfGroup(string id)
    {
        throw new NotImplementedException();
    }
}
