// filepath: d:\projetos\github\ntt_test\src\Infrastructure\Repositories\BranchRepository.cs
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Repositories;
using DeveloperStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Infrastructure.Repositories;

public class BranchRepository : IBranchRepository
{
    private readonly AppDbContext _context;

    public BranchRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Branch?> GetByIdAsync(long id)
    {
        return await _context.Set<Branch>().FindAsync(id);
    }

    public async Task<IEnumerable<Branch>> GetAllAsync(int page = 1, int pageSize = 10)
    {        
        return await _context.Set<Branch>()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<long> GetTotalCountAsync()
    {
        return await _context.Set<Branch>().LongCountAsync();
    }

    public async Task<Branch> AddAsync(Branch branch)
    {
        await _context.Set<Branch>().AddAsync(branch);
        await _context.SaveChangesAsync();
        return branch;
    }

    public async Task UpdateAsync(Branch branch)
    {
        _context.Set<Branch>().Update(branch);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var branch = await GetByIdAsync(id);
        if (branch is null)
            return false;

        _context.Set<Branch>().Remove(branch);
        return await _context.SaveChangesAsync() > 0;
    }
}