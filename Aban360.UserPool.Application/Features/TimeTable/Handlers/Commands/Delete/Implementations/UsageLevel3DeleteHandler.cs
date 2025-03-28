using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Persistence.Features.TimeTable.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Delete.Implementations
{
    internal sealed class UsageLevel3DeleteHandler : IUsageLevel3DeleteHandler
    {
        private readonly IUsageLevel3CommandService _usageLevel3CommandService;
        private readonly IUsageLevel3QueryService _usageLevel3QueryService;
        public UsageLevel3DeleteHandler(
            IUsageLevel3CommandService usageLevel3CommandService,
            IUsageLevel3QueryService usageLevel3QueryService)
        {
            _usageLevel3CommandService = usageLevel3CommandService;
            _usageLevel3CommandService.NotNull(nameof(_usageLevel3CommandService));

            _usageLevel3QueryService = usageLevel3QueryService;
            _usageLevel3QueryService.NotNull(nameof(_usageLevel3QueryService));
        }

        public async Task Handle(UsageLevel3DeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var usageLevel3 = await _usageLevel3QueryService.Get(deleteDto.Id);
            await _usageLevel3CommandService.Remove(usageLevel3);
        }
    }
}
