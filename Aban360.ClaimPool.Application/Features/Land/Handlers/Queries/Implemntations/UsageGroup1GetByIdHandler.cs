using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class UsageGroup1GetByIdHandler : IUsageGroup1GetByIdHandler
    {
        private readonly IUsageGroup1QueryService _usageGroup1QueryService;
        public UsageGroup1GetByIdHandler(IUsageGroup1QueryService usageGroup1QueryService)
        {
            _usageGroup1QueryService = usageGroup1QueryService;
            _usageGroup1QueryService.NotNull(nameof(usageGroup1QueryService));
        }
        public async Task<UsageGroup1GetDto> Handle(int id, CancellationToken cancellationToken)
        {
            UsageGroup1GetDto result = await _usageGroup1QueryService.Get(id);
            return result;
        }
    }
}
