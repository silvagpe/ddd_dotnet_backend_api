namespace DeveloperStore.Api.Controllers;

using DeveloperStore.Application.Queries;
using DeveloperStore.Application.Handlers;
using DeveloperStore.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using DeveloperStore.Application.Dtos;
using MediatR;
using DeveloperStore.Api.Middleware;

[ApiController]
[Route("api/[controller]")]
public class BranchController : ControllerBase
{
    private readonly IMediator _mediator;

    public BranchController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{branchId:long}")]
    public async Task<IActionResult> GetBranchByIdAsync(long branchId)
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
}