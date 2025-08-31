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
    internal sealed class WaterPaymentDetailHandler : IWaterPaymentDetailHandler
    {
        private readonly IWaterPaymentDetailQueryService _waterPaymentDetailQueryService;
        private readonly IValidator<PaymentDetailInputDto> _validator;
        public WaterPaymentDetailHandler(
            IWaterPaymentDetailQueryService waterPaymentDetailQueryService,
            IValidator<PaymentDetailInputDto> validator)
        {
            _waterPaymentDetailQueryService = waterPaymentDetailQueryService;
            _waterPaymentDetailQueryService.NotNull(nameof(waterPaymentDetailQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<PaymentDetailHeaderOutputDto, PaymentDetailDataOutputDto>> Handle(PaymentDetailInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<PaymentDetailHeaderOutputDto, PaymentDetailDataOutputDto> waterPaymentDetail = await _waterPaymentDetailQueryService.GetInfo(input);
            return waterPaymentDetail;
        }
    }
}
