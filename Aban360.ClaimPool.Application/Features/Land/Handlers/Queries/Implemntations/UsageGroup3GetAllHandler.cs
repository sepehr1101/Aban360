using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class UsageGroup3GetAllHandler : IUsageGroup3GetAllHandler
    {
        private readonly IUsageGroup3QueryService _usageGroup3QueryService;
        public UsageGroup3GetAllHandler(IUsageGroup3QueryService usageGroup3QueryService)
        {
            _usageGroup3QueryService = usageGroup3QueryService;
            _usageGroup3QueryService.NotNull(nameof(usageGroup3QueryService));
        }
        public async Task<IEnumerable<UsageGroup3GetDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<UsageGroup3GetDto> result = await _usageGroup3QueryService.Get();
            return result;
        }
    }
}
