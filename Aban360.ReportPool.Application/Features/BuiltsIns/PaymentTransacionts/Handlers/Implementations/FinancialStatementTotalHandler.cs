using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Implementations
{
    internal sealed class FinancialStatementTotalHandler : IFinancialStatementTotalHandler
    {
        private readonly IFinancialStatementQueryService _FinancialStatementQueryService;
        private readonly IValidator<FinancialStatementInputDto> _validator;
        private string _title = ReportLiterals.FinancialStatementTotal;
        public FinancialStatementTotalHandler(
            IFinancialStatementQueryService FinancialStatementQueryService,
            IValidator<FinancialStatementInputDto> validator)
        {
            _FinancialStatementQueryService = FinancialStatementQueryService;
            _FinancialStatementQueryService.NotNull(nameof(FinancialStatementQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<FinancialStatementHeaderOutputDto, FinancialStatementDataOutputDto>> Handle(FinancialStatementInputDto input, CancellationToken cancellationToken)
        {
            await InputValiate(input, cancellationToken);

            IEnumerable<FinancialStatementDataOutputDto> data = await _FinancialStatementQueryService.GetWaterTotal(input);
            FinancialStatementHeaderOutputDto header = new()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                Title = _title,
                RecordCount = data?.Count() ?? 0,
                CustomerCount = data?.Sum(s => Convert.ToInt64(s.CustomerCount)) ?? 0,
                ConsumptionTotalUnit = data?.Sum(s => Convert.ToInt64(s.ConsumptionTotalUnit)) ?? 0,
                DailyAverage = data?.Sum(s => Convert.ToDecimal(s.DailyAverage)) ?? 0,
                NetConsumption = data?.Sum(s => Convert.ToInt64(s.NetConsumption)) ?? 0,
                NetAmount = data?.Sum(s => Convert.ToInt64(s.NetAmount)) ?? 0,
                ReturnedConsumption = data?.Sum(s => Convert.ToInt64(s.ReturnedConsumption)) ?? 0,
                ReturnedAmount = data?.Sum(s => Convert.ToInt64(s.ReturnedAmount)) ?? 0,
                DiscountAmount = data?.Sum(s => Convert.ToInt64(s.DiscountAmount)) ?? 0,
                RawConsumption = data?.Sum(s => Convert.ToInt64(s.RawConsumption)) ?? 0,
                RawAmount = data?.Sum(s => Convert.ToInt64(s.RawAmount)) ?? 0,
                RawAmountAverage = data?.Sum(s => Convert.ToDecimal(s.RawAmountAverage)) ?? 0,
                ConsumptionAverageInMonth = data?.Sum(s => Convert.ToDecimal(s.ConsumptionAverageInMonth)) ?? 0,
            };

            ReportOutput<FinancialStatementHeaderOutputDto, FinancialStatementDataOutputDto> result = new(_title, header, data);
            return result;
        }
        private async Task InputValiate(FinancialStatementInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
    }
}
