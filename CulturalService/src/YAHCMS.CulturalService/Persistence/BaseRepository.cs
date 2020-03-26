
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using YAHCMS.CulturalService.Models;

namespace YAHCMS.CulturalService.Persistence
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly CulturalDbContext _context;

        public BaseRepository(CulturalDbContext context)
        {
            _context = context;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(long id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(c => c.ID == id);
        }

        public async Task<List<T>> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<T> CreateAsync(T entity)
        {
            EntityEntry<T> result = await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync(true);
            return result.Entity;
        }

        public async Task<T> Update(T entity)
        {
            EntityEntry<T> result = _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync(true);
            return result.Entity;
        }

        public T Delete(T entity)
        {
            EntityEntry<T> result = _context.Set<T>().Remove(entity);
            _context.SaveChanges(true);
            return result.Entity;
        }
    }
}