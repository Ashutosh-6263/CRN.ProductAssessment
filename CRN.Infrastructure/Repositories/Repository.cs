using CRN.Application.Interfaces;
using CRN.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T>
    where T : class
    {
        protected readonly ApplicationDbContext _context;

        protected readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .FindAsync(
                    new object[] { id },
                    cancellationToken);
        }

        public virtual async Task AddAsync(
            T entity,
            CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();
        }
    }
}
