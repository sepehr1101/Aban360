using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    internal sealed class CollectBillsDetailGetAllHandler : ICollectBillsDetailGetAllHandler
    {
        private readonly ICollectBillsDetailQueryService _collectBillsDetailQueryService;
        public CollectBillsDetailGetAllHandler(ICollectBillsDetailQueryService collectBillsDetailQueryService)
        {
            _collectBillsDetailQueryService = collectBillsDetailQueryService;
            _collectBillsDetailQueryService.NotNull(nameof(collectBillsDetailQueryService));
        }

        public async Task<IEnumerable<CollectBillsDetailGetDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<CollectBillsDetailGetDto> result = await _collectBillsDetailQueryService.Get();
            return result;
        }
    }

}
