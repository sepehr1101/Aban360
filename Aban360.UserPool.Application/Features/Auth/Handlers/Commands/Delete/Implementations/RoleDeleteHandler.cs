using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Delete.Implementations
{
    public class RoleDeleteHandler : IRoleDeleteHandler
    {
        private readonly IRoleQueryService _roleQueryService;
        private readonly IRoleCommandService _roleCommandService;
        public RoleDeleteHandler(
            IRoleQueryService roleQueryService,
            IRoleCommandService roleCommandService)
        {
            _roleQueryService = roleQueryService;
            _roleQueryService.NotNull(nameof(roleQueryService));

            _roleCommandService = roleCommandService;
            _roleCommandService.NotNull(nameof(roleCommandService));
        }

        public async Task Handle(RoleDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var role = await _roleQueryService.Get(deleteDto.Id);
            if (role == null)
            {
                throw new InvalidDataException();
            }
            _roleCommandService.Remove(role, "RemoveLogInfo");//todo: removeLogInfo
        }
    }
}
