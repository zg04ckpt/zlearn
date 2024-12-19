using Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IRepositories
{ 

    public interface IBaseRepository<T, Tid> where T : class // ràng buộc T phải là 1 class
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter);
        IQueryable<T> GetQuery();
        Task<bool> IsExist(Expression<Func<T, bool>> filter);
        Task<T> GetById(Tid id);
        Task<T?> Get(Expression<Func<T, bool>> lamda);
        void Create(T entity);
        void CreateRange(List<T> entities);
        void Update(T entity);
        void UpdateRange(List<T> entities);
        void Delete(T entity);
        void DeleteRange(List<T> entities);
        Task<bool> SaveChanges();
    }
}
