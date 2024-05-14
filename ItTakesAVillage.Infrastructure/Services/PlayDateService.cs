namespace ItTakesAVillage.Infrastructure.Services;

public class PlayDateService(IRepository<PlayDate> playDateRepository) : IEventService<PlayDate>
{
    private readonly IRepository<PlayDate> _playDateRepository = playDateRepository;

    public async Task<bool> Create(PlayDate playDate)
    {
        if (playDate.DateTime.Date < DateTime.Now.Date)
            return false;
        await _playDateRepository.AddAsync(playDate);
        return true;
    }
    public async Task<List<PlayDate>> GetAll()
    {
        return await _playDateRepository.GetOfTypeAsync<BaseEvent>();
    }
    public Task<List<PlayDate>> GetAllOfGroup(string id)
    {
        throw new NotImplementedException();
    }
    public Task<bool> Delete(int eventId, string userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update(int t)
    {
        throw new NotImplementedException();
    }
}
