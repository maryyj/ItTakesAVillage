namespace ItTakesAVillage.Interfaces;

public interface IEventService<T>
{
    Task<bool> Create(T t);
    Task<List<T>> GetAll();
    Task<List<T>> GetAllOfGroup(string id);
    Task <bool> Delete(int eventId, string userId);
    Task <bool> Update(int id);
}
