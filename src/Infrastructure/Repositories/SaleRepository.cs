using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Repositories;
using DeveloperStore.Infrastructure.Data;
using DeveloperStore.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Infrastructure.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly AppDbContext _context;

    public SaleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Sale> AddAsync(Sale sale, CancellationToken cancellationToken)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            if (_context.Entry(sale.Branch).State == EntityState.Detached)
            {
                _context.Attach(sale.Branch);
            }
            if (_context.Entry(sale.Customer).State == EntityState.Detached)
            {
                _context.Attach(sale.Customer);
            }
            foreach (var item in sale.Items)
            {
                if (_context.Entry(item.Product).State == EntityState.Detached)
                {
                    _context.Attach(item.Product);
                }
            }

            var addedSale = (await _context.Sales.AddAsync(sale, cancellationToken)).Entity;
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return addedSale;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var sale = await _context.Sales.FindAsync([id], cancellationToken);
        if (sale is not null)
        {
            _context.Sales.Remove(sale);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
        return false;
    }

    public async Task<(IEnumerable<Sale> Sales, int TotalItems)> GetAllAsync(CancellationToken cancellationToken, Dictionary<string, string> fields, string? order, int page = 1, int pageSize = 10)
    {
        var query = _context.Sales.AsQueryable();

        query = query.ApplyFilters(fields);
        
        if (!string.IsNullOrEmpty(order))
        {
            query = query.OrderByDynamic(order);
        }
        
        var totalItems = await query.CountAsync(cancellationToken);
        var data = await query
            // .Include(s => s.Branch)
            // .Include(s => s.Customer)
            .Include(s => s.Items) 
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (data, totalItems);
    }

    public async Task<Sale?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _context.Sales
            .Include(s => s.Branch)
            .Include(s => s.Customer)
            .Include(s => s.Items)            
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public Task<Sale> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetTotalCountAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            if (_context.Entry(sale.Branch).State == EntityState.Detached)
            {
                _context.Attach(sale.Branch);
            }
            if (_context.Entry(sale.Customer).State == EntityState.Detached)
            {
                _context.Attach(sale.Customer);
            }
            foreach (var item in sale.Items)
            {
                if (_context.Entry(item.Product).State == EntityState.Detached)
                {
                    _context.Attach(item.Product);
                }
            }

            _context.Sales.Update(sale);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return sale;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
