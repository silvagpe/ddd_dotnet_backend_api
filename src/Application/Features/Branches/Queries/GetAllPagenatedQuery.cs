using DeveloperStore.Application.Dtos;
using DeveloperStore.Application.Models; // Import the new PagedResult class
using MediatR;

namespace DeveloperStore.Application.Features.Branches.Queries;

public record GetAllPagenatedQuery(int Page = 1, int PageSize = 10) : IRequest<PagedResult<BranchDto>>;