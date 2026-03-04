using Application.DTOs;


namespace Application.Interfaces
{
    public interface IItemService
    {
        Task<ItemDto> CreateAsync(CreateItemDto dto);
        Task<IEnumerable<ItemDto>> GetByProductIdAsync(int productId);
        Task DeleteAsync(int id);
    }
}
