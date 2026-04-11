using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.Request.Inputs;
using Aban360.ReportPool.Domain.Features.Request.Outputs;

namespace Aban360.ReportPool.Persistence.Features.Request.Contracts
{
    public interface ITrackingStepGroupQueryService
    {
        Task<ReportOutput<TrackingStepHeaderOutputDto, TrackingStepGroupDataOutputDto>> Get(TrackingInputDto input);
    }
}
