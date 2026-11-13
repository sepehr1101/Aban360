using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Services;

public interface ICordinalDirectionService
{
    Task<CordinalDirectionGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task CreateAsync(CordinalDirectionCreateDto createDto, CancellationToken cancellationToken);
    Task DeleteAsync(CordinalDirectionDeleteDto deleteDto, CancellationToken cancellationToken);
    Task UpdateAsync(CordinalDirectionUpdateDto updateDto, CancellationToken cancellationToken);
    Task<ICollection<CordinalDirectionGetDto>> GetAllAsync(CancellationToken cancellationToken);
}

internal sealed class CordinalDirectionService : ICordinalDirectionService
{
    private readonly IMapper _mapper;
    private readonly ICordinalDirectionCommandService _commands;
    private readonly ICordinalDirectionQueryService _queries;

    public CordinalDirectionService(IMapper mapper, ICordinalDirectionCommandService commands, ICordinalDirectionQueryService queries)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        commands.NotNull(nameof(commands));
        _commands = commands;
        queries.NotNull(nameof(queries));
        _queries = queries;
    }

    public async Task<CordinalDirectionGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var item = await _queries.Get(id);
        return _mapper.Map<CordinalDirectionGetDto>(item);
    }

    public async Task CreateAsync(CordinalDirectionCreateDto createDto, CancellationToken cancellationToken)
    {
        CordinalDirection cordinalDirection = _mapper.Map<CordinalDirection>(createDto);
        await _commands.Add(cordinalDirection);
    }

    public async Task DeleteAsync(CordinalDirectionDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        CordinalDirection cordinalDirection =await _queries.Get(deleteDto.Id);
        await _commands.Remove(cordinalDirection);
    }

    public async Task UpdateAsync(CordinalDirectionUpdateDto updateDto, CancellationToken cancellationToken)
    {
        CordinalDirection cordinalDirection = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, cordinalDirection);
    }

    public async Task<ICollection<CordinalDirectionGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<CordinalDirection> cordinalDirection = await _queries.Get();
        return _mapper.Map<ICollection<CordinalDirectionGetDto>>(cordinalDirection);
    }
}
