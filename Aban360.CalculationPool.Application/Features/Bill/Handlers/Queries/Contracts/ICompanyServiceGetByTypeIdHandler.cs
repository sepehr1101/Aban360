using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts
{
    public interface ICompanyServiceGetByTypeIdHandler
    {
        Task<ICollection<NumericDictionary>> Handle(int typeId, CancellationToken cancellationToken);
    }
}
