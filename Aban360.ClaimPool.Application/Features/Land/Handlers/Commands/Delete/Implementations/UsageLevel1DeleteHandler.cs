using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Implementations
{
    internal sealed class UsageLevel1DeleteHandler : IUsageLevel1DeleteHandler
    {
        private readonly IUsageLevel1CommandService _usageLevel1CommandService;

        private readonly IUsageLevel1QueryService _usageLevel1QueryService;
        public UsageLevel1DeleteHandler(
            IUsageLevel1CommandService usageLevel1CommandService,
            IUsageLevel1QueryService usageLevel1QueryService)
        {
            _usageLevel1CommandService = usageLevel1CommandService;
            _usageLevel1CommandService.NotNull(nameof(_usageLevel1CommandService));

            _usageLevel1QueryService = usageLevel1QueryService;
            _usageLevel1QueryService.NotNull(nameof(_usageLevel1QueryService));
        }

        public async Task Handle(UsageLevel1DeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var usageLevel1 = await _usageLevel1QueryService.Get(deleteDto.Id);
            await _usageLevel1CommandService.Remove(usageLevel1);
        }
    }
}
