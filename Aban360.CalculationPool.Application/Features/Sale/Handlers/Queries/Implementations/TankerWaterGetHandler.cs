using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Implementations
{
    internal sealed class TankerWaterGetHandler : ITankerWaterGetHandler
    {
        private readonly ITankerQueryService _tankerQueryService;
        public TankerWaterGetHandler(ITankerQueryService tankerQueryService)
        {
            _tankerQueryService = tankerQueryService;
            _tankerQueryService.NotNull(nameof(tankerQueryService));
        }

        public async Task<ReportOutput<TankerWaterHeaderOutputDto, TankerWaterDateOutputDto>> Handle(TankerWaterInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<TankerWaterHeaderOutputDto, TankerWaterDateOutputDto> result = await _tankerQueryService.Get(inputDto);
            return result;
        }
    }
}
