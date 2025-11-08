using Aban360.CalculationPool.Application.Features.WaterReturn.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.CalculationPool.Persistence.Features.WaterReturn.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.WaterReturn.Handlers.Queries.Implementations
{
    internal sealed class RepairGetByBillIdHandler : IRepairGetByBillIdHandler
    {
        private readonly IRepairQueryService _queryService;
        public RepairGetByBillIdHandler(IRepairQueryService queryService)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<IEnumerable<RepairGetDto>> Handle(SearchInput input, CancellationToken cancellationToken)
        {
            IEnumerable<RepairGetDto> result = await _queryService.Get(input.Input);
            return result;
        }
    }
}
