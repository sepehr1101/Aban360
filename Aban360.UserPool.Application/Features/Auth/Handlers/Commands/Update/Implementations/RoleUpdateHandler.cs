using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Implementations
{
    public class RoleUpdateHandler : IRoleUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRoleQueryService _roleQueryService;
        public RoleUpdateHandler(
            IMapper mapper,
            IRoleQueryService roleQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _roleQueryService = roleQueryService;
            _roleQueryService.NotNull(nameof(roleQueryService));
        }

        public async Task Handle(RoleUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var role = await _roleQueryService.Get(updateDto.Id);
            if (role == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, role);
        }
    }
}
