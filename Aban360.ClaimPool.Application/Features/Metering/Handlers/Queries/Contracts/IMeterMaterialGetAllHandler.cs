using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts
{
    public interface IMeterMaterialGetAllHandler
    {
        Task<ICollection<MeterMaterialGetDto>> Handle(CancellationToken cancellationToken);
    }
}
