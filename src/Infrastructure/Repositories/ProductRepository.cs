using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Repositories;
using DeveloperStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> AddAsync(Product product, CancellationToken cancellationToken)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        return await _context.SaveChangesAsync(cancellationToken) > 0 ? product : null;
    }

    public async Task<Product?> UpdateAsync(Product product, CancellationToken cancellationToken)
    {
        _context.Products.Update(product);
        return await _context.SaveChangesAsync(cancellationToken) > 0 ? product : null;
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync([id], cancellationToken);
        if (product is not null)
        {
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
        return false;
    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken, int page = 1, int pageSize = 10)
    {
        return await _context.Products
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken)
    {
        return await _context.Products.CountAsync(cancellationToken);
    }

    public async Task<IEnumerable<string>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        return await _context.Products        
            .AsNoTracking()
            .Select(p => p.Category)
            .Distinct()
            .ToListAsync(cancellationToken);
    }
}