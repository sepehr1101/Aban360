using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts
{
    public interface IImpactSignGetAllHandler
    {
        Task<ICollection<ImpactSignGetDto>> Handle(CancellationToken cancellationToken);
    }
}
