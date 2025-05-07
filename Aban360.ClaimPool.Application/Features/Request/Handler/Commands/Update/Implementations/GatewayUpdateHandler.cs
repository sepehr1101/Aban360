using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Entities;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Implementations
{
    internal sealed class GatewayUpdateHandler : IGatewayUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IGatewayQueryService _getewayQueryService;
        private readonly IValidator<GatewayUpdateDto> _validator;

        public GatewayUpdateHandler(
            IMapper mapper,
            IGatewayQueryService getewayQueryService,
            IValidator<GatewayUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _getewayQueryService = getewayQueryService;
            _getewayQueryService.NotNull(nameof(_getewayQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(GatewayUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            Gateway gateway = await _getewayQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, gateway);
        }
    }
}