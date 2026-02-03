using Aban360.ClaimPool.Domain.Features.Tracking.Dto;

namespace Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts
{
    public interface IExamineTimeSetDetailHandler
    {
        Task<ExamineTimeSetOutputDto> Handle(TrackingDetailGetDto inputDto, CancellationToken cancellationToken);
    }
}
