using Core.Common;
using Core.Exceptions;
using Core.Interfaces.IRepositories;
using Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.Repositories
{
    public class BaseRepository<T, Tid> : IBaseRepository<T, Tid> where T : class
    {
        protected readonly AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>().Where(filter).AsNoTracking().ToListAsync();
        }

        public async Task<T> GetById(Tid id)
        {
            var data = await _context.Set<T>().FindAsync(id);
            return data;
        }

        public async Task<bool> IsExist(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>().AnyAsync(filter);
        }

        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void CreateRange(List<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void DeleteRange(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }


        public void Update(T entity)
        {
             _context.Set<T>().Update(entity);
        }

        public void UpdateRange(List<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
        }

        public async Task<bool> SaveChanges()
        {
            int changedRows = await _context.SaveChangesAsync();
            return changedRows > 0; 
        }

        public IQueryable<T> GetQuery()
        {
            return _context.Set<T>();
        }


        public async Task<T?> Get(Expression<Func<T, bool>> lamda)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(lamda);
        }
    }
}
