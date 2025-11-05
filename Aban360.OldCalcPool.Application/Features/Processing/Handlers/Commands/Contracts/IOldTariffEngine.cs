using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts
{
    public interface IOldTariffEngine
    {     
        Task<AbBahaCalculationDetails> Handle(MeterInfoInputDto input, CancellationToken cancellationToken);
        Task<AbBahaCalculationDetails> Handle(MeterInfoByPreviousDataInputDto input, CancellationToken cancellationToken);
        Task<AbBahaCalculationDetails> Handle(MeterImaginaryInputDto input, CancellationToken cancellationToken);
    }
}