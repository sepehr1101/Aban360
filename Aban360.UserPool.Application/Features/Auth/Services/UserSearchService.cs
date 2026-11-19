using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries.ValueKeys;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries.ValueKeyItems;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;
using System.Collections.Generic;

namespace Aban360.UserPool.Application.Features.Auth.Services;

public interface IUserSearchService
{
    Task<IEnumerable<UserQueryDto>> SearchAsync(SearchUserDto input, CancellationToken cancellationToken);
    Task<IEnumerable<UserQueryDto>> SearchByRoleAsync(int roleId, CancellationToken cancellationToken);
    Task<IEnumerable<UserQueryDto>> SearchByRoleAndZoneAsync(int roleId, int zoneId, CancellationToken cancellationToken);
}

internal sealed class UserSearchService : IUserSearchService
{
    private readonly IUserQueryService _userQueryService;
    private readonly IUserRoleQueryService _userRoleQueryService;
    public UserSearchService(
        IUserQueryService userQueryService,
        IUserRoleQueryService userRoleQueryService)
    {
        _userQueryService = userQueryService;
        _userQueryService.NotNull(nameof(userQueryService));
        _userRoleQueryService = userRoleQueryService;
        _userRoleQueryService.NotNull(nameof(userRoleQueryService));
    }

    public async Task<IEnumerable<UserQueryDto>> SearchAsync(SearchUserDto input, CancellationToken cancellationToken)
    {
        IEnumerable<UserQueryDto> result = await _userQueryService.Search(input);
        return result;
    }

    public async Task<IEnumerable<UserQueryDto>> SearchByRoleAsync(int roleId, CancellationToken cancellationToken)
    {
        return await _userRoleQueryService.Get(roleId);
    }

    public async Task<IEnumerable<UserQueryDto>> SearchByRoleAndZoneAsync(int roleId, int zoneId, CancellationToken cancellationToken)
    {
        return await _userRoleQueryService.GetByZone(roleId, zoneId);
    }
}
