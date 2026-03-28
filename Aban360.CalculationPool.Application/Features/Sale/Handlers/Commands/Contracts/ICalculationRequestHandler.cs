using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts
{
    public interface ICalculationRequestHandler
    {
        Task<ReportOutput<SaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto>> Handle(int trackNumber, int userCode, CancellationToken cancellationToken);
    }
}
