using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts
{
    public interface IProcessing
    {
        Task Handle(ConsumptionInputDto intput, CancellationToken cancellationToken);
        Task Handle(MeterInfoInputDto input, CancellationToken cancellationToken);
    }
}