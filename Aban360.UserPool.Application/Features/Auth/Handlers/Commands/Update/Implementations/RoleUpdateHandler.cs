using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Threading;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Implementations
{
    internal sealed class RoleUpdateHandler : IRoleUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRoleQueryService _roleQueryService;
        private readonly IEndpointQueryService _endpointQueryService;
        private readonly IValidator<RoleUpdateDto> _roleValidator;
        public RoleUpdateHandler(
            IMapper mapper,
            IRoleQueryService roleQueryService,
            IEndpointQueryService endpointQueryService,
            IValidator<RoleUpdateDto> roleValidator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _roleQueryService = roleQueryService;
            _roleQueryService.NotNull(nameof(roleQueryService));

            _endpointQueryService = endpointQueryService;
            _endpointQueryService.NotNull(nameof(endpointQueryService));
            
            _roleValidator = roleValidator;
            _roleValidator.NotNull(nameof(roleValidator));
        }

        public async Task Handle(RoleUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _roleValidator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }//


            Role role = await _roleQueryService.Get(updateDto.Id);
            List<string> endpointValue = await _endpointQueryService.GetAuthValue(updateDto.SelectedEndpointIds);

            if (updateDto.SelectedEndpointIds is not null && endpointValue.Count() == updateDto.SelectedEndpointIds.Count())
            {
                role.DefaultClaims = JsonOperation.Marshal(updateDto.SelectedEndpointIds);
            }
            _mapper.Map(updateDto, role);
        }
    }
}
