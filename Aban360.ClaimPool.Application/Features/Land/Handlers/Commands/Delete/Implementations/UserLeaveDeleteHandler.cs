using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Implementations
{
    internal sealed class UserLeaveDeleteHandler : IUserLeaveDeleteHandler
    {
        private readonly IUserLeaveCommandService _userLeaveCommandService;
        private readonly IUserLeaveQueryService _userLeaveQueryService;
        public UserLeaveDeleteHandler(
            IUserLeaveCommandService userLeaveCommandService,
            IUserLeaveQueryService userLeaveQueryService)
        {
            _userLeaveCommandService = userLeaveCommandService;
            _userLeaveCommandService.NotNull(nameof(_userLeaveCommandService));

            _userLeaveQueryService = userLeaveQueryService;
            _userLeaveQueryService.NotNull(nameof(_userLeaveQueryService));
        }

        public async Task Handle(UserLeaveDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var userLeave = await _userLeaveQueryService.Get(deleteDto.Id);
            await _userLeaveCommandService.Remove(userLeave);
        }
    }
}
