using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;

namespace Aban360.OldCalcPools.Application.Features.WaterReturn.Handlers.Commands.Contracts
{
    public interface IRepairCreateHandler
    {
        Task Handle(OfferingToCreateRepairDto createDto, CancellationToken cancellationToken);
        Task<AbBahaCalculationDetails> Handle(MeterInfoWithMonthlyConsumptionOutputDto input, CancellationToken cancellationToken);
        Task<AbBahaCalculationDetails> Handle(MeterInfoByLastMonthlyConsumptionOutputDto input, CancellationToken cancellationToken);
        Task<AbBahaCalculationDetails> Handle(MeterInfoByPreviousDataWithInvoiceIdInputDto input, CancellationToken cancellationToken);
    }
}
