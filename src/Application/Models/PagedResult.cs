namespace DeveloperStore.Application.Models;

public class PagedResult<T>
{
    public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
    public long TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public PagedResult(IEnumerable<T> data, long totalItems, int currentPage, int pageSize)
    {
        Data = data;
        TotalItems = totalItems;
        CurrentPage = currentPage;
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
    }
}
