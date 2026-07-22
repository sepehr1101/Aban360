using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class UsageGroup2GetByGroup1IdHandler : IUsageGroup2GetByGroup1IdHandler
    {
        private readonly IUsageGroup2QueryService _usageGroup2QueryService;
        public UsageGroup2GetByGroup1IdHandler(IUsageGroup2QueryService usageGroup2QueryService)
        {
            _usageGroup2QueryService = usageGroup2QueryService;
            _usageGroup2QueryService.NotNull(nameof(usageGroup2QueryService));
        }
        public async Task<IEnumerable<UsageGroup2GetDto>> Handle(short id, CancellationToken cancellationToken)
        {
            IEnumerable<UsageGroup2GetDto> result = await _usageGroup2QueryService.GetByParentId(id);
            return result;
        }
    }
}
