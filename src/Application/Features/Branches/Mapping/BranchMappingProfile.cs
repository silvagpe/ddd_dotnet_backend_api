using AutoMapper;

namespace DeveloperStore.Application.Features.Branches.Mapping;


public class BranchMappingProfile : Profile
{
    public BranchMappingProfile()
    {
        CreateMap<Domain.Entities.Branch, Dtos.BranchDto>();
    }
}