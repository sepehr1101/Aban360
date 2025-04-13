using Aban360.Common.Categories.UseragentLog;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Implementations
{
    internal sealed class RoleCreateHandler : IRoleCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRoleCommandService _roleCommandService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<RoleCreateDto> _roleValidator;
        public RoleCreateHandler(
            IMapper mapper,
            IRoleCommandService roleCommandService,
            IHttpContextAccessor httpContextAccessor,
            IValidator<RoleCreateDto> roleValidator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _roleCommandService = roleCommandService;
            _roleCommandService.NotNull(nameof(roleCommandService));

            _httpContextAccessor = httpContextAccessor;
            _httpContextAccessor.NotNull(nameof(_httpContextAccessor));

            _roleValidator = roleValidator;
            _roleValidator.NotNull(nameof(roleValidator));
        }

        public async Task Handle(RoleCreateDto createDto, CancellationToken cancellationToken)
        {

            var validationResult = await _roleValidator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }//

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
