using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts
{
    public interface IReAssessmentRequestHandler
    {
        Task Handle(TrackNumberWithDescriptionInputDto inputDto, int userCode, CancellationToken cancellationToken);
    }
}
