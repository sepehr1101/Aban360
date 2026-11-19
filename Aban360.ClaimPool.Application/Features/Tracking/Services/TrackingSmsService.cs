using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.ClaimPool.Persistence.Features.Sms.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Tracking.Services;
public interface ITrackingSmsService
{
    Task<ReportOutput<TrackingSmsHeaderOutputDto, TrackingSmsDataOutputDto>> GetByTrackIdAsync(Guid trackId, CancellationToken cancellationToken);
    Task<TrackingSmsDataOutputDto> GetByQueueIdAsync(Guid id, CancellationToken cancellationToken);
}

internal sealed class TrackingSmsService : ITrackingSmsService
{
    private readonly IQueueQueryService _smsQueryService;
    public TrackingSmsService(IQueueQueryService smsQueryService)
    {
        _smsQueryService = smsQueryService;
        _smsQueryService.NotNull(nameof(smsQueryService));
    }

    public async Task<ReportOutput<TrackingSmsHeaderOutputDto, TrackingSmsDataOutputDto>> GetByTrackIdAsync(Guid trackId, CancellationToken cancellationToken)
    {
        return await _smsQueryService.GetByTrackId(trackId);
    }

    public async Task<TrackingSmsDataOutputDto> GetByQueueIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _smsQueryService.Get(id);
    }
}
