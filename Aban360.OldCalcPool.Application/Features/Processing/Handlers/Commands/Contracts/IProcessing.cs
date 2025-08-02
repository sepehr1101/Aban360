using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts
{
    public interface IProcessing
    {
        Task Handle(ConsumptionInputDto intput, CancellationToken cancellationToken);
        Task<ProcessDetailOutputDto> Handle(MeterInfoInputDto input, CancellationToken cancellationToken);
        Task<ProcessDetailOutputDto> Handle(MeterInfoByPreviousDataInputDto input, CancellationToken cancellationToken);
        Task<ProcessDetailOutputDto> Handle(BaseOldTariffEngineImaginaryInputDto input, CancellationToken cancellationToken);
    }
}