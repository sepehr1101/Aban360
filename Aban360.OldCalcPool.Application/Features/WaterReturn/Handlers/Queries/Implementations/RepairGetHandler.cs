using Aban360.OldCalcPools.Application.Features.WaterReturn.Handlers.Queries.Contracts;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Queries.Contracts;
using Aban360.Common.Extensions;
using FluentValidation;
using Aban360.Common.BaseEntities;

namespace Aban360.OldCalcPools.Application.Features.WaterReturn.Handlers.Queries.Implementations
{
    internal sealed class RepairGetHandler : IRepairGetHandler
    {
        private readonly IRepairQueryService _queryService;
        public RepairGetHandler(IRepairQueryService queryService)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<RepairGetDto> Handle(SearchInput input, CancellationToken cancellationToken)
        {
            RepairGetDto result = await _queryService.Get(int.Parse( input.Input));
            return result;
        }
    }
}
