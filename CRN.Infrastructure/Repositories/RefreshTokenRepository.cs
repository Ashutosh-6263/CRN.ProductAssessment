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
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetByTokenAsync(
            string token,
            CancellationToken cancellationToken = default)
        {
            return await _context.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(
                    x => x.Token == token,
                    cancellationToken);
        }

        public async Task AddAsync(
            RefreshToken refreshToken,
            CancellationToken cancellationToken = default)
        {
            await _context.RefreshTokens.AddAsync(
                refreshToken,
                cancellationToken);
        }
    }
}
