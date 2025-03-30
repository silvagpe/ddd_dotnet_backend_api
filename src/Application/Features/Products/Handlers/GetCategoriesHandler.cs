using AutoMapper;
using DeveloperStore.Application.Features.Products.Commands;
using DeveloperStore.Application.Features.Products.Dtos;
using DeveloperStore.Application.Features.Products.Queries;
using DeveloperStore.Domain.Repositories;
using MediatR;

namespace DeveloperStore.Application.Features.Products.Handlers;

public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, CategoriesDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetCategoriesHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;        
    }

    public async Task<CategoriesDto> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {                
        var categories = await _productRepository.GetCategoriesAsync(cancellationToken);
        return new CategoriesDto(categories);
    }
}