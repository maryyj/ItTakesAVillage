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
}
