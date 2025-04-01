using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<IList<Product>> GetByIdsAsync(long[] ids, CancellationToken cancellationToken);
    Task<(IEnumerable<Product> Products, int TotalItems)> GetAllAsync(
        CancellationToken cancellationToken, Dictionary<string, string> fields, string? order, int page = 1, int pageSize = 10);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken);
    Task<Product?> AddAsync(Product product, CancellationToken cancellationToken);
    Task<Product?> UpdateAsync(Product product, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken);
    Task<IEnumerable<string>> GetCategoriesAsync(CancellationToken cancellationToken);
}