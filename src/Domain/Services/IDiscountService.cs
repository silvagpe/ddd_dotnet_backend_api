using DeveloperStore.Domain.ValueObjects;

namespace DeveloperStore.Domain.Services;

    public interface IDiscountService
    {
        Discount CalculateDiscount(int quantity);
        bool IsDiscountAllowed(int quantity);
        bool IsQuantityValid(int quantity);
    }