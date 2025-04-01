namespace DeveloperStore.Application.Features.Carts.Commands;

using System;
using System.Collections.Generic;
using DeveloperStore.Application.Features.Carts.Dtos;
using MediatR;

public class CreateCartCommand : IRequest<SaleDto?>
{    
    public long UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CartProduct> Products { get; set; } = new List<CartProduct>();
}
