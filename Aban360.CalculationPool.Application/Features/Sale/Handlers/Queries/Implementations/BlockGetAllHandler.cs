using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Implementations
{
    internal sealed class BlockGetAllHandler : IBlockGetAllHandler
    {
        private readonly IBlockQueryService _queryService;
        public BlockGetAllHandler(IBlockQueryService queryService)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<IEnumerable<string>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<string> result = await _queryService.Get();
            return result;
        }
    }
}
