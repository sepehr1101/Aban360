using Endpoint = Aban360.UserPool.Domain.Features.AceessTree.Entites.Endpoint;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.UseragentLog;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries.ValueKeys;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using Aban360.UserPool.Application.Common.Base;
using Aban360.UserPool.Application.Features.AccessTree.Factories;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries.ValueKeyItems;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Domain.Features.Auth.Dto.Base;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Constants.Enums;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;
using FluentValidation;
using Gridify;
using Gridify.EntityFramework;
using Microsoft.AspNetCore.Http;

namespace Aban360.UserPool.Application.Features.Auth.Services;
public interface IUserManagementService
{
    Task CreateAsync(UserCreateDto userCreateDto, CancellationToken cancellationToken);
    Task<UserPersonalGetDto> UpdateAsync(UserUpdateDto userUpdateDto, IAppUser currentUser, CancellationToken cancellationToken);
    Task DeleteManagerAsync(UserIdDto userId, CancellationToken cancellationToken);
    Task<ICollection<UserQueryDto>> AllAsync(CancellationToken cancellationToken);
    Task<ICollection<UserBriefQueryDto>> BriefAsync(CancellationToken cancellationToken);
    Task<ICollection<UserQueryDto>> GridifyAsync(GridifyQuery query, CancellationToken cancellationToken);
    Task<UserDisplayDto> GetSingleAsync(Guid userId, CancellationToken cancellationToken);
}

internal sealed class UserManagementService : UserBaseCreateOrUpdateService, IUserManagementService
{
    private readonly IUserQueryService _userQueryService;
    private readonly IUserCommandService _userCommandService;
    private readonly IUserClaimCommandService _userClaimCommandService;
    private readonly IUserRoleCommandService _userRoleCommandService;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IZoneCountQueryAddhoc _zoneCountQueryAddhoc;
    private readonly IEndpointQueryService _endpointQueryService;
    private readonly IRoleQueryService _roleQueryService;
    private readonly IValidator<UserCreateDto> _userValidator;
    private readonly IUnitOfWork _uow;
    private readonly IUserClaimQueryService _userClaimsQueryService;
    private readonly IUserRoleQueryService _userRoleQueryService;
    private readonly IValidator<UserUpdateDto> _updateValidator;
    private readonly IT51QueryService _t51QueryService;
    private readonly ILocationTreeAdHoc _locationTreeAdHoc;
    public UserManagementService(IUserCommandService userCommandService, IUserClaimCommandService userClaimCommandService, IUserRoleCommandService userRoleCommandService, IMapper mapper, IHttpContextAccessor contextAccessor, IZoneCountQueryAddhoc zoneCountQueryAddhoc, IEndpointQueryService endpointQueryService, IRoleQueryService roleQueryService, IValidator<UserCreateDto> userValidator, IUserQueryService userQueryService, IUnitOfWork uow, IUserClaimQueryService userClaimsQueryService, IUserRoleQueryService userRoleQueryService, IValidator<UserUpdateDto> updateAsyncUserValidator, IT51QueryService t51QueryService, ILocationTreeAdHoc locationTreeAdHoc)
    {
        _userCommandService = userCommandService;
        _userCommandService.NotNull(nameof(userCommandService));
        _userClaimCommandService = userClaimCommandService;
        _userClaimCommandService.NotNull(nameof(userClaimCommandService));
        _userRoleCommandService = userRoleCommandService;
        _userRoleCommandService.NotNull(nameof(userRoleCommandService));
        _mapper = mapper;
        _mapper.NotNull(nameof(mapper));
        _contextAccessor = contextAccessor;
        _contextAccessor.NotNull(nameof(contextAccessor));
        _zoneCountQueryAddhoc = zoneCountQueryAddhoc;
        _zoneCountQueryAddhoc.NotNull(nameof(zoneCountQueryAddhoc));
        _endpointQueryService = endpointQueryService;
        _endpointQueryService.NotNull(nameof(endpointQueryService));
        _roleQueryService = roleQueryService;
        _roleQueryService.NotNull(nameof(roleQueryService));
        _userValidator = userValidator;
        _userValidator.NotNull(nameof(userValidator));
        _userQueryService = userQueryService;
        _userQueryService.NotNull(nameof(_userQueryService));
        _uow = uow;
        _uow.NotNull(nameof(uow));
        _userQueryService.NotNull(nameof(userQueryService));
        _userClaimsQueryService = userClaimsQueryService;
        _userClaimsQueryService.NotNull(nameof(userClaimsQueryService));
        _userRoleQueryService = userRoleQueryService;
        _userRoleQueryService.NotNull(nameof(userRoleQueryService));
        _userClaimCommandService.NotNull(nameof(_userClaimCommandService));
        _userRoleCommandService.NotNull(nameof(_userRoleCommandService));
        _endpointQueryService.NotNull(nameof(_endpointQueryService));
        _updateValidator = updateAsyncUserValidator;
        _updateValidator.NotNull(nameof(updateAsyncUserValidator));
        _t51QueryService = t51QueryService;
        _t51QueryService.NotNull(nameof(t51QueryService));
        _locationTreeAdHoc = locationTreeAdHoc;
        _locationTreeAdHoc.NotNull(nameof(locationTreeAdHoc));
    }

