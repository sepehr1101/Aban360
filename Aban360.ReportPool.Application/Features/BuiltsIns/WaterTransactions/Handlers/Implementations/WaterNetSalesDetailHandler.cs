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
    internal sealed class WaterNetSalesDetailHandler : IWaterNetSalesDetailHandler
    {
        private readonly IWaterNetSalesDetailQueryService _waterNetSalesDetailQuery;
        private readonly IValidator<WaterSalesInputDto> _validator;
        public WaterNetSalesDetailHandler(
            IWaterNetSalesDetailQueryService waterNetSalesDetailQuery,
            IValidator<WaterSalesInputDto> validator)
        {
            _waterNetSalesDetailQuery = waterNetSalesDetailQuery;
            _waterNetSalesDetailQuery.NotNull(nameof(waterNetSalesDetailQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WaterSalesHeaderOutputDto, WaterNetSalesDetailDataOutputDto>> Handle(WaterSalesInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var result = await _waterNetSalesDetailQuery.GetInfo(input);
            return result;
        }
    }
}
