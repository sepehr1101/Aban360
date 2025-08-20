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
    internal sealed class SewageWaterDistanceofRequestAndInstallationDetailHandler : ISewageWaterDistanceofRequestAndInstallationDetailHandler
    {
        private readonly ISewageWaterDistanceofRequestAndInstallationDetailQueryService _sewageWaterDistanceofRequestAndInstallationDetailQuery;
        private readonly IValidator<SewageWaterDistanceofRequestAndInstallationInputDto> _validator;
        public SewageWaterDistanceofRequestAndInstallationDetailHandler(
            ISewageWaterDistanceofRequestAndInstallationDetailQueryService sewageWaterDistanceofRequestAndInstallationDetailQuery,
            IValidator<SewageWaterDistanceofRequestAndInstallationInputDto> validator)
        {
            _sewageWaterDistanceofRequestAndInstallationDetailQuery = sewageWaterDistanceofRequestAndInstallationDetailQuery;
            _sewageWaterDistanceofRequestAndInstallationDetailQuery.NotNull(nameof(sewageWaterDistanceofRequestAndInstallationDetailQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, SewageWaterDistanceofRequestAndInstallationDetailDataOutputDto>> Handle(SewageWaterDistanceofRequestAndInstallationInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var result = await _sewageWaterDistanceofRequestAndInstallationDetailQuery.Get(input);

            ICollection<int> distances = new List<int>();
            result.ReportData.ForEach(data =>
            {
                int duration = (int.Parse)(CalculationDistanceDate.CalcDistance(data.RequestDate, data.InstallationDate));
                data.DistanceOfRequestAndInstallation = CalculationDistanceDate.ConvertDaysToDate(duration);

                distances.Add(duration);
            });

            int averageDistance = (int)distances.Sum() / (result.ReportHeader.RecordCount <= 0 ? 1 : result.ReportHeader.RecordCount);
            result.ReportHeader.AvergeDistance = CalculationDistanceDate.ConvertDaysToDate(averageDistance);
            result.ReportHeader.MaxDistance = CalculationDistanceDate.ConvertDaysToDate(distances.Max());

            return result;
        }
    }
}
