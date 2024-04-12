using ItTakesAVillage.Models;

namespace ItTakesAVillage.Contracts
{
    public interface IEventService<T>
    {
        Task<bool> Create(T t);
        Task<List<T>> GetAll();
    }
}
