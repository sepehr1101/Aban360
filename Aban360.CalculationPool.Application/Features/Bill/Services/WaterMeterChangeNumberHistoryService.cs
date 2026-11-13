using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Services;

public interface IWaterMeterChangeNumberHistoryService
{
    Task<ICollection<WaterMeterChangeNumberHistoryGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<WaterMeterChangeNumberHistoryGetDto> GetSingleAsync(long id, CancellationToken cancellationToken);
}

internal sealed class WaterMeterChangeNumberHistoryService : IWaterMeterChangeNumberHistoryService
{
    private readonly IMapper _mapper;
    private readonly IWaterMeterChangeNumberHistoryQueryService _queries;

    public WaterMeterChangeNumberHistoryService(IMapper mapper, IWaterMeterChangeNumberHistoryQueryService queries)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        queries.NotNull(nameof(queries));
        _queries = queries;
    }

    public async Task<ICollection<WaterMeterChangeNumberHistoryGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var readingPeriod = await _queries.Get();
        return _mapper.Map<ICollection<WaterMeterChangeNumberHistoryGetDto>>(readingPeriod);
    }

    public async Task<WaterMeterChangeNumberHistoryGetDto> GetSingleAsync(long id, CancellationToken cancellationToken)
    {
        var readingPeriod = await _queries.Get(id);
        return _mapper.Map<WaterMeterChangeNumberHistoryGetDto>(readingPeriod);
    }
}
