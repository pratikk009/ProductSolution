using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data.Repositories;

namespace Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IGenericRepository<Product> Products { get; }
    public IGenericRepository<Item> Items { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Products = new GenericRepository<Product>(_context);
        Items = new GenericRepository<Item>(_context);
    }

    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public void Dispose()
        => _context.Dispose();
}