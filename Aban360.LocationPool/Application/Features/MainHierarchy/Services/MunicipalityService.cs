using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Services;

public interface IMunicipalityService
{
    Task CreateAsync(MunicipalityCreateDto createDto, CancellationToken cancellationToken);
    Task DeleteAsync(MunicipalityDeleteDto deleteDto, CancellationToken cancellationToken);
    Task UpdateAsync(MunicipalityUpdateDto updateDto, CancellationToken cancellationToken);
    Task<IEnumerable<NumericDictionary>> GetByZoneIdAsync(int zoneId,CancellationToken cancellationToken);
    Task<ICollection<MunicipalityGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<MunicipalityGetDto> GetSingleAsync(int id, CancellationToken cancellationToken);
}

internal sealed class MunicipalityService : IMunicipalityService
{
    private readonly IMapper _mapper;
    private readonly IMunicipalityCommandService _commands;
    private readonly IMunicipalityQueryService _queries;

    public MunicipalityService(IMapper mapper, IMunicipalityCommandService commands, IMunicipalityQueryService queries)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        commands.NotNull(nameof(commands));
        _commands = commands;
        queries.NotNull(nameof(queries));
        _queries = queries;
    }

    public async Task CreateAsync(MunicipalityCreateDto createDto, CancellationToken cancellationToken)
    {
        Municipality municipality = _mapper.Map<Municipality>(createDto);
        await _commands.Add(municipality);
    }

    public async Task DeleteAsync(MunicipalityDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        Municipality municipality = await _queries.Get(deleteDto.Id);
        await _commands.Remove(municipality);
    }

    public async Task UpdateAsync(MunicipalityUpdateDto updateDto, CancellationToken cancellationToken)
    {
        Municipality municipality = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, municipality);
    }

    public async Task<IEnumerable<NumericDictionary>> GetByZoneIdAsync(int zoneId,CancellationToken cancellationToken)
    {
        return  await _queries.GetByZoneId(zoneId);
    }

    public async Task<ICollection<MunicipalityGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<Municipality> municipality = await _queries.Get();
        return _mapper.Map<ICollection<MunicipalityGetDto>>(municipality);
    }

    public async Task<MunicipalityGetDto> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        Municipality municipality = await _queries.Get(id);
        return _mapper.Map<MunicipalityGetDto>(municipality);
    }
}
