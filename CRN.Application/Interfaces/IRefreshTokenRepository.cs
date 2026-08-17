using CRN.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN.Application.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(
            string token,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            RefreshToken refreshToken,
            CancellationToken cancellationToken = default);
    }
}
