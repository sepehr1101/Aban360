using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts
{
    public interface IProcessing
    {
        Task<AbBahaCalculationDetails> Handle(MeterInfoInputDto input, CancellationToken cancellationToken);
        Task<AbBahaCalculationDetails> Handle(MeterInfoByPreviousDataInputDto input, CancellationToken cancellationToken);
        Task<AbBahaCalculationDetails> Handle(BaseOldTariffEngineImaginaryInputDto input, CancellationToken cancellationToken);

        Task<AbBahaCalculationDetails> HandleWithAggregatedNerkh(MeterInfoInputDto input, CancellationToken cancellationToken);
        Task<AbBahaCalculationDetails> HandleWithAggregatedNerkh(MeterInfoByPreviousDataInputDto input, CancellationToken cancellationToken);
        Task<AbBahaCalculationDetails> HandleWithAggregatedNerkh(BaseOldTariffEngineImaginaryInputDto input, CancellationToken cancellationToken);
    }
}