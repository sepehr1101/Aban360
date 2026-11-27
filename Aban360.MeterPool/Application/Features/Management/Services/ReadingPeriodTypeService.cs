using Aban360.Common.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Features.Manegement.Commands.Contracts;
using Aban360.MeterPool.Persistence.Features.Manegement.Queries.Contracts;
using AutoMapper;

namespace Aban360.MeterPool.Application.Features.Management.Services;

public interface IReadingPeriodTypeService
{
    Task CreateAsync(ReadingPeriodTypeCreateDto createDto, CancellationToken cancellationToken);
    Task DeleteAsync(ReadingPeriodTypeDeleteDto deleteDto, CancellationToken cancellationToken);
    Task UpdateAsync(ReadingPeriodTypeUpdateDto updateDto, CancellationToken cancellationToken);
    Task<ICollection<ReadingPeriodTypeGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<ReadingPeriodTypeGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
}

internal sealed class ReadingPeriodTypeService : IReadingPeriodTypeService
{
    private readonly IMapper _mapper;
    private readonly IReadingPeriodTypeCommandService _commands;
    private readonly IHeadquartersAddhoc _headquarterAddhoc;
    private readonly IReadingPeriodTypeQueryService _queries;

    public ReadingPeriodTypeService(IMapper mapper, IReadingPeriodTypeCommandService commands, IHeadquartersAddhoc headquarterAddhoc, IReadingPeriodTypeQueryService queries)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        commands.NotNull(nameof(commands));
        _commands = commands;
        headquarterAddhoc.NotNull(nameof(headquarterAddhoc));
        _headquarterAddhoc = headquarterAddhoc;
        queries.NotNull(nameof(queries));
        _queries = queries;
    }

    public async Task CreateAsync(ReadingPeriodTypeCreateDto createDto, CancellationToken cancellationToken)
    {
        ReadingPeriodType readingPeriodType = _mapper.Map<ReadingPeriodType>(createDto);

        string headquarterTitle=await _headquarterAddhoc.Handle(createDto.HeadquartersId,cancellationToken);
        readingPeriodType.HeadquartersTitle = headquarterTitle;

        await _commands.Add(readingPeriodType);
    }

    public async Task DeleteAsync(ReadingPeriodTypeDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        ReadingPeriodType readingPeriodType = await _queries.Get(deleteDto.Id);
        _commands.Remove(readingPeriodType);
    }

    public async Task UpdateAsync(ReadingPeriodTypeUpdateDto updateDto, CancellationToken cancellationToken)
    {
        ReadingPeriodType readingPeriodType = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, readingPeriodType);
        string headquarterTitle = await _headquarterAddhoc.Handle(updateDto.HeadquartersId, cancellationToken);
        readingPeriodType.HeadquartersTitle = headquarterTitle;
    }

    public async Task<ICollection<ReadingPeriodTypeGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<ReadingPeriodType> readingPeriodType = await _queries.Get();
        return _mapper.Map<ICollection<ReadingPeriodTypeGetDto>>(readingPeriodType);
    }

    public async Task<ReadingPeriodTypeGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        ReadingPeriodType readingPeriodType = await _queries.Get(id);
        return _mapper.Map<ReadingPeriodTypeGetDto>(readingPeriodType);
    }
}
