using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations
{
    internal sealed class ServiceLinkRegisterManualHandler : IServiceLinkRegisterManualHandler
    {
        private readonly IValidator<ServiceLinkRegisterManualInputDto> _validator;
        private readonly ICustomerInfoService _customerInfoService;
        public ServiceLinkRegisterManualHandler(
            IValidator<ServiceLinkRegisterManualInputDto> validator,
            ICustomerInfoService customerInfoService)
        {
            _validator = validator;
            _validator.NotNull(nameof(validator));

            _customerInfoService = customerInfoService;
            _customerInfoService.NotNull(nameof(customerInfoService));
        }

        public async Task Handle(ServiceLinkRegisterManualInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ZoneIdAndCustomerNumberGetDto zoneIdAndCustomerNumber = await _customerInfoService.GetZoneIdAndCustomerNumber(input.BillId);
            //register
        }
    }
}
