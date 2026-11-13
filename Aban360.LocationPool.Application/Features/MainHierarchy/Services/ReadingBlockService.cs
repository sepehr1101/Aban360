using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Services;

public interface IReadingBlockService
{
    Task<ReadingBlockGetDto> GetSingleAsync(short id,CancellationToken cancellationToken);
    Task<ICollection<ReadingBlockGetDto>> GetAllAsync(int readingBoundId, CancellationToken cancellationToken);
    Task UpdateAsync(ReadingBlockUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(ReadingBlockDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(ReadingBlockCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class ReadingBlockService : IReadingBlockService
{
    private readonly IMapper _mapper;
    private readonly IReadingBlockQueryService _queries;
    private readonly IReadingBlockCommandService _commands;

    public ReadingBlockService(IMapper mapper, IReadingBlockQueryService queries, IReadingBlockCommandService commands)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        queries.NotNull(nameof(queries));
        _queries = queries;
        commands.NotNull(nameof(commands));
        _commands = commands;
    }

    public async Task<ReadingBlockGetDto> GetSingleAsync(short id,CancellationToken cancellationToken)
    {
        ReadingBlock readingBound = await _queries.Get(id);
        return _mapper.Map<ReadingBlockGetDto>(readingBound);
    }

    public async Task<ICollection<ReadingBlockGetDto>> GetAllAsync(int readingBoundId, CancellationToken cancellationToken)
    {
        ICollection<ReadingBlock> readingBlocks = await _queries.GetByBoundId(readingBoundId);
        return _mapper.Map<ICollection<ReadingBlockGetDto>>(readingBlocks);
    }

    public async Task UpdateAsync(ReadingBlockUpdateDto updateDto, CancellationToken cancellationToken)
    {
        ReadingBlock readingBlock = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, readingBlock);
    }

    public async Task DeleteAsync(ReadingBlockDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        ReadingBlock readingBlock = await _queries.Get(deleteDto.Id);
        await _commands.Remove(readingBlock);
    }

    public async Task CreateAsync(ReadingBlockCreateDto createDto, CancellationToken cancellationToken)
    {
        var readingBlock = _mapper.Map<ReadingBlock>(createDto);
        await _commands.Add(readingBlock);
    }
}
