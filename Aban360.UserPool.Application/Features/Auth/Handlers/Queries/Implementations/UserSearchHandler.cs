using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries.ValueKeys;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries.ValueKeyItems;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;
using System.Collections.Generic;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    internal sealed class UserSearchHandler : IUserSearchHandler
    {
        private readonly IUserQueryService _userQueryService;
        public UserSearchHandler(IUserQueryService userQueryService)
        {
            _userQueryService = userQueryService;
            _userQueryService.NotNull(nameof(userQueryService));
        }
        public async Task<IEnumerable<UserQueryDto>> Handle(SearchUserDto input, CancellationToken cancellationToken)
        {
            IEnumerable<UserQueryDto> result = await _userQueryService.Search(input);
            return result;
        }
    }
}
