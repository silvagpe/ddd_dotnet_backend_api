namespace DeveloperStore.Api.Controllers;


using Microsoft.AspNetCore.Mvc;
using MediatR;
using DeveloperStore.Api.Middleware;
using DeveloperStore.Api.Extensions;
using DeveloperStore.Application.Features.Carts.Commands;
using Namespace.Application.Features.Carts.Queries;
using DeveloperStore.Application.Features.Carts.Queries;

[ApiController]
[Route("api/[controller]")]
public class CartsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetCartAsync(long id)
    {
        var result = await _mediator.Send(new GetCartByIdQuery(id));

        if (result == null)
        {
            throw new ResourceNotFoundException(
                $"Product with ID {id} not found.",
                "Ensure the branch ID is correct and exists in the database."
            );
        }

        return Ok(result);
    }


    [HttpGet]
    public async Task<IActionResult> GetCartsAsync([FromQuery] Dictionary<string, string>? filters = null)
    {
        var filter = filters.ValidateFilters();

        if (!filter.IsPageValid())
        {
            return BadRequest(new
            {
                type = "ValidationError",
                error = "Invalid pagination parameters",
                detail = "Both _page and _pageSize must be greater than 0"
            });
        }
        var query = new GetCartsQuery(filter.page, filter.pageSize, filter.order, filter.fields);
        var result = await _mediator.Send(query);

        return Ok(result);        
    }

    [HttpPost]
    public async Task<IActionResult> PostCartAsync(CreateCartCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);        
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> PutCartAsync([FromRoute] long id, [FromBody] UpdateCartCommand command)
    {        
        command.Id = id;        
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteCartAsync(long id)
    {
        var command = new DeleteCartCommand(id);
        var result = await _mediator.Send(command);
        return Ok(new {message = result});
    }
    
}