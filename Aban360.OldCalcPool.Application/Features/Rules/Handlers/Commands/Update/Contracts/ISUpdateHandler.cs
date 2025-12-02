using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Contracts
{
    public interface ISUpdateHandler
    {
        Task Handle(SUpdateDto UpdateDto, CancellationToken cancellationToken);
    }
}
