using Application.DTOs;

namespace Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<ProductDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateProductDto dto, string user);
    Task UpdateAsync(int id, UpdateProductDto dto, string user);
    Task DeleteAsync(int id);

    Task<PagedResult<ProductDto>> GetAllAsync(PaginationParams paginationParams);
}