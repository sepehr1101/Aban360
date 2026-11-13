using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Features.UiElement.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.AccessTree.Services;

public interface IEndpointService
{
    Task<EndpointGetDto> GetSingleAsync(int id, CancellationToken cancellationToken);
    Task<ICollection<EndpointGetDto>> GetAllAsync( CancellationToken cancellationToken);
    Task UpdateAsync(EndpointUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(EndpointDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(EndpointCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class EndpointService : IEndpointService
{
    private readonly IMapper _mapper;
    private readonly IEndpointQueryService _endpointQueryService;
    private readonly IEndpointCommandService _endpointCommandService;

    public EndpointService(IMapper mapper, IEndpointQueryService endpointQueryService, IEndpointCommandService endpointCommandService)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        endpointQueryService.NotNull(nameof(endpointQueryService));
        _endpointQueryService = endpointQueryService;
        endpointCommandService.NotNull(nameof(endpointCommandService));
        _endpointCommandService = endpointCommandService;
    }

    public async Task<EndpointGetDto> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        Endpoint endpoint = await _endpointQueryService.GetInclude(id);
        return _mapper.Map<EndpointGetDto>(endpoint);
    }

    public async Task<ICollection<EndpointGetDto>> GetAllAsync( CancellationToken cancellationToken)
    {
        ICollection<Endpoint> endpoint = await _endpointQueryService.GetInclude();
        return _mapper.Map<ICollection<EndpointGetDto>>(endpoint);
    }

    public async Task UpdateAsync(EndpointUpdateDto updateDto, CancellationToken cancellationToken)
    {
        Endpoint endpoint = await _endpointQueryService.Get(updateDto.Id);
        _mapper.Map(updateDto, endpoint);
    }

    public async Task DeleteAsync(EndpointDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        Endpoint endpoint = await _endpointQueryService.Get(deleteDto.Id);
        _endpointCommandService.Remove(endpoint);
    }

    public async Task CreateAsync(EndpointCreateDto createDto, CancellationToken cancellationToken)
    {
        Endpoint endpoint = _mapper.Map<Endpoint>(createDto);
        endpoint.IsActive=true;
        await _endpointCommandService.Add(endpoint);
    }

}
