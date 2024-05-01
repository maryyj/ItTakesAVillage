
namespace ItTakesAVillage.Services;

public class ToolPoolService(IRepository<ToolPool> toolPoolRepository) : IEventService<ToolPool>
{
    private readonly IRepository<ToolPool> _toolPoolRepository = toolPoolRepository;

    public async Task<bool> Create(ToolPool tool)
    {
        if (tool.DateTime.Date < DateTime.Now.Date)
            return false;
        await _toolPoolRepository.AddAsync(tool);
        return true;
    }

    public Task<List<ToolPool>> GetAll()
    {
        throw new NotImplementedException();
    }
}
