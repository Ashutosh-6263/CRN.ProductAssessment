using CRN.Application.DTOs;
using CRN.Application.DTOs.Product;
using CRN.Application.Interfaces;
using CRN.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRN.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResponse<ProductResponse>> GetAllAsync(
    int pageNumber,
    int pageSize,
    CancellationToken cancellationToken = default)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentException(
                    "Page number must be greater than or equal to 1.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentException(
                    "Page size must be greater than 0.");
            }

            if (pageSize > 100)
            {
                throw new ArgumentException(
                    "Page size cannot exceed 100.");
            }

            var query = _unitOfWork.Products
                .Query()
                .AsNoTracking();

            var totalCount = await query.CountAsync(
                cancellationToken);

            var products = await query
                .OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ProductResponse
                {
                    Id = x.Id,
                    ProductName = x.ProductName,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedOn = x.ModifiedOn
                })
                .ToListAsync(cancellationToken);

            return new PagedResponse<ProductResponse>
            {
                Items = products,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<ProductResponse?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var product = await _unitOfWork.Products
                .GetByIdAsync(id, cancellationToken);

            if (product == null)
                return null;

            return MapToResponse(product);
        }

        public async Task<ProductResponse> CreateAsync(
            CreateProductRequest request,
            string createdBy,
            CancellationToken cancellationToken = default)
        {
            var existingProduct =
                await _unitOfWork.Products.GetByNameAsync(
                    request.ProductName,
                    cancellationToken);

            if (existingProduct != null)
            {
                throw new InvalidOperationException(
                    "A product with the same name already exists.");
            }

            var product = new Product
            {
                ProductName = request.ProductName.Trim(),
                CreatedBy = createdBy,
                CreatedOn = DateTime.UtcNow
            };

            await _unitOfWork.Products.AddAsync(
                product,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new ProductResponse
            {
                Id = product.Id,
                ProductName = product.ProductName,
                CreatedBy = product.CreatedBy,
                CreatedOn = product.CreatedOn,
                ModifiedBy = product.ModifiedBy,
                ModifiedOn = product.ModifiedOn
            };
        }

        public async Task<ProductResponse?> UpdateAsync(
            int id,
            UpdateProductRequest request,
            string modifiedBy,
            CancellationToken cancellationToken = default)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(
                id,
                cancellationToken);

            if (product == null)
            {
                return null;
            }

            var existingProduct =
                await _unitOfWork.Products.GetByNameAsync(
                    request.ProductName,
                    cancellationToken);

            if (existingProduct != null &&
                existingProduct.Id != id)
            {
                throw new InvalidOperationException(
                    "A product with the same name already exists.");
            }

            product.ProductName = request.ProductName.Trim();
            product.ModifiedBy = modifiedBy;
            product.ModifiedOn = DateTime.UtcNow;

            _unitOfWork.Products.Update(product);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new ProductResponse
            {
                Id = product.Id,
                ProductName = product.ProductName,
                CreatedBy = product.CreatedBy,
                CreatedOn = product.CreatedOn,
                ModifiedBy = product.ModifiedBy,
                ModifiedOn = product.ModifiedOn
            };
        }

        public async Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(
                id,
                cancellationToken);

            if (product == null)
            {
                return false;
            }

            _unitOfWork.Products.Delete(product);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return true;
        }
        private static ProductResponse MapToResponse(Product product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                ProductName = product.ProductName,
                CreatedBy = product.CreatedBy,
                CreatedOn = product.CreatedOn,
                ModifiedBy = product.ModifiedBy,
                ModifiedOn = product.ModifiedOn
            };
        }
    }
}
