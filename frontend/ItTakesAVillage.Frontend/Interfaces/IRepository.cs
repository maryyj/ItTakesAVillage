namespace ItTakesAVillage.Interfaces;

public interface IRepository<T>
{
    Task AddAsync(T t);
    Task<List<T>> GetAsync();
    Task<T?> GetAsync(int id);
    Task<T?> GetAsync(string id);
    Task<List<T>> GetOfTypeAsync<R>() where R : class;
    Task UpdateAsync(T t);
    Task DeleteAsync(T t);
    Task<List<T>> GetByFilterAsync(Expression<Func<T, bool>> expression);
}
