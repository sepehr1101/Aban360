using Aban360.ClaimPool.Domain.Features.Tracking.Dto;

namespace Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts
{
    public interface IGeneralInformationHandler
    {
        Task<GeneralRequestDataOutputDto> Handle(Guid trackId, CancellationToken cancellationToken);
    }
}