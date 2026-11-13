using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Services;

public interface IReadingBoundService
{
    Task<ReadingBoundGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<ReadingBoundGetDto>> GetByZoneIdAsync(int zoneId, CancellationToken cancellationToken);
    Task<ICollection<ReadingBoundGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(ReadingBoundUpdateDto updateDto, CancellationToken cancellationToken);
    Task ValidateAsync(ReadingBoundIsValidUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(ReadingBoundDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(ReadingBoundCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class ReadingBoundService : IReadingBoundService
{
    private readonly IMapper _mapper;
    private readonly IReadingBoundQueryService _readingBoundQueryService;
    private readonly IReadingBlockQueryService _readingBlockQueryService;
    private readonly IReadingBoundCommandService _readingBoundCommandService;

    public ReadingBoundService(IMapper mapper, IReadingBoundQueryService readingBoundQueryService, IReadingBlockQueryService readingBlockQueryService, IReadingBoundCommandService readingBoundCommandService)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        readingBoundQueryService.NotNull(nameof(readingBoundQueryService));
        _readingBoundQueryService = readingBoundQueryService;
        readingBlockQueryService.NotNull(nameof(readingBlockQueryService));
        _readingBlockQueryService = readingBlockQueryService;
        readingBoundCommandService.NotNull(nameof(readingBoundCommandService));
        _readingBoundCommandService = readingBoundCommandService;
    }

    public async Task<ReadingBoundGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        ReadingBound readingBound = await _readingBoundQueryService.Get(id);
        return _mapper.Map<ReadingBoundGetDto>(readingBound);
    }

    public async Task<ICollection<ReadingBoundGetDto>> GetByZoneIdAsync(int zoneId, CancellationToken cancellationToken)
    {
        ICollection<ReadingBound> readingBound = await _readingBoundQueryService.GetByZoneId(zoneId);
        return _mapper.Map<ICollection<ReadingBoundGetDto>>(readingBound);
    }

    public async Task<ICollection<ReadingBoundGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<ReadingBound> readingBound = await _readingBoundQueryService.Get();
        return _mapper.Map<ICollection<ReadingBoundGetDto>>(readingBound);
    }

    public async Task UpdateAsync(ReadingBoundUpdateDto updateDto, CancellationToken cancellationToken)
    {
        ReadingBound readingBound = await _readingBoundQueryService.Get(updateDto.Id);
        _mapper.Map(updateDto, readingBound);
    }

    public async Task ValidateAsync(ReadingBoundIsValidUpdateDto updateDto, CancellationToken cancellationToken)
    {
        ReadingBound readingBound = await _readingBoundQueryService.Get(updateDto.Id);
        readingBound.IsValid = false;
    
        ICollection<ReadingBlock> readingBlocks = await _readingBlockQueryService.GetByBoundId(readingBound.Id);
        if (!readingBlocks.Any())
        {
            throw new ReadingException(ExceptionLiterals.ReadingBlockNotFound);
        }
        ICollection<ReadingBlock> readingBlockOrdered = readingBlocks.OrderBy(block => block.FromReadingNumber).ToList();
        string fromReadingNumber = readingBlockOrdered?.FirstOrDefault()?.FromReadingNumber ?? string.Empty;
        string toReadingNumber = readingBlockOrdered?.OrderByDescending(block => block.FromReadingNumber)?.FirstOrDefault()?.ToReadingNumber ?? string.Empty;
        if (string.IsNullOrWhiteSpace(fromReadingNumber) || string.IsNullOrWhiteSpace(toReadingNumber))
        {
            throw new ReadingException(ExceptionLiterals.ReadingBlockNotFound);
        }
    
        string perToReadingNumber = "";
        for (int i = 0; i < (readingBlockOrdered?.Count() ?? 0); i++)
        {
            ReadingBlock? block = readingBlockOrdered.ElementAt(i);
    
            if (fromReadingNumber.Length != block.FromReadingNumber.Length || fromReadingNumber.Length != block.ToReadingNumber.Length)
            {
                throw new ReadingException(ExceptionLiterals.InvalidBlocksReadingNumberChars);
            }
    
            if (i == 0 && readingBound.FromReadingNumber != block.FromReadingNumber)
            {
                throw new ReadingException(ExceptionLiterals.InvalidBoundAndBlockStartReadingNumber);
            }
            if (i > 0)
            {
                if (Convert.ToInt32(block.FromReadingNumber) != Convert.ToInt32(perToReadingNumber) + 1)
                {
                    throw new ReadingException(string.Format(ExceptionLiterals.InvalidStartReadingNumber, block.Title));
                }
            }
    
            perToReadingNumber = block.ToReadingNumber;
        }
    
        readingBound.IsValid = true;
    
    }

    public async Task DeleteAsync(ReadingBoundDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        ReadingBound readingBound = await _readingBoundQueryService.Get(deleteDto.Id);
        await _readingBoundCommandService.Remove(readingBound);
    }

    public async Task CreateAsync(ReadingBoundCreateDto createDto, CancellationToken cancellationToken)
    {
        ReadingBound readingBound = _mapper.Map<ReadingBound>(createDto);
        await _readingBoundCommandService.Add(readingBound);
    }

}
