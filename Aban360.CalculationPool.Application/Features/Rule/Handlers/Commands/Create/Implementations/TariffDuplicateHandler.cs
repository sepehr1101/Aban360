using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Entties;
using Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentMigrator.Runner.Generators;
using FluentValidation;
using System.Formats.Tar;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Implementations
{
    internal sealed class TariffDuplicateHandler : ITariffDuplicateHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffCommandService _tariffCommandService;
        private readonly ITariffQueryService _tariffQueryService;
        private readonly IValidator<TariffDuplicateDto> _validator;

        public TariffDuplicateHandler(
            IMapper mapper,
            ITariffCommandService tariffCommandService,
            ITariffQueryService tariffQueryService,
            IValidator<TariffDuplicateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _tariffCommandService = tariffCommandService;
            _tariffCommandService.NotNull(nameof(tariffCommandService));

            _tariffQueryService = tariffQueryService;
            _tariffQueryService.NotNull(nameof(tariffQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(TariffDuplicateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
            Tariff oldtTariff = await _tariffQueryService.Get(createDto.Id);
            Tariff newTariff = new Tariff()
            {
                Title = createDto.Title,
                LineItemTypeId = oldtTariff.LineItemTypeId,
                OfferingId = oldtTariff.OfferingId,
                Duration = oldtTariff.Duration,
                Condition = oldtTariff.Condition,
                Consumption = oldtTariff.Consumption,
                Description = oldtTariff.Description,
                Formula = oldtTariff.Formula,
                FromDateJalali = oldtTariff.FromDateJalali,
                ToDateJalali = oldtTariff.ToDateJalali
            };
            await _tariffCommandService.Add(newTariff);
        }
    }
}
