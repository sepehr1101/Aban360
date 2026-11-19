using global::Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using global::Aban360.Common.Extensions;
using global::Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using global::Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using global::Aban360.UserPool.Domain.Features.Auth.Entities;
using global::Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;

namespace BeforeServiceWorkflow10.Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations{
    internal sealed class UserLoginFindHandler : IUserLoginFindHandler
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
            UserLogin userLogin = await _userLoginQueryService.Get(input.Id);
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
