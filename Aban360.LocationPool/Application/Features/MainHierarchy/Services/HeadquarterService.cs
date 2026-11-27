using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Services;

public interface IHeadquarterService
{
    Task CreateAsync(HeadquarterCreateDto createDto, CancellationToken cancellationToken);
    Task DeleteAsync(HeadquarterDeleteDto deleteDto, CancellationToken cancellationToken);
    Task UpdateAsync(HeadquarterUpdateDto updateDto, CancellationToken cancellationToken);
    Task<ICollection<HeadquarterGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<HeadquarterGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
}

internal sealed class HeadquarterService : IHeadquarterService
{
    private readonly IMapper _mapper;
    private readonly IHeadquarterCommandService _commands;
    private readonly IHeadquarterQueryService _queries;

    public HeadquarterService(IMapper mapper, IHeadquarterCommandService commands, IHeadquarterQueryService queries)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        commands.NotNull(nameof(commands));
        _commands = commands;
        queries.NotNull(nameof(queries));
        _queries = queries;
    }

    public async Task CreateAsync(HeadquarterCreateDto createDto, CancellationToken cancellationToken)
    {
        Headquarters headquarter = _mapper.Map<Headquarters>(createDto);
        await _commands.Add(headquarter);
    }

    public async Task DeleteAsync(HeadquarterDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        Headquarters headquarter = await _queries.Get(deleteDto.Id);
        await _commands.Remove(headquarter);
    }

    public async Task UpdateAsync(HeadquarterUpdateDto updateDto, CancellationToken cancellationToken)
    {
        Headquarters headquarter = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, headquarter);
    }

    public async Task<ICollection<HeadquarterGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<Headquarters> headquarter = await _queries.Get();
        return _mapper.Map<ICollection<HeadquarterGetDto>>(headquarter);
    }

    public async Task<HeadquarterGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        Headquarters headquarter = await _queries.Get(id);
        return _mapper.Map<HeadquarterGetDto>(headquarter);
    }
}
