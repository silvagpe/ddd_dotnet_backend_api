using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Repositories;

public interface ICustomerRepository
{
    Task<Customer> GetByIdAsync(int id);
    Task<IEnumerable<Customer>> GetAllAsync(int page = 1, int pageSize = 10);
    Task<int> GetTotalCountAsync();
    Task<Customer> AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(int id);
}