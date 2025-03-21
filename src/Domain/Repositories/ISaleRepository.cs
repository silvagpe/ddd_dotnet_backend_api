using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Repositories;

public interface ISaleRepository
{
    Task<Sale> GetByIdAsync(int id);
    Task<Sale> GetBySaleNumberAsync(string saleNumber);
    Task<IEnumerable<Sale>> GetAllAsync(int page = 1, int pageSize = 10);
    Task<int> GetTotalCountAsync();
    Task<Sale> AddAsync(Sale sale);
    Task UpdateAsync(Sale sale);
    Task DeleteAsync(int id);
}