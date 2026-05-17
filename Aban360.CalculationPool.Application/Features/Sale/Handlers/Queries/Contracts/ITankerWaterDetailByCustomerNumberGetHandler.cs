using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts
{
    public interface ITankerWaterDetailByCustomerNumberGetHandler
    {
        Task<ReportOutput<TankerHeaderOutputDto, StringDictionary>> Handle(ZoneIdAndCustomerNumber input, CancellationToken cancellationToken);
    }
}
