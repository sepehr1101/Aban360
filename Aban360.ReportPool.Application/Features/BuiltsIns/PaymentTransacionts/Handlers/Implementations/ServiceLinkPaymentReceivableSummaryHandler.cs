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
    internal sealed class ServiceLinkPaymentReceivableSummaryHandler : IServiceLinkPaymentReceivableSummaryHandler
    {
        private readonly IServiceLinkPaymentReceivableSummaryService _serviceLinkPaymentReceivableQueryService;
        private readonly IValidator<WaterPaymentReceivableInputDto> _validator;
        public ServiceLinkPaymentReceivableSummaryHandler(
            IServiceLinkPaymentReceivableSummaryService serviceLinkPaymentReceivableQueryService,
            IValidator<WaterPaymentReceivableInputDto> validator)
        {
            _serviceLinkPaymentReceivableQueryService = serviceLinkPaymentReceivableQueryService;
            _serviceLinkPaymentReceivableQueryService.NotNull(nameof(serviceLinkPaymentReceivableQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableSummaryDataOutputDto>> Handle(WaterPaymentReceivableInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableSummaryDataOutputDto> waterPaymentReceivable = await _serviceLinkPaymentReceivableQueryService.GetInfo(input);
            return waterPaymentReceivable;
        }
    }
}
