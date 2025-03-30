using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Repositories;

public interface IBranchRepository
{
    Task<Branch?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<IEnumerable<Branch>> GetAllAsync(CancellationToken cancellationToken, int page = 1, int pageSize = 10);
    Task<long> GetTotalCountAsync(CancellationToken cancellationToken);
    Task<Branch?> AddAsync(Branch branch, CancellationToken cancellationToken);
    Task<Branch?> UpdateAsync(Branch branch, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken);
}