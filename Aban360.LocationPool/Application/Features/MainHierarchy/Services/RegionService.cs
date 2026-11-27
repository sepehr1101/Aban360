using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Services;

public interface IRegionService
{
    Task CreateAsync(RegionCreateDto createDto, CancellationToken cancellationToken);
    Task DeleteAsync(RegionDeleteDto deleteDto, CancellationToken cancellationToken);
    Task UpdateAsync(RegionUpdateDto updateDto, CancellationToken cancellationToken);
    Task<IEnumerable<NumericDictionary>> GetDictionaryAsync(CancellationToken cancellationToken);
    Task<ICollection<RegionGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<RegionGetDto> GetSingleAsync(int id, CancellationToken cancellationToken);
}

internal sealed class RegionService : IRegionService
{
    private readonly IMapper _mapper;
    private readonly IRegionCommandService _commands;
    private readonly IRegionQueryService _queries;

    public RegionService(IMapper mapper, IRegionCommandService commands, IRegionQueryService queries)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        commands.NotNull(nameof(commands));
        _commands = commands;
        queries.NotNull(nameof(queries));
        _queries = queries;
    }

    public async Task CreateAsync(RegionCreateDto createDto, CancellationToken cancellationToken)
    {
        Region region = _mapper.Map<Region>(createDto);
        await _commands.Add(region);
    }

    public async Task DeleteAsync(RegionDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        Region region = await _queries.Get(deleteDto.Id);
        await _commands.Remove(region);
    }

    public async Task UpdateAsync(RegionUpdateDto updateDto, CancellationToken cancellationToken)
    {
        Region region = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, region);
    }

    public async Task<IEnumerable<NumericDictionary>> GetDictionaryAsync(CancellationToken cancellationToken)
    {
        return await _queries.GetDictionary();
    }

    public async Task<ICollection<RegionGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<Region> region = await _queries.Get();
        return _mapper.Map<ICollection<RegionGetDto>>(region);
    }

    public async Task<RegionGetDto> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        Region region = await _queries.Get(id);
        return _mapper.Map<RegionGetDto>(region);
    }
}
