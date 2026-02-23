using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts
{
    public interface IMeterChangeCreateHandler
    {
        Task Handle(MeterChangeInputDto inputDto, CancellationToken cancellationToken);
    }
}
