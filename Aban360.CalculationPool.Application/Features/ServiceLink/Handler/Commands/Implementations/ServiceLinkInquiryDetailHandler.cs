using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.CalculationPool.Persistence.Features.ServiceLink.Qeuries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations
{
    internal sealed class ServiceLinkInquiryDetailHandler : IServiceLinkInquiryDetailHandler
    {
        private readonly IServiceLinkDetailQueryService _serviceLinkDetailQueryService;
        private readonly ICustomerInfoService _customerInfoService;
        private readonly IValidator<ServiceLinkInquiryInputDto> _validator;
        public ServiceLinkInquiryDetailHandler(
            IServiceLinkDetailQueryService serviceLinkDetailQueryService,
            ICustomerInfoService customerInfoService,
            IValidator<ServiceLinkInquiryInputDto> validator)
        {
            _serviceLinkDetailQueryService = serviceLinkDetailQueryService;
            _serviceLinkDetailQueryService.NotNull(nameof(serviceLinkDetailQueryService));

            _customerInfoService = customerInfoService;
            _customerInfoService.NotNull(nameof(customerInfoService));
           
            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<IEnumerable<ServiceLinkInquiryOutputDto>> Handle(ServiceLinkInquiryInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ZoneIdAndCustomerNumberGetDto zoneIdAndCustomerNumber = await _customerInfoService.GetZoneIdAndCustomerNumber(input.BillId);
            IEnumerable<ServiceLinkInquiryOutputDto> detail = await _serviceLinkDetailQueryService.Get(input);
            return detail;
        }
    }
}
