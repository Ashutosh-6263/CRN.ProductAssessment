using CRN.Application.DTOs;
using CRN.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN.Application.Interfaces
{
    public interface IProductService
    {
        Task<PagedResponse<ProductResponse>> GetAllAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

        Task<ProductResponse?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<ProductResponse> CreateAsync(
            CreateProductRequest request,
            string createdBy,
            CancellationToken cancellationToken = default);

        Task<ProductResponse?> UpdateAsync(
            int id,
            UpdateProductRequest request,
            string modifiedBy,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}
