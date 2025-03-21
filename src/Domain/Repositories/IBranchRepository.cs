using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Repositories;

public interface IBranchRepository
{
    Task<Branch> GetByIdAsync(int id);
    Task<IEnumerable<Branch>> GetAllAsync(int page = 1, int pageSize = 10);
    Task<int> GetTotalCountAsync();
    Task<Branch> AddAsync(Branch branch);
    Task UpdateAsync(Branch branch);
    Task DeleteAsync(int id);
}