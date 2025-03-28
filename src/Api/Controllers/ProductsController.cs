namespace DeveloperStore.Api.Controllers;


using DeveloperStore.Application.Queries;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using DeveloperStore.Api.Middleware;
using DeveloperStore.Application.Features.Branches.Queries;
using DeveloperStore.Api.Extensions;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{productId:long}")]
    public async Task<IActionResult> GetProductAsync(long productId)
    {
        // var result = await _mediator.Send(new GetBranchByIdQuery(productId));

        // if (result == null)
        // {
        //     throw new ResourceNotFoundException(
        //         $"Product with ID {productId} not found.",
        //         "Ensure the branch ID is correct and exists in the database."
        //     );
        // }

        // return Ok(result);

        return Ok();
    }


    [HttpGet]
    public async Task<IActionResult> GetProducsAsync([FromQuery] Dictionary<string, string>? filters = null)
    {
        // var filter = filters.ValidateFilters();

        // if (!filter.IsPageValid())
        // {
        //     return BadRequest(new
        //     {
        //         type = "ValidationError",
        //         error = "Invalid pagination parameters",
        //         detail = "Both _page and _pageSize must be greater than 0"
        //     });
        // }
        // var query = new GetBranchesQuery(filter.page, filter.pageSize, filter.order, filter.fields);
        // var result = await _mediator.Send(query);

        // return Ok(result);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> PostProductAsync(long productId)
    {
        return Ok();
    }

    [HttpPut("{productId:long}")]
    public async Task<IActionResult> PutProductAsync(long productId)
    {
        return Ok();
    }

    [HttpDelete("{productId:long}")]
    public async Task<IActionResult> DeleteProductAsync(long productId)
    {
        return Ok();
    }
    
    [HttpGet("categories")]
    public async Task<IActionResult> GetCategoriesAsync()
    {
        return Ok();
    }

    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetProductsByCategoryAsync(string category)
    {
        return Ok();
    }
}