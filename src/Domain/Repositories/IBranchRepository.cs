using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Repositories;

public interface IBranchRepository
{
    Task<Branch?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<(IEnumerable<Branch> Branches, int TotalItems)> GetAllAsync(
        CancellationToken cancellationToken, Dictionary<string, string> fields, string? order, int page = 1, int pageSize = 10);
    Task<long> GetTotalCountAsync(CancellationToken cancellationToken);
    Task<Branch?> AddAsync(Branch branch, CancellationToken cancellationToken);
    Task<Branch?> UpdateAsync(Branch branch, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken);
}