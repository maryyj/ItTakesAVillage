using ItTakesAVillage.Contracts;
using ItTakesAVillage.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ItTakesAVillage.Services
{
    public class PlayDateService : IEventService<PlayDate>
    {
        private readonly IRepository<PlayDate> _playDateRepository;

        public PlayDateService(IRepository<PlayDate> playDateRepository)
        {
            _playDateRepository = playDateRepository;
        }

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
}
