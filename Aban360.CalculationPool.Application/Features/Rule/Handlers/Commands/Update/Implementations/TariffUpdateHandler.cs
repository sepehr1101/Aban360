using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Entties;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Update.Implementations
{
    internal sealed class TariffUpdateHandler : ITariffUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffQueryService _tariffQueryService;
        private readonly IValidator<TariffUpdateDto> _validator;

        public TariffUpdateHandler(
            IMapper mapper,
            ITariffQueryService tariffQueryService,
            IValidator<TariffUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _tariffQueryService = tariffQueryService;
            _tariffQueryService.NotNull(nameof(tariffQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(TariffUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            Tariff tariff = await _tariffQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, tariff);
        }
    }
}
