using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Timing;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Implementations
{
    internal sealed class SewageWaterDistanceofRequestAndInstallationSummaryHandler : ISewageWaterDistanceofRequestAndInstallationSummaryHandler
    {
        private readonly ISewageWaterDistanceofRequestAndInstallationSummaryQueryService _sewageWaterDistanceofRequestAndInstallationSummaryQuery;
        private readonly IValidator<SewageWaterDistanceofRequestAndInstallationInputDto> _validator;
        public SewageWaterDistanceofRequestAndInstallationSummaryHandler(
            ISewageWaterDistanceofRequestAndInstallationSummaryQueryService sewageWaterDistanceofRequestAndInstallationSummaryQuery,
            IValidator<SewageWaterDistanceofRequestAndInstallationInputDto> validator)
        {
            _sewageWaterDistanceofRequestAndInstallationSummaryQuery = sewageWaterDistanceofRequestAndInstallationSummaryQuery;
            _sewageWaterDistanceofRequestAndInstallationSummaryQuery.NotNull(nameof(sewageWaterDistanceofRequestAndInstallationSummaryQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, SewageWaterDistanceofRequestAndInstallationSummaryDataOutputDto>> Handle(SewageWaterDistanceofRequestAndInstallationInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var result = await _sewageWaterDistanceofRequestAndInstallationSummaryQuery.Get(input);

            ICollection<float> distances=new List<float>();
            foreach (var item in result.ReportData)
            {
                item.DistanceAverageText = CalculationDistanceDate.ConvertDaysToDate((int)item.DistanceAverage);
                distances.Add(item.DistanceAverage);
            }
            int averageDistance = (int)distances.Sum() / (result.ReportHeader.RecordCount <= 0 ? 1 : result.ReportHeader.RecordCount);
            result.ReportHeader.AvergeDistance=CalculationDistanceDate.ConvertDaysToDate(averageDistance);
            result.ReportHeader.MaxDistance = CalculationDistanceDate.ConvertDaysToDate((int)distances.Max());

            return result;
        }
    }
}
