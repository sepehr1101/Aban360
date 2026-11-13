using Aban360.Common.Extensions;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Features.Manegement.Commands.Contracts;
using Aban360.MeterPool.Persistence.Features.Manegement.Queries.Contracts;
using AutoMapper;

namespace Aban360.MeterPool.Application.Features.Management.Services;

public interface IReadingPeriodService
{
    Task CreateAsync(ReadingPeriodCreateDto createDto, CancellationToken cancellationToken);
    Task DeleteAsync(ReadingPeriodDeleteDto deleteDto, CancellationToken cancellationToken);
    Task UpdateAsync(ReadingPeriodUpdateDto updateDto, CancellationToken cancellationToken);
    Task<ICollection<ReadingPeriodGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<ReadingPeriodGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
}

internal sealed class ReadingPeriodService : IReadingPeriodService
{
    private readonly IMapper _mapper;
    private readonly IReadingPeriodCommandService _commands;
    private readonly IReadingPeriodQueryService _queries;

    public ReadingPeriodService(IMapper mapper, IReadingPeriodCommandService commands, IReadingPeriodQueryService queries)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        commands.NotNull(nameof(commands));
        _commands = commands;
        queries.NotNull(nameof(queries));
        _queries = queries;
    }

    public async Task CreateAsync(ReadingPeriodCreateDto createDto, CancellationToken cancellationToken)
    {
        ReadingPeriod readingPeriod = _mapper.Map<ReadingPeriod>(createDto);
        await _commands.Add(readingPeriod);
    }

    public async Task DeleteAsync(ReadingPeriodDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        ReadingPeriod readingPeriod = await _queries.Get(deleteDto.Id);
        _commands.Remove(readingPeriod);
    }

    public async Task UpdateAsync(ReadingPeriodUpdateDto updateDto, CancellationToken cancellationToken)
    {
        ReadingPeriod readingPeriod = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, readingPeriod);
    }

    public async Task<ICollection<ReadingPeriodGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<ReadingPeriod> readingPeriod = await _queries.Get();
        return _mapper.Map<ICollection<ReadingPeriodGetDto>>(readingPeriod);
    }

    public async Task<ReadingPeriodGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        ReadingPeriod readingPeriod = await _queries.Get(id);
        return _mapper.Map<ReadingPeriodGetDto>(readingPeriod);
    }
}
