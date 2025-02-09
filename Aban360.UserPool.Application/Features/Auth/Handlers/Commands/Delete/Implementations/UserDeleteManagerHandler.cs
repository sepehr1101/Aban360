using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Base;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Delete.Implementations
{
    public class UserDeleteManagerHandler : IUserDeleteManagerHandler
    {
        private readonly IUserQueryService _userQueryService;
        public UserDeleteManagerHandler(IUserQueryService userQueryService)
        {
            _userQueryService = userQueryService;
            _userQueryService.NotNull(nameof(userQueryService));
        }

        public async Task Handle(UserIdDto userId, CancellationToken cancellationToken)
        {
            var user = await _userQueryService.Get(userId.Id);
            if (user == null)
            {
                throw new InvalidDataException();
            }

            user.ValidTo = DateTime.Now;
            user.RemoveLogInfo = "Remove Log Info";
        }
    }
}
