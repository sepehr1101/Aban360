using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class WaterNetSalesSummaryHandler : IWaterNetSalesSummaryHandler
    {
        private readonly IWaterNetSalesSummaryQueryService _waterNetSalesSummaryQuery;
        private readonly IValidator<WaterSalesInputDto> _validator;
        public WaterNetSalesSummaryHandler(
            IWaterNetSalesSummaryQueryService waterNetSalesSummaryQuery,
            IValidator<WaterSalesInputDto> validator)
        {
            _waterNetSalesSummaryQuery = waterNetSalesSummaryQuery;
            _waterNetSalesSummaryQuery.NotNull(nameof(waterNetSalesSummaryQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WaterSalesHeaderOutputDto, WaterNetSalesSummaryDataOutputDto>> Handle(WaterSalesInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var result = await _waterNetSalesSummaryQuery.GetInfo(input);
            return result;
        }
    }
}
