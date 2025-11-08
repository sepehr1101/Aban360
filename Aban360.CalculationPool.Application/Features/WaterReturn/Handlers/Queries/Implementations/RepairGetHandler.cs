using Aban360.CalculationPool.Application.Features.WaterReturn.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.CalculationPool.Persistence.Features.WaterReturn.Queries.Contracts;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.WaterReturn.Handlers.Queries.Implementations
{
    internal sealed class RepairGetHandler : IRepairGetHandler
    {
        private readonly IRepairQueryService _queryService;
        public RepairGetHandler(IRepairQueryService queryService)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<RepairGetDto> Handle(SearchById input, CancellationToken cancellationToken)
        {
            RepairGetDto result = await _queryService.Get(input.Id);
            return result;
        }
    }
}
