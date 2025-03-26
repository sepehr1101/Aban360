using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Persistence.Features.TimeTable.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Delete.Implementations
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
