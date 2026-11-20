using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Request.Services;
public interface IRequestDictionariesService
{
    Task<IEnumerable<NumericDictionary>> ServiceGroupGetAllAsync(bool isReturn, CancellationToken cancellationToken);
    Task<IEnumerable<NumericDictionary>> TrackingResultGetAllAsync(CancellationToken cancellationToken);
}

internal sealed class RequestDictionariesService : IRequestDictionariesService
{
    public RequestDictionariesService(IT100QueryService t100QueryService, IT64QueryService trackingResultQueryService)
    {
        _t100QueryService = t100QueryService;
        _t100QueryService.NotNull(nameof(t100QueryService));
        _trackingResultQueryService = trackingResultQueryService;
        _trackingResultQueryService.NotNull(nameof(trackingResultQueryService));
    }

    private readonly IT100QueryService _t100QueryService;
    public async Task<IEnumerable<NumericDictionary>> ServiceGroupGetAllAsync(bool isReturn, CancellationToken cancellationToken)
    {
        return await _t100QueryService.Get(isReturn);
    }

    private readonly IT64QueryService _trackingResultQueryService;
    public async Task<IEnumerable<NumericDictionary>> TrackingResultGetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<NumericDictionary> trackingResultsDictionary = await _trackingResultQueryService.Get();
        return trackingResultsDictionary;
    }
}
