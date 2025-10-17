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
        private readonly IValidator<ServiceLinkPaymentReceivableInputDto> _validator;
        public ServiceLinkPaymentReceivableSummaryHandler(
            IServiceLinkPaymentReceivableSummaryService serviceLinkPaymentReceivableQueryService,
            IValidator<ServiceLinkPaymentReceivableInputDto> validator)
        {
            _serviceLinkPaymentReceivableQueryService = serviceLinkPaymentReceivableQueryService;
            _serviceLinkPaymentReceivableQueryService.NotNull(nameof(serviceLinkPaymentReceivableQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableSummaryDataOutputDto>> Handle(ServiceLinkPaymentReceivableInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableSummaryDataOutputDto> waterPaymentReceivable = await _serviceLinkPaymentReceivableQueryService.GetInfo(input);
            waterPaymentReceivable.ReportData.ForEach(r =>
            {
                r.DueAmount = Math.Abs(r.DueAmount);
                r.OverdueAmount= Math.Abs(r.OverdueAmount);
                r.Amount = r.DueAmount + r.OverdueAmount;
            });
            waterPaymentReceivable.ReportHeader.DueCount=waterPaymentReceivable.ReportData.Sum(r => r.DueCount);
            waterPaymentReceivable.ReportHeader.OverdueCount=waterPaymentReceivable.ReportData.Sum(r => r.OverdueCount);
            waterPaymentReceivable.ReportHeader.Amount=waterPaymentReceivable.ReportData.Sum(r => r.Amount);
         
            return waterPaymentReceivable;
        }
    }
}
