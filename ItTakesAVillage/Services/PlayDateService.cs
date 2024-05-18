namespace ItTakesAVillage.Services;

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
    public async Task<List<PlayDate>> GetAllOfGroup(object id)
    {
        if (id is int groupId)
            return await _playDateRepository.GetByFilterAsync(x => x.GroupId == groupId);
        else
            throw new ArgumentException("Parameter must be int");
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
