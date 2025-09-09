using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Contracts
{
    public interface INerkhCreateHandler
    {
        Task Handle(NerkhCreateDto createDto, int nerkh, CancellationToken cancellationToken);
        Task Handle(NerkhCreateDto createDto, CancellationToken cancellationToken);
    }
}
