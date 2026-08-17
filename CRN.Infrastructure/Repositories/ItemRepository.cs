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
    public class ItemRepository
    : Repository<Item>, IItemRepository
    {
        public ItemRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IReadOnlyList<Item>> GetByProductIdAsync(
            int productId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.ProductId == productId)
                .ToListAsync(cancellationToken);
        }
    }
}
