namespace DeveloperStore.Domain.ValueObjects;

public class Rating
{
    public double Rate { get; private set; }
    public int Count { get; private set; }

    private Rating() { }  // For EF Core

    public Rating(double rate, int count)
    {
        if (rate < 0 || rate > 5)
            throw new ArgumentOutOfRangeException(nameof(rate), "Rate must be between 0 and 5.");
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");

        Rate = rate;
        Count = count;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Rating other)
            return false;

        return Rate == other.Rate && Count == other.Count;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Rate, Count);
    }
}