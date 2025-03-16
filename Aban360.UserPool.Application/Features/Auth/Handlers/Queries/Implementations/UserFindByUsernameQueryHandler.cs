using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    internal sealed class UserFindByUsernameQueryHandler : IUserFindByUsernameQueryHandler
    {
        private readonly IMapper _mapper;
        private readonly IUserQueryService _userQueryService;
        public UserFindByUsernameQueryHandler(IMapper mapper, IUserQueryService userQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _userQueryService = userQueryService;
            _userQueryService.NotNull(nameof(userQueryService));
        }
        public async Task<(User, bool)> Handle(string username, string plainPassword)
        {
            User? user = await _userQueryService.Get(username);
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            string password = await SecurityOperations.GetSha512Hash(plainPassword);
            if (user.Password != password)
            {
                throw new ArgumentException(nameof(password));
            }
            return (user, true);
        }
    }
}
