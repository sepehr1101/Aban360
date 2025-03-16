using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using AutoMapper;
using Gridify;
using Gridify.EntityFramework;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    public sealed class UserGridifyQueryHandler : IUserGridifyQueryHandler
    {
        private readonly IUserQueryService _userQueryService;
        private readonly IMapper _mapper;
        public UserGridifyQueryHandler(
            IMapper mapper,
            IUserQueryService userQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _userQueryService = userQueryService;
            _userQueryService.NotNull(nameof(userQueryService));
        }
        public async Task<ICollection<UserQueryDto>> Handle(GridifyQuery query, CancellationToken cancellationToken)
        {
            Paging<User> users = await _userQueryService
                .GetQuery()
                .GridifyAsync(query);

            ICollection<UserQueryDto> usersDto = _mapper.Map<ICollection<UserQueryDto>>(users.Data);
            return usersDto;
        }
    }
}
