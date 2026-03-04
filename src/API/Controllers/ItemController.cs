using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


[Authorize]
[ApiController]
[Route("api/v1/products/{productId}/items")]
public class ItemsController : ControllerBase
{
    private readonly IItemService _service;

    public ItemsController(IItemService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int productId)
    {
        var items = await _service.GetByProductIdAsync(productId);
        return Ok(items);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(int productId, CreateItemDto dto)
    {
        dto.ProductId = productId;
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { productId }, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}