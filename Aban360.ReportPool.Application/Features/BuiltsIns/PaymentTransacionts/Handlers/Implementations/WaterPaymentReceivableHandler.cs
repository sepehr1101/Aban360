﻿using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Implementations
{
    internal sealed class WaterPaymentReceivableHandler : IWaterPaymentReceivableHandler
    {
        private readonly IWaterPaymentReceivableQueryService _waterPaymentReceivableQueryService;
        private readonly IValidator<WaterPaymentReceivableInputDto> _validator;
        public WaterPaymentReceivableHandler(
            IWaterPaymentReceivableQueryService waterPaymentReceivableQueryService,
            IValidator<WaterPaymentReceivableInputDto> validator)
        {
            _waterPaymentReceivableQueryService = waterPaymentReceivableQueryService;
            _waterPaymentReceivableQueryService.NotNull(nameof(waterPaymentReceivableQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableDataOutputDto>> Handle(WaterPaymentReceivableInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableDataOutputDto> waterPaymentReceivable = await _waterPaymentReceivableQueryService.GetInfo(input);
            return waterPaymentReceivable;
        }
    }
}
