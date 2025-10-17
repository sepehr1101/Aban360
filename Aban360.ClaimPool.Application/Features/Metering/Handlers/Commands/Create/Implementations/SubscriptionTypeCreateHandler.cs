using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Implementations
{
    internal sealed class SubscriptionTypeCreateHandler : ISubscriptionTypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ISubscriptionTypeCommandService _subscriptionTypeCommandService;
        private readonly IValidator<SubscriptionTypeCreateDto> _validator;

        public SubscriptionTypeCreateHandler(
            IMapper mapper,
            ISubscriptionTypeCommandService subscriptionTypeCommandService,
            IValidator<SubscriptionTypeCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _subscriptionTypeCommandService = subscriptionTypeCommandService;
            _subscriptionTypeCommandService.NotNull(nameof(_subscriptionTypeCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(SubscriptionTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            SubscriptionType subscriptionType = _mapper.Map<SubscriptionType>(createDto);
            await _subscriptionTypeCommandService.Add(subscriptionType);
        }
    }
}
