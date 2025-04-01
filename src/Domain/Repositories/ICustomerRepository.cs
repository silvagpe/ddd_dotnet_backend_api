using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken, int page = 1, int pageSize = 10);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken);
    Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken);
    Task UpdateAsync(Customer customer, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
}