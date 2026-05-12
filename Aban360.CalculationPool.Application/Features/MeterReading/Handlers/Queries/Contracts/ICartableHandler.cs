using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.ApplicationUser;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts
{
    public interface ICartableHandler
    {
        Task<IEnumerable<MeterFlowCartableGetDto>> Handle(IAppUser appUser, CancellationToken cancellationToken);
    }
}
