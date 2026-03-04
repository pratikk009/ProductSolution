using Xunit;
using Moq;
using FluentAssertions;
using Application.Services;
using Application.Interfaces;
using Application.DTOs;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Application.Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IGenericRepository<Product>> _productRepoMock;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productRepoMock = new Mock<IGenericRepository<Product>>();

        _unitOfWorkMock.Setup(u => u.Products)
                       .Returns(_productRepoMock.Object);

        _productService = new ProductService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Product_When_Found()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            ProductName = "Test Product"
        };

        _productRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(product);

        // Act
        var result = await _productService.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.ProductName.Should().Be("Test Product");
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_NotFound()
    {
        // Arrange
        _productRepoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _productService.GetByIdAsync(1);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_Should_Call_Add_And_SaveChanges()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            ProductName = "New Product",
            Stock = 5,
            Price = 100
        };

        _productRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Product>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        await _productService.CreateAsync(dto, "admin");

        // Assert
        _productRepoMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Should_Throw_Exception_When_NotFound()
    {
        // Arrange
        _productRepoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Product?)null);

        // Act
        Func<Task> act = async () => await _productService.DeleteAsync(1);

        // Assert
        await act.Should().ThrowAsync<Exception>()
                 .WithMessage("Product not found");
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_Paged_Result()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = 1, ProductName = "A" },
            new Product { Id = 2, ProductName = "B" }
        };

        _productRepoMock
            .Setup(r => r.GetPagedAsync(1, 10))
            .ReturnsAsync((products, 2));

        var pagination = new PaginationParams
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await _productService.GetAllAsync(pagination);

        // Assert
        result.TotalCount.Should().Be(2);
        result.Data.Count().Should().Be(2);
    }
}