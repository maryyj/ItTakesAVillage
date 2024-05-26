
namespace ItTakesAVillage.Infrastructure.Services;

public class ItTAkesAVillageUserService(IRepository<ItTakesAVillageUser> itTakesAVillageUserRepository) : IItTakesAVillageUserService
{
    private readonly IRepository<ItTakesAVillageUser> _itTakesAVillageUserRepository = itTakesAVillageUserRepository;
    public Task<ItTakesAVillageUser> GetAsync(int userId)
    {
        return _itTakesAVillageUserRepository.GetAsync(userId);
    }
}
