using CRN.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN.Application.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> GetByNameAsync(
            string productName,
            CancellationToken cancellationToken = default);
    }
}
