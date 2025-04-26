using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Implementation
{
    internal sealed class OfferingGroupUpdateHandler : IOfferingGroupUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IOfferingGroupQueryService _offeringGroupQueryService;
        private readonly IValidator<OfferingGroupUpdateDto> _validator;
        public OfferingGroupUpdateHandler(
            IMapper mapper,
            IOfferingGroupQueryService offeringGroupQueryService,
            IValidator<OfferingGroupUpdateDto> validator )
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _offeringGroupQueryService = offeringGroupQueryService;
            _offeringGroupQueryService.NotNull(nameof(offeringGroupQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(OfferingGroupUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            OfferingGroup offeringGroup = await _offeringGroupQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, offeringGroup);
        }
    }
}
