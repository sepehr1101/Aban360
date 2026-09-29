using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts
{
    public interface IReAssessmentRequestHandler
    {
        Task<SetAssessmentTimeDataOutputDto> Handle(SetReAssessmentTimeInputDto inputDto, int userCode, CancellationToken cancellationToken);
    }
}
