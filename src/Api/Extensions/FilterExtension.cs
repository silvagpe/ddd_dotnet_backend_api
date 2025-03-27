using System.Web;

namespace DeveloperStore.Api.Extensions;

public record Filter(int page, int pageSize, string? order, Dictionary<string, string> fields);
public static class FilterExtension{

    private const string PAGE = "_page";
    private const string PAGE_SIZE = "_size";
    private const string ORDER = "_order";

    public static Filter ValidateFilters(this Dictionary<string, string>? filters)
    {
        if (filters is null){            
            return new Filter(1, 10, null, new Dictionary<string, string>());
        }
        int _page = 1;
        int _size = 10;
        string? _order = null;

        if (filters.TryGetValue(PAGE, out string? page)){
            if (!int.TryParse(page, out _page)){
                _page = 1;
            }
            filters.Remove(PAGE);
        };
        if(filters.TryGetValue(PAGE_SIZE, out string? pageSize)){
            if (!int.TryParse(pageSize, out _size)){
                _size = 10;
            }
            filters.Remove(PAGE_SIZE);
        };
        if(filters.TryGetValue(ORDER, out string? order)){
            _order = HttpUtility.UrlDecode(order);
            if (_order.Contains("\"")){
                _order = _order.Replace("\"", "");
            }
            filters.Remove(ORDER);
        };
        return new Filter(_page, _size, _order, filters);
    }   

    public static bool IsPageValid(this Filter filter){
        return filter.page > 0 || filter.pageSize > 0;
    }
}