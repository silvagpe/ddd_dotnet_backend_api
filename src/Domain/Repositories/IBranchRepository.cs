using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Repositories;

public interface IBranchRepository
{
    Task<Branch?> GetByIdAsync(long id);
    Task<IEnumerable<Branch>> GetAllAsync(int page = 1, int pageSize = 10);
    Task<long> GetTotalCountAsync();
    Task<Branch> AddAsync(Branch branch);
    Task UpdateAsync(Branch branch);
    Task<bool> DeleteAsync(long id);
}