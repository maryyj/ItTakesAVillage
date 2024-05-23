namespace ItTakesAVillage.Core.Interfaces;

public interface IEventService<T>
{
    Task<bool> CreateAsync(T t);
    Task<T> GetAsync(int id);
    Task<List<T>> GetAllAsync();
    Task<List<T>> GetAllOfGroupAsync(object id);
    Task <bool> DeleteAsync(int eventId);
    Task <bool> UpdateAsync(T t);
}
