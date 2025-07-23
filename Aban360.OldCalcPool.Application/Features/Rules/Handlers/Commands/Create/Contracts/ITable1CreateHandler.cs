using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Contracts
{
    public interface ITable1CreateHandler
    {
        Task Handle(Table1CreateDto CreateDto, CancellationToken cancellationToken);
    }
}
