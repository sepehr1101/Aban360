using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.CalculationPool.Persistence.Features.Sale.Commands.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Implementations
{
    internal sealed class TankerWaterSetPayHandler : ITankerWaterSetPayHandler
    {
        private readonly ITankerWaterSetPayCommandService _tankerCommandSerivce;
        private readonly ICustomerInfoService _customerInfoSerivce;
        private readonly ICommonMemberQueryService _commonMemberQuerySerivce;
        private readonly IValidator<TankerWaterSetPayInputDto> _validator;
        public TankerWaterSetPayHandler(
            ITankerWaterSetPayCommandService tankerCommandSerivce,
            ICustomerInfoService customerInfoSerivce,
            ICommonMemberQueryService commonMemberQuerySerivce,
            IValidator<TankerWaterSetPayInputDto> validator)
        {
            _tankerCommandSerivce = tankerCommandSerivce;
            _tankerCommandSerivce.NotNull(nameof(tankerCommandSerivce));

            _customerInfoSerivce = customerInfoSerivce;
            _customerInfoSerivce.NotNull(nameof(customerInfoSerivce));

            _commonMemberQuerySerivce = commonMemberQuerySerivce;
            _commonMemberQuerySerivce.NotNull(nameof(commonMemberQuerySerivce));

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

            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQuerySerivce.Get(input.BillId);
            TankerWaterSetPayWithZoneIdAndCustomerNumberInputDto tankerSetPayDto = new(input.PaymentId, zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber);
            await _tankerCommandSerivce.Insert(tankerSetPayDto);

        }
    }
}
