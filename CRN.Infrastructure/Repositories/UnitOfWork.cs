using CRN.Application.Interfaces;
using CRN.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IProductRepository Products { get; }

        public IItemRepository Items { get; }

        public IUserRepository Users { get; }

        public IRefreshTokenRepository RefreshTokens { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            IProductRepository productRepository,
            IItemRepository itemRepository,
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _context = context;

            Products = productRepository;
            Items = itemRepository;
            Users = userRepository;
            RefreshTokens = refreshTokenRepository;
        }

        public async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(
                cancellationToken);
        }
    }
}
