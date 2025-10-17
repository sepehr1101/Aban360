using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    internal sealed class OfferingGroupCreateHandler : IOfferingGroupCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IOfferingGroupCommandService _offeringGroupCommandService;
        private readonly IValidator<OfferingGroupCreateDto> _validator;

        public OfferingGroupCreateHandler(
            IMapper mapper,
            IOfferingGroupCommandService offeringGroupCommandService,
            IValidator<OfferingGroupCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _offeringGroupCommandService = offeringGroupCommandService;
            _offeringGroupCommandService.NotNull(nameof(offeringGroupCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(OfferingGroupCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            OfferingGroup offeringGroup = _mapper.Map<OfferingGroup>(createDto);
            await _offeringGroupCommandService.Add(offeringGroup);
        }
    }
}
