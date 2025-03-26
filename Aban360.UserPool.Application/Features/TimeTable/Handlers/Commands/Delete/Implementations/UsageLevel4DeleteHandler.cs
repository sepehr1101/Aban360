using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Persistence.Features.TimeTable.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Delete.Implementations
{
    internal sealed class UsageLevel4DeleteHandler : IUsageLevel4DeleteHandler
    {
        private readonly IUsageLevel4CommandService _usageLevel4CommandService;
        private readonly IUsageLevel4QueryService _usageLevel4QueryService;
        public UsageLevel4DeleteHandler(
            IUsageLevel4CommandService usageLevel4CommandService,
            IUsageLevel4QueryService usageLevel4QueryService)
        {
            _usageLevel4CommandService = usageLevel4CommandService;
            _usageLevel4CommandService.NotNull(nameof(_usageLevel4CommandService));

            _usageLevel4QueryService = usageLevel4QueryService;
            _usageLevel4QueryService.NotNull(nameof(_usageLevel4QueryService));
        }

        public async Task Handle(UsageLevel4DeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var usageLevel4 = await _usageLevel4QueryService.Get(deleteDto.Id);
            await _usageLevel4CommandService.Remove(usageLevel4);
        }
    }
}
