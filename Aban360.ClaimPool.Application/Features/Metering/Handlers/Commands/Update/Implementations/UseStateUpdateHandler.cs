using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Implementations
{
    internal sealed class UseStateUpdateHandler : IUseStateUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUseStateQueryService _useStateQueryService;
        private readonly IValidator<UseStateUpdateDto> _validator;

        public UseStateUpdateHandler(
            IMapper mapper,
            IUseStateQueryService useStateQueryService,
            IValidator<UseStateUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _useStateQueryService = useStateQueryService;
            _useStateQueryService.NotNull(nameof(useStateQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(UseStateUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            UseState useState = await _useStateQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, useState);
        }
    }
}
