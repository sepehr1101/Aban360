using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Contracts
{
    public interface ITable1UpdateHandler
    {
        Task Handle(Table1UpdateDto UpdateDto, CancellationToken cancellationToken);
    }
}
