using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Entities;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class GatewayCreateHandler : IGatewayCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IGatewayCommandService _getewayCommandService;
        private readonly IValidator<GatewayCreateDto> _validator;

        public GatewayCreateHandler(
            IMapper mapper,
            IGatewayCommandService getewayCommandService,
            IValidator<GatewayCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _getewayCommandService = getewayCommandService;
            _getewayCommandService.NotNull(nameof(_getewayCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(GatewayCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            Gateway geteway = _mapper.Map<Gateway>(createDto);
            await _getewayCommandService.Add(geteway);
        }
    }
}