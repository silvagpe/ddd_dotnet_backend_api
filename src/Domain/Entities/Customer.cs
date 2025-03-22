using DeveloperStore.Domain.Common;

namespace DeveloperStore.Domain.Entities;

public class Customer : Entity, IAggregateRoot
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public string FullName => $"{FirstName} {LastName}";

    public ICollection<Sale> Sales { get; set; } = new List<Sale>();

    private Customer() { }  // For EF Core

    public Customer(string firstName, string lastName, string email, string phone)
    {
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Phone = phone;
    }

    public void UpdateDetails(string firstName, string lastName, string email, string phone)
    {
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Phone = phone;
    }
}