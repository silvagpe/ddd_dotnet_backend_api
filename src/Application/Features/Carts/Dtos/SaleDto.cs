namespace DeveloperStore.Application.Features.Carts.Dtos;


public class SaleDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Date { get; set; }
    public List<SaleProductDto> Products { get; set; }
}
