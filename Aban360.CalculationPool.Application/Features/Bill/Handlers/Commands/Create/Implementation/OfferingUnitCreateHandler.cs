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
    internal sealed class OfferingUnitCreateHandler : IOfferingUnitCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IOfferingUnitCommandService _offeringUnitCommandService;
        private readonly IValidator<OfferingUnitCreateDto> _validator;
        public OfferingUnitCreateHandler(
            IMapper mapper,
            IOfferingUnitCommandService OfferingUnitCommandService,
            IValidator<OfferingUnitCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _offeringUnitCommandService = OfferingUnitCommandService;
            _offeringUnitCommandService.NotNull(nameof(OfferingUnitCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(OfferingUnitCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            OfferingUnit offeringUnit = _mapper.Map<OfferingUnit>(createDto);
            await _offeringUnitCommandService.Add(offeringUnit);
        }
    }
}
