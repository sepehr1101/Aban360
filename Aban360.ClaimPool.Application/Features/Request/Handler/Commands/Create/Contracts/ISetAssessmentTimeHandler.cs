using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts
{
    public interface ISetAssessmentTimeHandler
    {
        Task<SetAssessmentTimeDataOutputDto> Handle(AssessmentSetTimeInputDto input, int userName, CancellationToken cancellationToken);
    }
}
