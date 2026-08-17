using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            T entity,
            CancellationToken cancellationToken = default);

        void Update(T entity);

        void Delete(T entity);

        IQueryable<T> Query();
    }
}
