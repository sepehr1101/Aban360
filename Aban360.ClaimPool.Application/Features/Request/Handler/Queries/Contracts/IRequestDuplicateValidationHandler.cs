using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts
{
    public interface IRequestDuplicateValidationHandler
    {
        Task<TrackingDuplicateValidationOutputDto> Handle(TrackingDuplicateValidationInputDto inputDto, CancellationToken cancellationToken);
    }
}
