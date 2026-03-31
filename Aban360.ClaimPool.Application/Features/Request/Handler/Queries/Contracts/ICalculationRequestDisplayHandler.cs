using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts
{
    public interface ICalculationRequestDisplayHandler
    {
        Task<ReportOutput<SaleAndAfterSaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto>> Handle(int trackNumber, CancellationToken cancellationToken);
    }
}
