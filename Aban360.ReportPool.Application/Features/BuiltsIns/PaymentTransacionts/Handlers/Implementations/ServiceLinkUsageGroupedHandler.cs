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
    internal sealed class ServiceLinkUsageGroupedHandler : IServiceLinkUsageGroupedHandler
    {
        private readonly IServiceLinkUsageGroupedQueryService _serviceLinkUsageGroupedQueryService;
        private readonly IValidator<ServiceLinkWaterItemGroupedInputDto> _validator;
        public ServiceLinkUsageGroupedHandler(
            IServiceLinkUsageGroupedQueryService serviceLinkUsageGroupedQueryService,
            IValidator<ServiceLinkWaterItemGroupedInputDto> validator)
        {
            _serviceLinkUsageGroupedQueryService = serviceLinkUsageGroupedQueryService;
            _serviceLinkUsageGroupedQueryService.NotNull(nameof(serviceLinkUsageGroupedQueryService));

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

            ReportOutput<ServiceLinkWaterItemGroupedHeaderOutputDto, ServiceLinkWaterItemGroupedDataOutputDto> serviceLinkUsageGrouped = await _serviceLinkUsageGroupedQueryService.GetInfo(input);
            return serviceLinkUsageGrouped;
        }
    }
}
