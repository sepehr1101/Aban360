using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts
{
    public interface IAbonmanGetHandler
    {
        Task<AbonmanGetDto> Handle(int id, CancellationToken cancellationToken);
    }
}
