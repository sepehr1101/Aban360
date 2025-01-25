using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    public class UserLoginFindHandler : IUserLoginFindHandler
    {
        private readonly IUserLoginQueryService _userLoginQueryService;
        public UserLoginFindHandler(
            IUserLoginQueryService userLoginQueryService)
        {
            _userLoginQueryService = userLoginQueryService;
            _userLoginQueryService.NotNull(nameof(userLoginQueryService));
        }
        public async Task<UserLogin> Handle(SecondStepLoginInput input, CancellationToken cancellationToken)
        {
            var userLogin = await _userLoginQueryService.Get(input.Id);
            userLogin.NotNull(nameof(userLogin));
            userLogin.TwoStepCode.NotEmptyString(userLogin.TwoStepCode);
            if (userLogin.TwoStepCode != input.ConfirmCode)
            {
                throw new Exception("کد وارد شده صحیح نمیباشد");
            }
            return userLogin;
        }
    }
}
