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
    internal sealed class TariffConstantUpdateHandler : ITariffConstantUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffConstantQueryService _tariffConstantQueryService;
        private readonly IValidator<TariffConstantUpdateDto> _validator;

        public TariffConstantUpdateHandler(
            IMapper mapper,
            ITariffConstantQueryService tariffConstantQueryService,
            IValidator<TariffConstantUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _tariffConstantQueryService = tariffConstantQueryService;
            _tariffConstantQueryService.NotNull(nameof(tariffConstantQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(TariffConstantUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            TariffConstant tariffConstant = await _tariffConstantQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, tariffConstant);
        }
    }
}
