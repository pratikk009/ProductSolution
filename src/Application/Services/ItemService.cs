using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;

public class ItemService : IItemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ItemService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ItemDto> CreateAsync(CreateItemDto dto)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
        if (product == null)
            throw new NotFoundException("Product not found");

        var item = new Item
        {
            ProductId = dto.ProductId,
            Quantity = dto.Quantity
        };

        await _unitOfWork.Items.AddAsync(item);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ItemDto>(item);
    }

    public async Task<IEnumerable<ItemDto>> GetByProductIdAsync(int productId)
    {
        var items = await _unitOfWork.Items
            .FindAsync(x => x.ProductId == productId);

        return _mapper.Map<IEnumerable<ItemDto>>(items);
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _unitOfWork.Items.GetByIdAsync(id);
        if (item == null)
            throw new NotFoundException("Item not found");

        _unitOfWork.Items.Remove(item);
        await _unitOfWork.SaveChangesAsync();
    }
}