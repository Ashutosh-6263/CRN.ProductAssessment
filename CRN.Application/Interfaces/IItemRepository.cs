using CRN.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN.Application.Interfaces
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<IReadOnlyList<Item>> GetByProductIdAsync(
            int productId,
            CancellationToken cancellationToken = default);
    }
}
