using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts
{
    public interface ITariffCalculationByDetailHandler
    {
        Task<IntervalCalculationResultWrapper> Test(TariffByDetailCreateDto createDto,CancellationToken cancellationToken);
    }
}