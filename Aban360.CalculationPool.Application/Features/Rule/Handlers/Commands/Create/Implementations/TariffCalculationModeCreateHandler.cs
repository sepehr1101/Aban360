using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Implementations
{
    internal sealed class TariffCalculationModeCreateHandler : ITariffCalculationModeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffCalculationModeCommandService _tariffCalculationModeCommandService;
        private readonly IValidator<TariffCalculationModeCreateDto> _validator;

        public TariffCalculationModeCreateHandler(
            IMapper mapper,
            ITariffCalculationModeCommandService tariffCalculationModeCommandService,
            IValidator<TariffCalculationModeCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _tariffCalculationModeCommandService = tariffCalculationModeCommandService;
            _tariffCalculationModeCommandService.NotNull(nameof(tariffCalculationModeCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(TariffCalculationModeCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            TariffCalculationMode tariffCalculationMode = _mapper.Map<TariffCalculationMode>(createDto);
            await _tariffCalculationModeCommandService.Add(tariffCalculationMode);
        }
    }
}
