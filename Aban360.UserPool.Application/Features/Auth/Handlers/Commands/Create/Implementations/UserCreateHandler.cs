using Aban360.Common.Categories.UseragentLog;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using Aban360.UserPool.Application.Common.Base;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Constants.Enums;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Implementations
{
    internal sealed class UserCreateHandler : UserBaseCreateOrUpdateService, IUserCreateHandler
    {
        private readonly IUserCommandService _userCommandService;
        private readonly IUserClaimCommandService _userClaimCommandService;
        private readonly IUserRoleCommandService _userRoleCommandService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IZoneCountQueryAddhoc _zoneCountQueryAddhoc;
        private readonly IEndpointQueryService _endpointQueryService;
        private readonly IValidator<UserCreateDto> _userValidator;

        public UserCreateHandler(
            IUserCommandService userCommandService,
            IUserClaimCommandService userClaimCommandService,
            IUserRoleCommandService userRoleCommandService,
            IMapper mapper,
            IHttpContextAccessor contextAccessor,
            IZoneCountQueryAddhoc zoneCountQueryAddhoc,
            IEndpointQueryService endpointQueryService,
            IValidator<UserCreateDto> userValidator)
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

            _userValidator = userValidator;
            _userValidator.NotNull(nameof(userValidator));
        }
        public async Task Handle(UserCreateDto userCreateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _userValidator.ValidateAsync(userCreateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }//


            LogInfo logInfo = DeviceDetection.GetLogInfo(_contextAccessor.HttpContext.Request);
            string logInfoString = JsonOperation.Marshal(logInfo);
            Guid operationGroupId = Guid.NewGuid();

            int zoneCount = await _zoneCountQueryAddhoc.GetCount(userCreateDto.SelectedZoneIds, cancellationToken);
            List<string> endpointValue = await _endpointQueryService.GetAuthValue(userCreateDto.SelectedEndpointIds.ToArray());
            Validate(zoneCount,userCreateDto.SelectedZoneIds.Count(),endpointValue.Count(), userCreateDto.SelectedEndpointIds.Count());

            ICollection<UserClaim> zones = CreateUserClaim(userCreateDto.SelectedZoneIds.Select(x=>x.ToString()).ToList(), ClaimType.ZoneId, logInfoString, operationGroupId, operationGroupId);
            ICollection<UserClaim> endpionts = CreateUserClaim(endpointValue,ClaimType.Endpoint, logInfoString, operationGroupId, operationGroupId);
            ICollection<UserClaim> defaultZoneId = CreateUserClaim(new List<string> { userCreateDto.SelectedZoneIds.Select(x => x.ToString()).First() }, ClaimType.DefaultZoneId, logInfoString, operationGroupId, operationGroupId);
            List<UserClaim> userCliams = zones.Union(endpionts).ToList();

            ICollection<UserRole> userRoles = CreateUserRoles(userCreateDto.SelectedRoleIds, logInfoString, operationGroupId, operationGroupId);
            User user = _mapper.Map<User>(userCreateDto);
            user.Id = operationGroupId;
            user.InsertLogInfo = logInfoString;
            user.Password = await SecurityOperations.GetSha512Hash(userCreateDto.Password);
            await _userCommandService.Add(user);
            await _userClaimCommandService.Add(userCliams);
            await _userRoleCommandService.Add(userRoles);
        }
    }
}