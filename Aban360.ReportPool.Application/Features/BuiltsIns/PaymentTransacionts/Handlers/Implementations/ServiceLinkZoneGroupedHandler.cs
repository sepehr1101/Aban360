using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Implementations
{
    internal sealed class ServiceLinkZoneGroupedHandler : IServiceLinkZoneGroupedHandler
    {
        private readonly IServiceLinkZoneGroupedQueryService _serviceLinkZoneGroupedQueryService;
        private readonly IValidator<ServiceLinkWaterItemGroupedInputDto> _validator;
        public ServiceLinkZoneGroupedHandler(
            IServiceLinkZoneGroupedQueryService serviceLinkZoneGroupedQueryService,
            IValidator<ServiceLinkWaterItemGroupedInputDto> validator)
        {
            _serviceLinkZoneGroupedQueryService = serviceLinkZoneGroupedQueryService;
            _serviceLinkZoneGroupedQueryService.NotNull(nameof(serviceLinkZoneGroupedQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ServiceLinkWaterItemGroupedHeaderOutputDto, ServiceLinkWaterItemGroupedDataOutputDto>> Handle(ServiceLinkWaterItemGroupedInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<ServiceLinkWaterItemGroupedHeaderOutputDto, ServiceLinkWaterItemGroupedDataOutputDto> serviceLinkZoneGrouped = await _serviceLinkZoneGroupedQueryService.GetInfo(input);
            return serviceLinkZoneGrouped;
        }
    }
}
