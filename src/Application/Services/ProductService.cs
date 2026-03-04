using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _unitOfWork.Products.GetAllAsync();
        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            ProductName = p.ProductName
        });
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null) return null;

        return new ProductDto
        {
            Id = product.Id,
            ProductName = product.ProductName
        };
    }

    public async Task<int> CreateAsync(CreateProductDto dto, string user)
    {
        var product = new Product
        {
            ProductName = dto.ProductName,
            Stock = dto.Stock,
            Price = dto.Price,
            CreatedBy = user,
            CreatedOn = DateTime.UtcNow
        };

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return product.Id;
    }

    public async Task UpdateAsync(int id, UpdateProductDto dto, string user)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id)
            ?? throw new Exception("Product not found");

        product.ProductName = dto.ProductName;
        product.Stock = dto.Stock;
        product.Price = dto.Price;
        product.ModifiedBy = user;
        product.ModifiedOn = DateTime.UtcNow;

        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id)
            ?? throw new Exception("Product not found");

        _unitOfWork.Products.Delete(product);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PagedResult<ProductDto>>
     GetAllAsync(PaginationParams paginationParams)
    {
        var (products, totalCount) =
            await _unitOfWork.Products.GetPagedAsync(
                paginationParams.PageNumber,
                paginationParams.PageSize);

        var mappedProducts = products.Select(p => new ProductDto
        {
            Id = p.Id,
            ProductName = p.ProductName
        });

        return new PagedResult<ProductDto>(
            mappedProducts,
            totalCount,
            paginationParams.PageNumber,
            paginationParams.PageSize
        );
    }
}