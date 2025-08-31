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
    internal sealed class WaterRawSalesSummaryHandler : IWaterRawSalesSummaryHandler
    {
        private readonly IWaterRawSalesSummaryQueryService _waterRawSalesSummaryQuery;
        private readonly IValidator<WaterSalesInputDto> _validator;
        public WaterRawSalesSummaryHandler(
            IWaterRawSalesSummaryQueryService waterRawSalesSummaryQuery,
            IValidator<WaterSalesInputDto> validator)
        {
            _waterRawSalesSummaryQuery = waterRawSalesSummaryQuery;
            _waterRawSalesSummaryQuery.NotNull(nameof(waterRawSalesSummaryQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WaterSalesHeaderOutputDto, WaterRawSalesSummaryDataOutputDto>> Handle(WaterSalesInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var result = await _waterRawSalesSummaryQuery.GetInfo(input);
            return result;
        }
    }
}
