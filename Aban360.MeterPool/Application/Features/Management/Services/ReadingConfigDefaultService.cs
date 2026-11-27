using Aban360.Common.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Features.Management.Commands.Contracts;
using Aban360.MeterPool.Persistence.Features.Management.Queries.Contracts;
using AutoMapper;

namespace Aban360.MeterPool.Application.Features.Management.Services;

public interface IReadingConfigDefaultService
{
    Task<ReadingConfigDefaultGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<ReadingConfigDefaultGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(ReadingConfigDefaultUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(ReadingConfigDefaultDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(ReadingConfigDefaultCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class ReadingConfigDefaultService : IReadingConfigDefaultService
{
    private readonly IMapper _mapper;
    private readonly IReadingConfigDefaultQueryService _queries;
    private readonly IHeadquartersAddhoc _headquartersAddhoc;
    private readonly IReadingConfigDefaultCommandService _commands;

    public ReadingConfigDefaultService(IMapper mapper, IReadingConfigDefaultQueryService queries, IHeadquartersAddhoc headquartersAddhoc, IReadingConfigDefaultCommandService commands)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        queries.NotNull(nameof(queries));
        _queries = queries;
        headquartersAddhoc.NotNull(nameof(headquartersAddhoc));
        _headquartersAddhoc = headquartersAddhoc;
        commands.NotNull(nameof(commands));
        _commands = commands;
    }

    public async Task<ReadingConfigDefaultGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        ReadingConfigDefault readingConfigDefault = await _queries.Get(id);
        return _mapper.Map<ReadingConfigDefaultGetDto>(readingConfigDefault);
    }

    public async Task<ICollection<ReadingConfigDefaultGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<ReadingConfigDefault> readingConfigDefault = await _queries.Get();
        return _mapper.Map<ICollection<ReadingConfigDefaultGetDto>>(readingConfigDefault);
    }

    public async Task UpdateAsync(ReadingConfigDefaultUpdateDto updateDto, CancellationToken cancellationToken)
    {
        ReadingConfigDefault readingConfigDefault = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, readingConfigDefault);
        var headquarterTitle = await _headquartersAddhoc.Handle(updateDto.HeadquartersId, cancellationToken);
        readingConfigDefault.HeadquartersTitle = headquarterTitle;
    }

    public async Task DeleteAsync(ReadingConfigDefaultDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        ReadingConfigDefault readingConfigDefault = await _queries.Get(deleteDto.Id);
        await _commands.Remove(readingConfigDefault);
    }

    public async Task CreateAsync(ReadingConfigDefaultCreateDto createDto, CancellationToken cancellationToken)
    {
        ReadingConfigDefault readingConfigDefault = _mapper.Map<ReadingConfigDefault>(createDto);
        var headquarterTitle = await _headquartersAddhoc.Handle(createDto.HeadquartersId, cancellationToken);
        readingConfigDefault.HeadquartersTitle = headquarterTitle;

        await _commands.Add(readingConfigDefault);
    }
}
