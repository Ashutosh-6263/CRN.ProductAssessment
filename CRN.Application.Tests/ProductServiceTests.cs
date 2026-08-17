using CRN.Application.DTOs.Product;
using CRN.Application.Interfaces;
using CRN.Application.Services;
using CRN.Domain.Entities;
using Moq;

namespace CRN.Application.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _productRepositoryMock = new Mock<IProductRepository>();

            _unitOfWorkMock
                .Setup(x => x.Products)
                .Returns(_productRepositoryMock.Object);

            _productService = new ProductService(
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ProductExists_ReturnsProduct()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                ProductName = "Laptop",
                CreatedBy = "system",
                CreatedOn = DateTime.UtcNow
            };

            _productRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act
            var result = await _productService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Laptop", result.ProductName);
        }

        [Fact]
        public async Task GetByIdAsync_ProductDoesNotExist_ReturnsNull()
        {
            // Arrange
            _productRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    99,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _productService.GetByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_NewProduct_CreatesSuccessfully()
        {
            // Arrange
            var request = new CreateProductRequest
            {
                ProductName = "Laptop"
            };

            _productRepositoryMock
                .Setup(x => x.GetByNameAsync(
                    "Laptop",
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            _productRepositoryMock
                .Setup(x => x.AddAsync(
                    It.IsAny<Product>(),
                    It.IsAny<CancellationToken>()))
                .Callback<Product, CancellationToken>((product, token) =>
                {
                    product.Id = 1;
                })
                .Returns(Task.CompletedTask);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _productService.CreateAsync(
                request,
                "ashutosh");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Laptop", result.ProductName);
            Assert.Equal("ashutosh", result.CreatedBy);

            _productRepositoryMock.Verify(
                x => x.AddAsync(
                    It.IsAny<Product>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_DuplicateProduct_ThrowsException()
        {
            // Arrange
            var request = new CreateProductRequest
            {
                ProductName = "Laptop"
            };

            var existingProduct = new Product
            {
                Id = 1,
                ProductName = "Laptop"
            };

            _productRepositoryMock
                .Setup(x => x.GetByNameAsync(
                    "Laptop",
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _productService.CreateAsync(
                    request,
                    "ashutosh"));

            Assert.Equal(
                "A product with the same name already exists.",
                exception.Message);
        }

        [Fact]
        public async Task UpdateAsync_ProductExists_UpdatesSuccessfully()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                ProductName = "Laptop",
                CreatedBy = "system",
                CreatedOn = DateTime.UtcNow
            };

            var request = new UpdateProductRequest
            {
                ProductName = "Updated Laptop"
            };

            _productRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _productRepositoryMock
                .Setup(x => x.GetByNameAsync(
                    "Updated Laptop",
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _productService.UpdateAsync(
                1,
                request,
                "ashutosh");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Updated Laptop", result.ProductName);
            Assert.Equal("ashutosh", result.ModifiedBy);
            Assert.NotNull(result.ModifiedOn);

            _productRepositoryMock.Verify(
                x => x.Update(product),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ProductDoesNotExist_ReturnsNull()
        {
            // Arrange
            _productRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    99,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            var request = new UpdateProductRequest
            {
                ProductName = "Updated Laptop"
            };

            // Act
            var result = await _productService.UpdateAsync(
                99,
                request,
                "ashutosh");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_DuplicateProductName_ThrowsException()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                ProductName = "Laptop"
            };

            var existingProduct = new Product
            {
                Id = 2,
                ProductName = "Mobile"
            };

            var request = new UpdateProductRequest
            {
                ProductName = "Mobile"
            };

            _productRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _productRepositoryMock
                .Setup(x => x.GetByNameAsync(
                    "Mobile",
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _productService.UpdateAsync(
                    1,
                    request,
                    "ashutosh"));

            Assert.Equal(
                "A product with the same name already exists.",
                exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_ProductExists_ReturnsTrue()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                ProductName = "Laptop"
            };

            _productRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _productService.DeleteAsync(1);

            // Assert
            Assert.True(result);

            _productRepositoryMock.Verify(
                x => x.Delete(product),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ProductDoesNotExist_ReturnsFalse()
        {
            // Arrange
            _productRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    99,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _productService.DeleteAsync(99);

            // Assert
            Assert.False(result);
        }
    }
}