    public async Task CreateAsync(UserCreateDto userCreateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _userValidator.ValidateAsync(userCreateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        } //

        if (await _userQueryService.Exists(userCreateDto))
        {
            throw new CustomValidationException("کاربر قبلا ایجاد شده است");
        }

        LogInfo logInfo = DeviceDetection.GetLogInfo(_contextAccessor.HttpContext.Request);
        string logInfoString = JsonOperation.Marshal(logInfo);
        Guid operationGroupId = Guid.NewGuid();
        int zoneCount = await _zoneCountQueryAddhoc.GetCount(userCreateDto.SelectedZoneIds, cancellationToken);
        //List<string> endpointValue = await _endpointQueryService.GetAuthValue(userCreateDto.SelectedEndpointIds.ToArray());
        IEnumerable<EndpointInfo> endpointValues = await CreateGetUserEndpoints(userCreateDto);
        Validate(zoneCount, userCreateDto.SelectedZoneIds.Count(), endpointValues.Count(), userCreateDto.SelectedEndpointIds.Distinct().Count());
        ICollection<UserClaim> zones = CreateUserClaim(userCreateDto.SelectedZoneIds.Select(x => x.ToString()).ToList(), ClaimType.ZoneId, logInfoString, operationGroupId, operationGroupId);
        ICollection<UserClaim> endpionts = CreateUserClaim(endpointValues, ClaimType.Endpoint, logInfoString, operationGroupId, operationGroupId);
        string defaultZoneIdValue = userCreateDto.SelectedZoneIds.Count() > 30 ? "130000" : userCreateDto.SelectedZoneIds.Select(x => x.ToString()).First();
        ICollection<UserClaim> defaultZoneId = CreateUserClaim(new List<string> { defaultZoneIdValue }, ClaimType.DefaultZoneId, logInfoString, operationGroupId, operationGroupId);
        List<UserClaim> userCliams = zones.Union(endpionts).Union(defaultZoneId).ToList();
        ICollection<UserRole> userRoles = CreateUserRoles(userCreateDto.SelectedRoleIds, logInfoString, operationGroupId, operationGroupId);
        User user = _mapper.Map<User>(userCreateDto);
        user.Id = operationGroupId;
        user.InsertLogInfo = logInfoString;
        user.Password = await SecurityOperations.GetSha512Hash(userCreateDto.Password);
        await _userCommandService.Add(user);
        await _userClaimCommandService.Add(userCliams);
        await _userRoleCommandService.Add(userRoles);
    }

    private async Task<IEnumerable<EndpointInfo>> CreateGetUserEndpoints(UserCreateDto userCreateDto)
    {
        IEnumerable<EndpointInfo> userDirectEndpoints = new List<EndpointInfo>();
        IEnumerable<EndpointInfo> userRoleEndpoints = new List<EndpointInfo>();
        List<string> endpointValues = await _endpointQueryService.GetAuthValue(userCreateDto.SelectedEndpointIds.ToArray());
        if (endpointValues != null && endpointValues.Any())
        {
            userDirectEndpoints = endpointValues.Select(e => new EndpointInfo(e, null));
        }

        ICollection<Role> selectedRoles = await _roleQueryService.Get(userCreateDto.SelectedRoleIds);
        foreach (Role role in selectedRoles)
        {
            if (!string.IsNullOrWhiteSpace(role.DefaultClaims))
            {
                int[] roleEndpiontsId = JsonOperation.Unmarshal<int[]>(role.DefaultClaims); //check
                List<string> roleEndpointsValue = await _endpointQueryService.GetAuthValue(roleEndpiontsId);
                if (roleEndpointsValue.Any())
                {
                    userRoleEndpoints = roleEndpointsValue.Select(e => new EndpointInfo(e, role.Id));
                }
            }
        }

        IEnumerable<EndpointInfo> allEndpoints = userRoleEndpoints.Union(userDirectEndpoints);
        IEnumerable<EndpointInfo> distinctEndpoints = allEndpoints.GroupBy(e => e.endpoint).Select(grp => new EndpointInfo(grp.Key, grp.FirstOrDefault()?.roleId));
        return distinctEndpoints;
    }

