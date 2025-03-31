namespace DeveloperStore.Application.Features.Carts.Dtos;


public class SaleDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Date { get; set; }
    public List<SaleProductDto> Products { get; set; }
}
