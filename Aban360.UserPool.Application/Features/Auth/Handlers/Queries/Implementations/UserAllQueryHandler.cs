using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    public sealed class UserAllQueryHandler : IUserAllQueryHandler
    {
        private readonly IUserQueryService _userQueryService;
        private readonly IMapper _mapper;
        public UserAllQueryHandler(
            IMapper mapper,
            IUserQueryService userQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _userQueryService = userQueryService;
            _userQueryService.NotNull(nameof(userQueryService));
        }
        public async Task<ICollection<UserQueryDto>> Handle(CancellationToken cancellationToken)
        {
            var users = await _userQueryService.Get();
            var userQueryDtoList = _mapper.Map<ICollection<UserQueryDto>>(users);
            return userQueryDtoList;
        }
    }
}
