using CRN.Application.Interfaces;
using CRN.Domain.Entities;
using CRN.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN.Infrastructure.Repositories
{
    public class ProductRepository
    : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<Product?> GetByNameAsync(
            string productName,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.ProductName == productName,
                    cancellationToken);
        }
    }
}
