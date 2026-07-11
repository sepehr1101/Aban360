using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    public interface IConCompanyGetByBillIdDictionaryHandler
    {
        Task<IEnumerable<NumericDictionary>> Handle(string billId, CancellationToken cancellationToken);
    }
}
