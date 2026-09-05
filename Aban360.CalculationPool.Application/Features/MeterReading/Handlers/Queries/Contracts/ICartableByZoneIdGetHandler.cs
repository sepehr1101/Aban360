using Aban360.CalculationPool.Domain.Features.CollectBills.Inputs;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.ApplicationUser;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts
{
    public interface ICartableByZoneIdGetHandler
    {
        Task<IEnumerable<MeterFlowCartableGetDto>> Handle(MeterFlowByZoneInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
