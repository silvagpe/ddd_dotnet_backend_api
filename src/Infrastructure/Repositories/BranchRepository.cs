// filepath: d:\projetos\github\ntt_test\src\Infrastructure\Repositories\BranchRepository.cs
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Repositories;
using DeveloperStore.Infrastructure.Data;
using DeveloperStore.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Infrastructure.Repositories;

public class BranchRepository : IBranchRepository
{
    private readonly AppDbContext _context;

    public BranchRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Branch?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _context.Branches
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<(IEnumerable<Branch> Branches, int TotalItems)> GetAllAsync(CancellationToken cancellationToken, Dictionary<string, string> fields, string? order, int page = 1, int pageSize = 10)
    {
        var query = _context.Branches.AsQueryable();

        query = query.ApplyFilters(fields);
        
        if (!string.IsNullOrEmpty(order))
        {
            query = query.OrderByDynamic(order);
        }
        
        var totalItems = await query.CountAsync(cancellationToken);
        var data = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (data, totalItems);
    }

    public async Task<long> GetTotalCountAsync(CancellationToken cancellationToken)
    {
        return await _context.Branches.LongCountAsync();
    }

    public async Task<Branch?> AddAsync(Branch branch, CancellationToken cancellationToken)
    {
        await _context.Branches.AddAsync(branch, cancellationToken);
        return await _context.SaveChangesAsync(cancellationToken) > 0  ? branch : null;
    }    

    public async Task<Branch?> UpdateAsync(Branch branch, CancellationToken cancellationToken)
    {
        var existingBranch = await _context.Branches.FindAsync([branch.Id], cancellationToken);
        if (existingBranch is null)
        {
            return null;
        }
        _context.Entry(existingBranch).CurrentValues.SetValues(branch);
        return await _context.SaveChangesAsync(cancellationToken) > 0 ? branch : null;
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var branch = await _context.Branches.FindAsync([id], cancellationToken);
        if (branch is not null)
        {
            _context.Branches.Remove(branch);            
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        return false;
    }
}