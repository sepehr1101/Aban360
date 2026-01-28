using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts
{
    public interface ISaleGetHandler
    {
        Task<ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto>> Handle(SaleInputDto inputDto, CancellationToken cancellationToken);
        Task<ReportOutput<SaleHeaderReportOutputDto, SaleDataOutputDto>> ReportHandle(SaleInputDto inputDto, CancellationToken cancellationToken);
    }
}
