using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Implementations
{
    internal sealed class TariffConstantCreateHandler : ITariffConstantCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffConstantCommandService _tariffConstantCommandService;
        private readonly IValidator<TariffConstantCreateDto> _validator;

        public TariffConstantCreateHandler(
            IMapper mapper,
            ITariffConstantCommandService tariffConstantCommandService,
            IValidator<TariffConstantCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _tariffConstantCommandService = tariffConstantCommandService;
            _tariffConstantCommandService.NotNull(nameof(tariffConstantCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(TariffConstantCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            TariffConstant tariffConstant = _mapper.Map<TariffConstant>(createDto);
            await _tariffConstantCommandService.Add(tariffConstant);
        }
    }
}
