using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Repositories;
using DeveloperStore.Infrastructure.Data;
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
    public Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Sale>> GetAllAsync(CancellationToken cancellationToken, int page = 1, int pageSize = 10)
    {
        throw new NotImplementedException();
    }

    public async Task<Sale?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Sales
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

    public Task UpdateAsync(Sale sale, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
