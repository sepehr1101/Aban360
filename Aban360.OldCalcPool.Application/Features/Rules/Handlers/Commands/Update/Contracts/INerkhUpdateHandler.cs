using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Contracts
{
    public interface INerkhUpdateHandler
    {
        Task Handle(NerkhUpdateDto UpdateDto, int nerkh, CancellationToken cancellationToken);
    }
}
