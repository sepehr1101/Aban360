using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts
{
    public interface IWaterMeterGetAllHandler
    {
        Task<ICollection<WaterMeterGetDto>> Handle(CancellationToken cancellationToken);
    }
}
