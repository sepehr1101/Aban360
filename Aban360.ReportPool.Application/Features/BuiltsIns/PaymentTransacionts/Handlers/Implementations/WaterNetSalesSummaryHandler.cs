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
    internal sealed class WaterNetSalesSummaryHandler : IWaterNetSalesSummaryHandler
    {
        private readonly IWaterNetSalesSummaryQueryService _waterNetSalesSummaryQueryService;
        private readonly IValidator<WaterNetSalesSummaryInputDto> _validator;
        public WaterNetSalesSummaryHandler(
            IWaterNetSalesSummaryQueryService waterNetSalesSummaryQueryService,
            IValidator<WaterNetSalesSummaryInputDto> validator)
        {
            _waterNetSalesSummaryQueryService = waterNetSalesSummaryQueryService;
            _waterNetSalesSummaryQueryService.NotNull(nameof(waterNetSalesSummaryQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WaterNetSalesSummaryHeaderOutputDto, WaterNetSalesSummaryDataOutputDto>> Handle(WaterNetSalesSummaryInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<WaterNetSalesSummaryHeaderOutputDto, WaterNetSalesSummaryDataOutputDto> waterNetSalesSummary = await _waterNetSalesSummaryQueryService.GetInfo(input);
            return waterNetSalesSummary;
        }
    }
}
