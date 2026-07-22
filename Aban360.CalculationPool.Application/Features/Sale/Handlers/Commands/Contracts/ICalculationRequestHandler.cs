using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts
{
    public interface ICalculationRequestHandler
    {
        Task<ReportOutput<SaleAndAfterSaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto>> Handle(TrackNumberWithDescriptionInputDto inputDto, int userCode, CancellationToken cancellationToken);
    }
}
