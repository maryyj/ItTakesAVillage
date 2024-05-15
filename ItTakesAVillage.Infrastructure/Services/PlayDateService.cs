namespace ItTakesAVillage.Infrastructure.Services;

public class PlayDateService(IRepository<PlayDate> playDateRepository) : IEventService<PlayDate>
{
    private readonly IRepository<PlayDate> _playDateRepository = playDateRepository;

    public async Task<bool> CreateAsync(PlayDate playDate)
    {
        if (playDate.DateTime.Date < DateTime.Now.Date)
            return false;
        await _playDateRepository.AddAsync(playDate);
        return true;
    }
    public async Task<List<PlayDate>> GetAllAsync()
    {
        return await _playDateRepository.GetOfTypeAsync<BaseEvent>();
    }
    public Task<List<PlayDate>> GetAllForUserGroupsAsync(string id)
    {
        throw new NotImplementedException();
    }
    public Task<bool> DeleteAsync(int eventId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(int t)
    {
        throw new NotImplementedException();
    }
}
