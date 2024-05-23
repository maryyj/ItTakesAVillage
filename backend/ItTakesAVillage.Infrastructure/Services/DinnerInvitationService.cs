namespace ItTakesAVillage.Infrastructure.Services
{
    public class DinnerInvitationService(
        IRepository<DinnerInvitation> dinnerInvitationRepository) : IEventService<DinnerInvitation>
    {
        private readonly IRepository<DinnerInvitation> _dinnerInvitationRepository = dinnerInvitationRepository;

        public async Task<List<DinnerInvitation>> GetAllAsync()
        {
            return await _dinnerInvitationRepository.GetOfTypeAsync<BaseEvent>();
        }
        public async Task<bool> CreateAsync(DinnerInvitation invitation)
        {
            if (invitation.DateTime.Date < DateTime.Now.Date)
                return false;

            await _dinnerInvitationRepository.AddAsync(invitation);
            return true;
        }
        public async Task<List<DinnerInvitation>> GetAllOfGroupAsync(object id)
        {
            if (id is int groupId)
                return await _dinnerInvitationRepository.GetByFilterAsync(x => x.GroupId == groupId);
            else
                throw new ArgumentException("Parameter must be int");
        }
        public async Task<bool> DeleteAsync(int eventId)
        {
            var invitation = await _dinnerInvitationRepository.GetAsync(eventId);
            if (invitation == null)
                return false;
            await _dinnerInvitationRepository.DeleteAsync(invitation);

            return true;
        }
        public async Task<bool> UpdateAsync(DinnerInvitation invitation)
        {
            if (invitation == null || invitation.DateTime < DateTime.Now)
                return false;

            await _dinnerInvitationRepository.UpdateAsync(invitation);
            return true;
        }

        public async Task<DinnerInvitation> GetAsync(int id) => await _dinnerInvitationRepository.GetAsync(id);
    }
}
