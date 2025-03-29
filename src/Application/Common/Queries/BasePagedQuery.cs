namespace DeveloperStore.Application.Common.Queries;

public class BasePagedQuery
{
    public int Page { get; }
    public int PageSize { get; }
    public string? Order { get; }
    public Dictionary<string, string> Fields { get; }

    public BasePagedQuery(int page, int pageSize, string? order, Dictionary<string, string> fields)
    {
        Page = page;
        PageSize = pageSize;
        Order = order;
        Fields = fields;
    }
}