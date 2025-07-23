using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Contracts
{
    public interface IZaribUpdateHandler
    {
        Task Handle(ZaribUpdateDto UpdateDto, CancellationToken cancellationToken);
    }
}