    public record EndpointInfo(string endpoint, int? roleId);
    public async Task<UserPersonalGetDto> UpdateAsync(UserUpdateDto userUpdateDto, IAppUser currentUser, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(userUpdateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        } //

        LogInfo logInfo = DeviceDetection.GetLogInfo(_contextAccessor.HttpContext.Request);
        string logInfoString = JsonOperation.Marshal(logInfo);
        Guid operationGroupId = Guid.NewGuid();
        User userInDb = await _userQueryService.Get(userUpdateDto.Id);
        User user = _mapper.Map<User>(userInDb);
        UserPersonalGetDto userPersinalInfo = new(user.Id, user.FullName, user.DisplayName, user.Username, user.Mobile);
        user.Mobile = userUpdateDto.Mobile;
        user.FullName = userUpdateDto.FullName;
        user.DisplayName = userUpdateDto.DisplayName;
        ICollection<UserClaim> previousClaims = await _userClaimsQueryService.Get(userUpdateDto.Id);
        ICollection<UserRole> previousRoles = await _userRoleQueryService.Get(userUpdateDto.Id);
        _userClaimCommandService.Remove(previousClaims.Where(user => user.ClaimTypeId != ClaimType.RequestKartable).ToList(), logInfoString);
        _userRoleCommandService.Remove(previousRoles, logInfoString);
        int zoneCount = await _zoneCountQueryAddhoc.GetCount(userUpdateDto.SelectedZoneIds, cancellationToken);
        //List<string> endpointValue = await _endpointQueryService.GetAuthValue(userUpdateDto.SelectedEndpointIds.Distinct().ToArray());
        List<EndpointRoleId> endpointValue = await UpdateGetEndpointsValueNonAdmin(userUpdateDto);
        Validate(zoneCount, userUpdateDto.SelectedZoneIds.Count(), endpointValue.Count(), userUpdateDto.SelectedEndpointIds.Count());
        ICollection<UserClaim> zones = CreateUserClaim(userUpdateDto.SelectedZoneIds.Select(x => x.ToString()).Distinct().ToList(), ClaimType.ZoneId, logInfoString, operationGroupId, userUpdateDto.Id);
        ICollection<UserClaim> endpionts = CreateUserClaim(endpointValue, ClaimType.Endpoint, logInfoString, operationGroupId, userUpdateDto.Id);
        string defaultZoneIdValue = userUpdateDto.SelectedZoneIds.Count() > 30 ? "130000" : userUpdateDto.SelectedZoneIds.Select(x => x.ToString()).First();
        ICollection<UserClaim> defaultZoneId = CreateUserClaim(new List<string> { defaultZoneIdValue }, ClaimType.DefaultZoneId, logInfoString, operationGroupId, userUpdateDto.Id);
        List<UserClaim> userCliams = zones.Union(endpionts).Union(defaultZoneId).ToList();
        ICollection<UserRole> userRoles = CreateUserRoles(userUpdateDto.SelectedRoleIds, logInfoString, operationGroupId, userUpdateDto.Id);
        await _userClaimCommandService.Add(userCliams);
        await _userRoleCommandService.Add(userRoles);
        return userPersinalInfo;
    }

    private async Task<List<string>> UpdateGetEndpointsValueAdmin(UserUpdateDto userUpdateDto)
    {
        List<string> endpointValue = await _endpointQueryService.GetAuthValue(userUpdateDto.SelectedEndpointIds.Distinct().ToArray());
        ICollection<Role> selectedRoles = await _roleQueryService.Get(userUpdateDto.SelectedRoleIds.Distinct().ToArray());
        foreach (Role role in selectedRoles)
        {
            if (!string.IsNullOrWhiteSpace(role.DefaultClaims))
            {
                int[] roleEndpiontsId = JsonOperation.Unmarshal<int[]>(role.DefaultClaims); //check
                List<string> roleEndpointsValue = await _endpointQueryService.GetAuthValue(roleEndpiontsId);
                if (roleEndpointsValue.Any())
                {
                    endpointValue = endpointValue.Union(roleEndpointsValue).ToList();
                }
            }
        }

        return endpointValue.Distinct().ToList();
    }

    private async Task<List<EndpointRoleId>> UpdateGetEndpointsValueNonAdmin(UserUpdateDto userUpdateDto)
    {
        List<EndpointRoleId> endpointValues = new();
        ICollection<Role> selectedRoles = await _roleQueryService.Get(userUpdateDto.SelectedRoleIds.Distinct().ToArray());
        foreach (Role role in selectedRoles)
        {
            if (!string.IsNullOrWhiteSpace(role.DefaultClaims))
            {
                int[] roleEndpiontsId = JsonOperation.Unmarshal<int[]>(role.DefaultClaims); //check
                List<string> roleEndpointsValues = await _endpointQueryService.GetAuthValue(roleEndpiontsId);
                if (roleEndpointsValues.Any())
                {
                    foreach (var endpointVal in roleEndpointsValues)
                    {
                        EndpointRoleId endpointRoleId = new(endpointVal, role.Id);
                        endpointValues.Add(endpointRoleId);
                    }
                //endpointValues = endpointValues.Union(roleEndpointsValues).ToList();
                }
            }
        }

        return endpointValues.Distinct().ToList();
    }

