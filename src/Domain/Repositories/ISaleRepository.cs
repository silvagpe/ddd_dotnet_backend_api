using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Repositories;

public interface ISaleRepository
{
    Task<Sale?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<Sale> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken);
    Task<IEnumerable<Sale>> GetAllAsync(CancellationToken cancellationToken, int page = 1, int pageSize = 10);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken);
    Task<Sale> AddAsync(Sale sale, CancellationToken cancellationToken);
    Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
}