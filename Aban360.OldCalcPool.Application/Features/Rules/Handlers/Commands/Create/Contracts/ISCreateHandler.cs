using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Contracts
{
    public interface ISCreateHandler
    {
        Task Handle(SCreateDto createDto, CancellationToken cancellationToken);
    }
}
