using CRN.Application.DTOs.Item;
using CRN.Application.Interfaces;
using CRN.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN.Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ItemResponse?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Items
                .Query()
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new ItemResponse
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ItemResponse> CreateAsync(
            CreateItemRequest request,
            CancellationToken cancellationToken = default)
        {
            var productExists = await _unitOfWork.Products
                .Query()
                .AnyAsync(
                    x => x.Id == request.ProductId,
                    cancellationToken);

            if (!productExists)
            {
                throw new KeyNotFoundException(
                    $"Product with id {request.ProductId} was not found.");
            }

            var item = new Item
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };

            await _unitOfWork.Items.AddAsync(
                item,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new ItemResponse
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity
            };
        }

        public async Task<ItemResponse?> UpdateAsync(
            int id,
            UpdateItemRequest request,
            CancellationToken cancellationToken = default)
        {
            var item = await _unitOfWork.Items
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);

            if (item == null)
            {
                return null;
            }

            item.Quantity = request.Quantity;

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new ItemResponse
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity
            };
        }

        public async Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var item = await _unitOfWork.Items
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);

            if (item == null)
            {
                return false;
            }

            _unitOfWork.Items.Delete(item);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return true;
        }
        public async Task<IReadOnlyList<ItemResponse>> GetByProductIdAsync(
    int productId,
    CancellationToken cancellationToken = default)
        {
            var productExists = await _unitOfWork.Products
                .Query()
                .AnyAsync(
                    x => x.Id == productId,
                    cancellationToken);

            if (!productExists)
            {
                throw new KeyNotFoundException(
                    $"Product with id {productId} was not found.");
            }

            var items = await _unitOfWork.Items
                .GetByProductIdAsync(
                    productId,
                    cancellationToken);

            return items
                .Select(x => new ItemResponse
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                })
                .ToList();
        }
    }
}
