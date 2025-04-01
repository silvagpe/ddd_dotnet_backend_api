namespace DeveloperStore.Application.Features.Carts.Commands;

using System;
using System.Collections.Generic;
using DeveloperStore.Application.Features.Carts.Dtos;
using MediatR;

public class UpdateCartCommand : IRequest<SaleDto?>
{   
    public long Id { get; set; } 
    public long UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CartProduct> Products { get; set; } = new List<CartProduct>();
}
