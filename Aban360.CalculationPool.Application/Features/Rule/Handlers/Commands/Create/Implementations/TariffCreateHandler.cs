using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Entties;
using Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Implementations
{
    internal sealed class TariffCreateHandler : ITariffCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffCommandService _tariffCommandService; 
        private readonly IValidator<TariffCreateDto> _validator;

        public TariffCreateHandler(
            IMapper mapper,
            ITariffCommandService tariffCommandService,
            IValidator<TariffCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _tariffCommandService = tariffCommandService;
            _tariffCommandService.NotNull(nameof(tariffCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(TariffCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            Tariff tariff = _mapper.Map<Tariff>(createDto);
            await _tariffCommandService.Add(tariff);
        }
    }
}
