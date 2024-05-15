namespace ItTakesAVillage.Core.Interfaces;
public interface IEventService<T>
{
    Task<bool> CreateAsync(T t);
    Task<List<T>> GetAllAsync();
    Task<List<T>> GetAllForUserGroupsAsync(string id);
    Task <bool> DeleteAsync(int eventId);
    Task <bool> UpdateAsync(int id);
}
