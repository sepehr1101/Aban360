using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts
{
    public interface IRemovedBillHandler
    {
        Task Handle(RemovedBillInputDto inputDto, CancellationToken cancellationToken);
    }
}
