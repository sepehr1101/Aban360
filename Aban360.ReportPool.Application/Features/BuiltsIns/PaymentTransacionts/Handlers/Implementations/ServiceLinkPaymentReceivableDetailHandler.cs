using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Implementations
{
    internal sealed class ServiceLinkPaymentReceivableDetailHandler : IServiceLinkPaymentReceivableDetailHandler
    {
        private readonly IServiceLinkPaymentReceivableQueryDetailService _serviceLinkPaymentReceivableQueryService;
        private readonly IValidator<ServiceLinkPaymentReceivableInputDto> _validator;
        public ServiceLinkPaymentReceivableDetailHandler(
            IServiceLinkPaymentReceivableQueryDetailService serviceLinkPaymentReceivableQueryService,
            IValidator<ServiceLinkPaymentReceivableInputDto> validator)
        {
            _serviceLinkPaymentReceivableQueryService = serviceLinkPaymentReceivableQueryService;
            _serviceLinkPaymentReceivableQueryService.NotNull(nameof(serviceLinkPaymentReceivableQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableDataOutputDto>> Handle(ServiceLinkPaymentReceivableInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableDataOutputDto> waterPaymentReceivable = await _serviceLinkPaymentReceivableQueryService.GetInfo(input);
            int dueCount = 0, overdueCount = 0;
            long dueAmount = 0, overdueAmount = 0;
            waterPaymentReceivable.ReportData.ForEach(r =>
            {
                r.Amount=Math.Abs(r.Amount);
                if (r.AmountState == ReportLiterals.Due)
                {
                    dueCount++;
                    dueAmount += r.Amount;
                }
                else
                {
                    overdueCount++;
                    overdueAmount += r.Amount;
                }
            });
            waterPaymentReceivable.ReportHeader.DueAmount = dueAmount;
            waterPaymentReceivable.ReportHeader.DueCount = dueCount;
            waterPaymentReceivable.ReportHeader.OverdueAmount = overdueAmount;
            waterPaymentReceivable.ReportHeader.OverdueCount = overdueCount;
            waterPaymentReceivable.ReportHeader.Amount = waterPaymentReceivable.ReportHeader.OverdueAmount + waterPaymentReceivable.ReportHeader.DueAmount;
            return waterPaymentReceivable;
        }
    }
}