    public record EndpointRoleId(string endpoint, int roleId);
    public async Task DeleteManagerAsync(UserIdDto userId, CancellationToken cancellationToken)
    {
        User user = await _userQueryService.Get(userId.Id);
        if (user == null)
        {
            throw new InvalidDataException();
        }

        user.ValidTo = DateTime.Now;
        user.RemoveLogInfo = "Remove Log Info";
    }

    public async Task<ICollection<UserQueryDto>> AllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<NumericDictionary> zonesInfo = await _t51QueryService.Get();
        ICollection<UserQueryDto> userQueryDtoList = await _userQueryService.GetWithDefaultZone();
        userQueryDtoList.ForEach(u => u.DefaultZoneTitle = zonesInfo.Where(zone => zone.Id == (string.IsNullOrWhiteSpace(u.DefaultZoneId) ? 0 : int.Parse(u.DefaultZoneId))).FirstOrDefault()?.Title ?? string.Empty);
        return userQueryDtoList;
    }

    public async Task<ICollection<UserBriefQueryDto>> BriefAsync(CancellationToken cancellationToken)
    {
        return await _userQueryService.GetBrief(cancellationToken);
    }

    public async Task<ICollection<UserQueryDto>> GridifyAsync(GridifyQuery query, CancellationToken cancellationToken)
    {
        Paging<User> users = await _userQueryService.GetQuery().GridifyAsync(query);
        ICollection<UserQueryDto> usersDto = _mapper.Map<ICollection<UserQueryDto>>(users.Data);
        return usersDto;
    }

    public async Task<UserDisplayDto> GetSingleAsync(Guid userId, CancellationToken cancellationToken)
    {
        User user = await _userQueryService.GetIncludeUserAndClaims(userId);
        UserDisplayDto userDisplayDto = new();
        //if (user.UserClaims.Any())
        //{
        userDisplayDto.AccessTree = await GetSingleCreateAccessTree(user.UserClaims);
        userDisplayDto.LocationTree = await GetSingleCreateLocationTree(user.UserClaims, cancellationToken);
        //}
        userDisplayDto.RoleInfo = await GetSingleCreateRoleInfo(user.UserRoles);
        userDisplayDto.UserInfo = _mapper.Map<UserQueryDto>(user);
        return userDisplayDto;
    }

    private async Task<UserRoleInfo> GetSingleCreateRoleInfo(ICollection<UserRole> userRoles)
    {
        if (userRoles == null)
        {
            return new UserRoleInfo();
        }

        ICollection<Role> roles = await _roleQueryService.Get();
        IEnumerable<UserRoleQueryDto> query =
            from role in roles
            join userRole in userRoles.Distinct()on role.Id equals userRole.RoleId into roleGroup
            from rg in roleGroup.DefaultIfEmpty()select new UserRoleQueryDto()
            {
                Id = role.Id,
                Title = role.Title,
                IsSelected = rg != null
            };
        List<UserRoleQueryDto> userRoleQueryDtos = query.ToList();
        return new UserRoleInfo(userRoleQueryDtos);
    }

    private async Task<LocationTree> GetSingleCreateLocationTree(ICollection<UserClaim> userClaims, CancellationToken cancellationToken)
    {
        List<int> zoneIds = userClaims.Where(userClaim => userClaim.ClaimTypeId == ClaimType.ZoneId).Select(userClaim => int.Parse(userClaim.ClaimValue)).Distinct().ToList();
        LocationTree locationTree = await _locationTreeAdHoc.Handle(zoneIds, cancellationToken);
        return locationTree;
    }

    private async Task<AccessTreeValueKeyDto> GetSingleCreateAccessTree(ICollection<UserClaim> userClaims)
    {
        List<string> authValues = userClaims.Where(userClaim => userClaim.ClaimTypeId == ClaimType.Endpoint).Select(userClaim => userClaim.ClaimValue).ToList();
        ICollection<Endpoint> endpoints = await _endpointQueryService.GetIncludeActiveAll();
        List<int> toBeSelectedEndpointIds = endpoints.Where(endpoint => authValues.Contains(endpoint.AuthValue)).Select(endpoint => endpoint.Id).Distinct().ToList();
        AccessTreeValueKeyDto accessTree = endpoints.CreateAccessTree(toBeSelectedEndpointIds);
        return accessTree;
    }
}

