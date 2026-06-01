using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.GatewayAdhoc.Features.Sale.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.GatewayAdhoc.Features.Sale.Queries.Implementations
{
    internal sealed class SaleGetAddhoc : ISaleGetAddhoc
    {
        private readonly ISaleGetHandler _saleGetHandler;
        public SaleGetAddhoc(ISaleGetHandler saleGetHandler)
        {
            _saleGetHandler = saleGetHandler;
            _saleGetHandler.NotNull(nameof(saleGetHandler));
        }

        public async Task<ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto>> Handle(SaleInputDto inputDto, CancellationToken cancellationToken)
        {
            return await _saleGetHandler.Handle(inputDto, cancellationToken);
        }
    }
}
