using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts
{
    public interface ISetAssessmentResultHandler
    {
        Task Handle(AssessmentResultInputDto inputDto, CancellationToken cancellationToken);
    }
}
