using DeveloperStore.Domain.Common;

namespace DeveloperStore.Domain.Entities;

public class Branch : Entity, IAggregateRoot
{
    public string Name { get; private set; }
    public string Address { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string ZipCode { get; private set; }
    public string Phone { get; private set; }

    public ICollection<Sale> Sales { get; set; } = new List<Sale>();

    private Branch() { }  // For EF Core

    public Branch(string name, string address, string city, string state, string zipCode, string phone)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Address = address ?? throw new ArgumentNullException(nameof(address));
        City = city ?? throw new ArgumentNullException(nameof(city));
        State = state ?? throw new ArgumentNullException(nameof(state));
        ZipCode = zipCode ?? throw new ArgumentNullException(nameof(zipCode));
        Phone = phone;
    }

    public void UpdateDetails(string name, string address, string city, string state, string zipCode, string phone)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Address = address ?? throw new ArgumentNullException(nameof(address));
        City = city ?? throw new ArgumentNullException(nameof(city));
        State = state ?? throw new ArgumentNullException(nameof(state));
        ZipCode = zipCode ?? throw new ArgumentNullException(nameof(zipCode));
        Phone = phone;
    }
}