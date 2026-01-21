using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts
{
    public interface IAfterSaleGetHandler
    {
        Task<FlatReportOutput<SaleHeaderOutputDto, AfterSaleDataOutputDto>> Handle(AfterSaleInputDto input, CancellationToken cancellationToken);
    }
}
