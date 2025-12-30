using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.CalculationPool.Persistence.Features.Sale.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Implementations
{
    internal sealed class TankerWaterSetPayHandler : ITankerWaterSetPayHandler
    {
        private readonly ITankerWaterSetPayCommandService _tankerCommandSerivce;
        private readonly ICustomerInfoService _customerInfoSerivce;
        private readonly IValidator<TankerWaterSetPayInputDto> _validator;
        public TankerWaterSetPayHandler(
            ITankerWaterSetPayCommandService tankerCommandSerivce,
            ICustomerInfoService customerInfoSerivce,
            IValidator<TankerWaterSetPayInputDto> validator)
        {
            _tankerCommandSerivce = tankerCommandSerivce;
            _tankerCommandSerivce.NotNull(nameof(tankerCommandSerivce));

            _customerInfoSerivce = customerInfoSerivce;
            _customerInfoSerivce.NotNull(nameof(customerInfoSerivce));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(TankerWaterSetPayInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ZoneIdAndCustomerNumberGetDto zoneIdAndCustomerNumber = await _customerInfoSerivce.GetZoneIdAndCustomerNumber(input.BillId);
            TankerWaterSetPayWithZoneIdAndCustomerNumberInputDto tankerSetPayDto = new(input.Id, zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber);
            await _tankerCommandSerivce.Insert(tankerSetPayDto);

        }
    }
}
