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
    public async Task<List<PlayDate>> GetAllOfGroupAsync(object id)
    {
        if (id is int groupId)
            return await _playDateRepository.GetByFilterAsync(x => x.GroupId == groupId);
        else
            throw new ArgumentException("Parameter must be int");
    }

    public async Task<bool> DeleteAsync(int eventId)
    {
        var playdate = await _playDateRepository.GetAsync(eventId);
        if (playdate == null)
            return false;
        await _playDateRepository.DeleteAsync(playdate);

        return true;
    }

    public async Task<bool> UpdateAsync(PlayDate playDate)
    {
        if (playDate == null | playDate.DateTime < DateTime.Now)
            return false;
        await _playDateRepository.UpdateAsync(playDate);
        return true;
    }

    public async Task<PlayDate> GetAsync(int id) => await _playDateRepository.GetAsync(id);
}
