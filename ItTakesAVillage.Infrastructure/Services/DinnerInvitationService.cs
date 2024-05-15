namespace ItTakesAVillage.Infrastructure.Services
{
    public class DinnerInvitationService : IEventService<DinnerInvitation>
    {
        private readonly IRepository<DinnerInvitation> _dinnerInvitationRepository;

        public DinnerInvitationService(IRepository<DinnerInvitation> dinnerInvitationRepository)
        {
            _dinnerInvitationRepository = dinnerInvitationRepository;
        }
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
        public Task<List<DinnerInvitation>> GetAllForUserGroupsAsync(string id)
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
}
