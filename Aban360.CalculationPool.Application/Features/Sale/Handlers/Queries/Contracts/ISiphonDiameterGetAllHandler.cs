using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts
{
    public interface ISiphonDiameterGetAllHandler
    {
        Task<IEnumerable<NumericDictionary>> Handle(CancellationToken cancellationToken);
    }
}
