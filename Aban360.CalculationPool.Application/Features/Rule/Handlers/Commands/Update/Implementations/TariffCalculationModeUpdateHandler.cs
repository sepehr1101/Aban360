using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Update.Implementations
{
    internal sealed class TariffCalculationModeUpdateHandler : ITariffCalculationModeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffCalculationModeQueryService _tariffCalculationModeQueryService; 
        private readonly IValidator<TariffCalculationModeUpdateDto> _validator;

        public TariffCalculationModeUpdateHandler(
            IMapper mapper,
            ITariffCalculationModeQueryService tariffCalculationModeQueryService,
            IValidator<TariffCalculationModeUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _tariffCalculationModeQueryService = tariffCalculationModeQueryService;
            _tariffCalculationModeQueryService.NotNull(nameof(tariffCalculationModeQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(TariffCalculationModeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            TariffCalculationMode tariffCalculationMode = await _tariffCalculationModeQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, tariffCalculationMode);
        }
    }
}
