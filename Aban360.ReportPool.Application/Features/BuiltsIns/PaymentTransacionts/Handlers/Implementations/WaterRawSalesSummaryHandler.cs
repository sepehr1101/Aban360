using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Implementations
{
    internal sealed class WaterRawSalesSummaryHandler : IWaterRawSalesSummaryHandler
    {
        private readonly IWaterRawSalesSummaryQueryService _waterRawSalesSummaryQueryService;
        private readonly IValidator<WaterSalesInputDto> _validator;
        public WaterRawSalesSummaryHandler(
            IWaterRawSalesSummaryQueryService waterRawSalesSummaryQueryService,
            IValidator<WaterSalesInputDto> validator)
        {
            _waterRawSalesSummaryQueryService = waterRawSalesSummaryQueryService;
            _waterRawSalesSummaryQueryService.NotNull(nameof(waterRawSalesSummaryQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WaterSalesHeaderOutputDto, WaterRawSalesSummaryDataOutputDto>> Handle(WaterSalesInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<WaterSalesHeaderOutputDto, WaterRawSalesSummaryDataOutputDto> waterRawSalesSummary = await _waterRawSalesSummaryQueryService.GetInfo(input);
            return waterRawSalesSummary;
        }
    }
}
