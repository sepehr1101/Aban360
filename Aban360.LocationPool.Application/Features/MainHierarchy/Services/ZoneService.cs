using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Services;

public interface IZoneService
{
    Task<ZoneGetDto> GetSingleAsync(int id, CancellationToken cancellationToken);
    Task<int> GetCountAsync(ICollection<int> input,CancellationToken cancellationToken);
    Task<ICollection<ZoneGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(ZoneUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(ZoneDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(ZoneCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class ZoneService : IZoneService
{
    private readonly IZoneQueryService _zoneQueryService;
    private readonly IMapper _mapper;
    private readonly IZoneCommandService _zoneCommandService;

    public ZoneService(IZoneQueryService zoneQueryService, IMapper mapper, IZoneCommandService zoneCommandService)
    {
        zoneQueryService.NotNull(nameof(zoneQueryService));
        _zoneQueryService = zoneQueryService;
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        zoneCommandService.NotNull(nameof(zoneCommandService));
        _zoneCommandService = zoneCommandService;
    }

    public async Task<ZoneGetDto> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        Zone zone = await _zoneQueryService.Get(id);
        return _mapper.Map<ZoneGetDto>(zone);
    }

    public async Task<int> GetCountAsync(ICollection<int> input,CancellationToken cancellationToken)
    {
        return await _zoneQueryService.GetCount(input);
    }

    public async Task<ICollection<ZoneGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<Zone> zone = await _zoneQueryService.Get();
        return _mapper.Map<ICollection<ZoneGetDto>>(zone);
    }

    public async Task UpdateAsync(ZoneUpdateDto updateDto, CancellationToken cancellationToken)
    {
        Zone zone = await _zoneQueryService.Get(updateDto.Id);
        _mapper.Map(updateDto, zone);
    }

    public async Task DeleteAsync(ZoneDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        Zone zone = await _zoneQueryService.Get(deleteDto.Id);
        await _zoneCommandService.Remove(zone);
    }

    public async Task CreateAsync(ZoneCreateDto createDto, CancellationToken cancellationToken)
    {
        Zone zone = _mapper.Map<Zone>(createDto);
        await _zoneCommandService.Add(zone);
    }

}
