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
    internal sealed class SubscriptionTypeUpdateHandler : ISubscriptionTypeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ISubscriptionTypeQueryService _subscriptionTypeQueryService;
        private readonly IValidator<SubscriptionTypeUpdateDto> _validator;
        public SubscriptionTypeUpdateHandler(
            IMapper mapper,
            ISubscriptionTypeQueryService subscriptionTypeQueryService,
            IValidator<SubscriptionTypeUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _subscriptionTypeQueryService = subscriptionTypeQueryService;
            _subscriptionTypeQueryService.NotNull(nameof(_subscriptionTypeQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(SubscriptionTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            SubscriptionType subscriptionType = await _subscriptionTypeQueryService.Get(updateDto.Id);
            _mapper.Map(subscriptionType, updateDto);
        }
    }
}
