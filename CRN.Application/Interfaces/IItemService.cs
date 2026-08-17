using CRN.Application.DTOs.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN.Application.Interfaces
{
    public interface IItemService
    {
        Task<ItemResponse?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<ItemResponse> CreateAsync(
            CreateItemRequest request,
            CancellationToken cancellationToken = default);

        Task<ItemResponse?> UpdateAsync(
            int id,
            UpdateItemRequest request,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ItemResponse>> GetByProductIdAsync(
            int productId,
            CancellationToken cancellationToken = default);
    }
}

