using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Create.Implementation
{
    internal sealed class OfferingCreateHandler : IOfferingCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IOfferingCommandService _offeringCommandService;
        private readonly IValidator<OfferingCreateDto> _validator;
        public OfferingCreateHandler(
            IMapper mapper,
            IOfferingCommandService offeringCommandService,
            IValidator<OfferingCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _offeringCommandService = offeringCommandService;
            _offeringCommandService.NotNull(nameof(offeringCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(OfferingCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            Offering offering = _mapper.Map<Offering>(createDto);
            await _offeringCommandService.Add(offering);
        }
    }
}
