using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ItTakesAVillage.Services;

public class ToolLoanService(
    IRepository<ToolLoan> toolLoanRepository) : IEventService<ToolLoan>
{
    private readonly IRepository<ToolLoan> _toolLoanRepository = toolLoanRepository;
    public async Task<List<ToolLoan>> GetAll() => await _toolLoanRepository.GetAsync();
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
    public async Task<List<ToolLoan>> GetAllOfGroup(object id)
    {
        if (id is string userId)
            return await _toolLoanRepository.GetByFilterAsync(x => x.BorrowerId == userId && x.IsReturned == false);
        else
            throw new ArgumentException("Parameter must be string");
    }

    public async Task<bool> Update(ToolLoan loan)
    {
        if (loan == null)
            return false;
        loan.IsReturned = true;
        loan.ToolPool.IsBorrowed = false;
        await _toolLoanRepository.UpdateAsync(loan);
        return true;
    }
    public async Task<bool> Delete(int toolId)
    {
        List<ToolLoan> toolLoans = await _toolLoanRepository.GetByFilterAsync(x => x.ToolId == toolId);
        if (toolLoans == null)
            return false;
        foreach (var loan in toolLoans)
        {
            await _toolLoanRepository.DeleteAsync(loan);
        }
        return true;
    }
}
