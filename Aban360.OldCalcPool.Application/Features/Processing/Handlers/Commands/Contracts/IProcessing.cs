using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts
{
    public interface IProcessing
    {     
        Task<AbBahaCalculationDetails> HandleWithAggregatedNerkh(MeterInfoInputDto input, CancellationToken cancellationToken);
        Task<AbBahaCalculationDetails> HandleWithAggregatedNerkh(MeterInfoByPreviousDataInputDto input, CancellationToken cancellationToken);
        Task<AbBahaCalculationDetails> HandleWithAggregatedNerkh(BaseOldTariffEngineImaginaryInputDto input, CancellationToken cancellationToken);
    }
}