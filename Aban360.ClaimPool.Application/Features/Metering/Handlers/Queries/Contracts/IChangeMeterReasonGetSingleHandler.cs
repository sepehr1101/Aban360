using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts
{
    public interface IChangeMeterReasonGetSingleHandler
    {
        Task<ChangeMeterReasonGetDto> Handle(ChangeMeterReasonEnum id, CancellationToken cancellationToken);
    }
}
