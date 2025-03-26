using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Persistence.Features.TimeTable.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Delete.Implementations
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
