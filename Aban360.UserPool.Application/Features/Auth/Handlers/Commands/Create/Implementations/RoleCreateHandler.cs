﻿using Aban360.Common.Categories.UseragentLog;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Implementations
{
    internal sealed class RoleCreateHandler : IRoleCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRoleCommandService _roleCommandService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RoleCreateHandler(
            IMapper mapper,
            IRoleCommandService roleCommandService,
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _roleCommandService = roleCommandService;
            _roleCommandService.NotNull(nameof(roleCommandService));

            _httpContextAccessor = httpContextAccessor;
            _httpContextAccessor.NotNull(nameof(_httpContextAccessor));
        }

        public async Task Handle(RoleCreateDto createDto, CancellationToken cancellationToken)
        {
            Role role = _mapper.Map<Role>(createDto);
            if (createDto.SelectedEndpointIds is not null && createDto.SelectedEndpointIds.Any())
            {
                role.DefaultClaims = JsonOperation.Marshal(createDto.SelectedEndpointIds);
            }
            LogInfo logInfo = DeviceDetection.GetLogInfo(_httpContextAccessor.HttpContext.Request);
            role.InsertLogInfo = JsonOperation.Marshal(logInfo);
            await _roleCommandService.Add(role);
        }
    }
}
