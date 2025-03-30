namespace DeveloperStore.Api.Controllers;

using DeveloperStore.Application.Queries;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using DeveloperStore.Api.Middleware;
using DeveloperStore.Application.Features.Branches.Queries;
using DeveloperStore.Api.Extensions;

[ApiController]
[Route("api/[controller]")]
public class BranchesController : ControllerBase
{
    private readonly IMediator _mediator;

    public BranchesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{branchId:long}")]
    public async Task<IActionResult> GetBranchesAsync(long branchId)
    {
        var result = await _mediator.Send(new GetBranchByIdQuery(branchId));

        if (result == null)
        {
            throw new ResourceNotFoundException(
                $"Branch with ID {branchId} not found.",
                "Ensure the branch ID is correct and exists in the database."
            );
        }

        return Ok(result);
    }


    [HttpGet]
    public async Task<IActionResult> GetBranchesAsync([FromQuery] Dictionary<string, string>? filters = null)
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
        var query = new GetBranchesQuery(filter.page, filter.pageSize, filter.order, filter.fields);
        var result = await _mediator.Send(query);

        return Ok(result);
    }
}