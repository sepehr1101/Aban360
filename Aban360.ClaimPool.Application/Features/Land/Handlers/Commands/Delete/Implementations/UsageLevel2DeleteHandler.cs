using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Implementations
{
    internal sealed class UsageLevel2DeleteHandler : IUsageLevel2DeleteHandler
    {
        private readonly IUsageLevel2CommandService _usageLevel2CommandService;
        private readonly IUsageLevel2QueryService _usageLevel2QueryService;
        public UsageLevel2DeleteHandler(
            IUsageLevel2CommandService usageLevel2CommandService,
            IUsageLevel2QueryService usageLevel2QueryService)
        {
            _usageLevel2CommandService = usageLevel2CommandService;
            _usageLevel2CommandService.NotNull(nameof(_usageLevel2CommandService));

            _usageLevel2QueryService = usageLevel2QueryService;
            _usageLevel2QueryService.NotNull(nameof(_usageLevel2QueryService));
        }

        public async Task Handle(UsageLevel2DeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var usageLevel2 = await _usageLevel2QueryService.Get(deleteDto.Id);
            await _usageLevel2CommandService.Remove(usageLevel2);
        }
    }
}
