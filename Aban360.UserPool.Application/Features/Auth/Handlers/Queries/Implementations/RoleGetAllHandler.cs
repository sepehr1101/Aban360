using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    internal sealed class RoleGetAllHandler : IRoleGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IRoleQueryService _roleQueryService;
        public RoleGetAllHandler(
            IMapper mapper,
            IRoleQueryService roleQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _roleQueryService = roleQueryService;
            _roleQueryService.NotNull(nameof(roleQueryService));
        }

        public async Task<ICollection<RoleGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<Role> role = await _roleQueryService.Get();
            return _mapper.Map<ICollection<RoleGetDto>>(role);
        }
    }
}
