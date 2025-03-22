namespace DeveloperStore.Api.Controllers;

using DeveloperStore.Application.Queries;
using DeveloperStore.Application.Handlers;
using DeveloperStore.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using DeveloperStore.Application.Dtos;

[ApiController]
[Route("api/[controller]")]
public class BranchController : ControllerBase
{
    public BranchController()
    {        
    }

    [HttpGet("{branchId:long}")]
    public async Task<BranchDto?> GetBranchByIdAsync(long branchId)
    {        
        return null;
    }
}