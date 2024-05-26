namespace ItTakesAVillage.Core.Interfaces;

public interface IItTakesAVillageUserService
{
    Task<ItTakesAVillageUser> GetAsync(int userId);
}
