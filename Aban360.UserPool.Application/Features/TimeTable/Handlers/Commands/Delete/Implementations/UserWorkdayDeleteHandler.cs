using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Persistence.Features.TimeTable.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Delete.Implementations
{
    internal sealed class UserWorkdayDeleteHandler : IUserWorkdayDeleteHandler
    {
        private readonly IUserWorkdayCommandService _userWorkdayCommandService;
        private readonly IUserWorkdayQueryService _userWorkdayQueryService;
        public UserWorkdayDeleteHandler(
            IUserWorkdayCommandService userWorkdayCommandService,
            IUserWorkdayQueryService userWorkdayQueryService)
        {
            _userWorkdayCommandService = userWorkdayCommandService;
            _userWorkdayCommandService.NotNull(nameof(_userWorkdayCommandService));

            _userWorkdayQueryService = userWorkdayQueryService;
            _userWorkdayQueryService.NotNull(nameof(_userWorkdayQueryService));
        }

        public async Task Handle(UserWorkdayDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var userWorkday = await _userWorkdayQueryService.Get(deleteDto.Id);
            await _userWorkdayCommandService.Remove(userWorkday);
        }
    }
}
