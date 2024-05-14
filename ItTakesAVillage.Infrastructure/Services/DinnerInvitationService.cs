namespace ItTakesAVillage.Infrastructure.Services
{
    public class DinnerInvitationService : IEventService<DinnerInvitation>
    {
        private readonly IRepository<DinnerInvitation> _dinnerInvitationRepository;

        public DinnerInvitationService(IRepository<DinnerInvitation> dinnerInvitationRepository)
        {
            _dinnerInvitationRepository = dinnerInvitationRepository;
        }
        public async Task<List<DinnerInvitation>> GetAll()
        {
            return await _dinnerInvitationRepository.GetOfTypeAsync<BaseEvent>();
        }
        public async Task<bool> Create(DinnerInvitation invitation)
        {
            if (invitation.DateTime.Date < DateTime.Now.Date)
                return false;

            await _dinnerInvitationRepository.AddAsync(invitation);
            return true;
        }
        public Task<List<DinnerInvitation>> GetAllOfGroup(string id)
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
}
