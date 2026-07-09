using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class UsageGroup1GetAllHandler : IUsageGroup1GetAllHandler
    {
        private readonly IUsageGroup1QueryService _usageGroup1QueryService;
        public UsageGroup1GetAllHandler(IUsageGroup1QueryService usageGroup1QueryService)
        {
            _usageGroup1QueryService = usageGroup1QueryService;
            _usageGroup1QueryService.NotNull(nameof(usageGroup1QueryService));
        }
        public async Task<IEnumerable<UsageGroup1GetDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<UsageGroup1GetDto> result = await _usageGroup1QueryService.Get();
            return result;
        }
    }
}
