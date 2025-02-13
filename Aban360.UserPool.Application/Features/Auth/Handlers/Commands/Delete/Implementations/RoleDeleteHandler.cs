using Aban360.Common.Categories.UseragentLog;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.AspNetCore.Http;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Delete.Implementations
{
    internal sealed class RoleDeleteHandler : IRoleDeleteHandler
    {
        private readonly IRoleQueryService _roleQueryService;
        private readonly IRoleCommandService _roleCommandService;
        private readonly IHttpContextAccessor _contextAccessor;

        public RoleDeleteHandler(
            IRoleQueryService roleQueryService,
            IRoleCommandService roleCommandService,
            IHttpContextAccessor contextAccessor)
        {
            _roleQueryService = roleQueryService;
            _roleQueryService.NotNull(nameof(roleQueryService));

            _roleCommandService = roleCommandService;
            _roleCommandService.NotNull(nameof(roleCommandService));

            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));
        }

        public async Task Handle(RoleDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var role = await _roleQueryService.Get(deleteDto.Id);
            if (role.IsRemovable)
            {
                throw new BaseException("این نقش غیرقابل حذف است");//todo magic string
            }
            string removeLogInfo = LogInfoJson.Get(_contextAccessor.HttpContext.Request, true);
            _roleCommandService.Remove(role, removeLogInfo);
        }
    }
}
