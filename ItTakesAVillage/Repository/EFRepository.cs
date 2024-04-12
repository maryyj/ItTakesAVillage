using ItTakesAVillage.Contracts;
using ItTakesAVillage.Data;
using ItTakesAVillage.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ItTakesAVillage.Repository
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        private readonly ItTakesAVillageContext _context;
        public EFRepository(ItTakesAVillageContext context)
        {
            _context = context;
        }
        public async Task AddAsync(T t)
        {
            await _context.Set<T>().AddAsync(t);
            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> GetAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<T?> GetAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task<T?> GetAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetByFilterAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<List<T>> GetOfTypeAsync<R>() where R : class
        {
            return await _context.Set<R>()
                .OfType<T>()
                .ToListAsync();
        }

        public async Task UpdateAsync(T t)
        {
            _context.Entry(t).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
