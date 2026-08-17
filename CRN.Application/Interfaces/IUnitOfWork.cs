using System;
using CRN.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }

        IItemRepository Items { get; }

        IUserRepository Users { get; }

        IRefreshTokenRepository RefreshTokens { get; }

        Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default);
    }
}
