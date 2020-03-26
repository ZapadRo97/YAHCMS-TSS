
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YAHCMS.CulturalService.Models;

namespace YAHCMS.CulturalService.Persistence
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(long id);
        Task<List<T>> GetByCondition(Expression<Func<T, bool>> expression);
        Task<T> CreateAsync(T entity);
        Task<T> Update(T entity);
        T Delete(T entity);
    }
}