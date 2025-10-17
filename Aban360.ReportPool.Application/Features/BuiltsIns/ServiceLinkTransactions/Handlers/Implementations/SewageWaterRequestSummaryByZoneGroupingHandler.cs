using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Implementations
{
    internal sealed class SewageWaterRequestSummaryByZoneGroupingHandler : ISewageWaterRequestSummaryByZoneGroupingHandler
    {
        private readonly ISewageWaterRequestSummaryByZoneQueryService _sewageWaterRequestSummaryByZoneQuery;
        private readonly IValidator<SewageWaterRequestInputDto> _validator;
        public SewageWaterRequestSummaryByZoneGroupingHandler(
            ISewageWaterRequestSummaryByZoneQueryService sewageWaterRequestSummaryByZoneQuery,
            IValidator<SewageWaterRequestInputDto> validator)
        {
            _sewageWaterRequestSummaryByZoneQuery = sewageWaterRequestSummaryByZoneQuery;
            _sewageWaterRequestSummaryByZoneQuery.NotNull(nameof(sewageWaterRequestSummaryByZoneQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<SewageWaterRequestHeaderOutputDto, ReportOutput<SewageWaterRequestSummaryByZoneIdGroupingDataOutputDto, SewageWaterRequestSummaryByZoneIdGroupingDataOutputDto>>> Handle(SewageWaterRequestInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var result = await _sewageWaterRequestSummaryByZoneQuery.Get(input);
            IEnumerable<ReportOutput<SewageWaterRequestSummaryByZoneIdGroupingDataOutputDto, SewageWaterRequestSummaryByZoneIdGroupingDataOutputDto>> dataGroup = result
                .ReportData
                .GroupBy(m => m.RegionTitle) // فقط بر اساس RegionId گروه‌بندی
                .Select(g => new ReportOutput<SewageWaterRequestSummaryByZoneIdGroupingDataOutputDto, SewageWaterRequestSummaryByZoneIdGroupingDataOutputDto>
                (
                    result.Title,
                    new SewageWaterRequestSummaryByZoneIdGroupingDataOutputDto
                    {
                        ItemTitle = g.First().RegionTitle,
                        CustomerCount = g.Sum(x => x.CustomerCount),
                        TotalUnit = g.Sum(x => x.TotalUnit),
                        UnSpecified = g.Sum(x => x.UnSpecified),
                        Field0_5 = g.Sum(x => x.Field0_5),
                        Field0_75 = g.Sum(x => x.Field0_75),
                        Field1 = g.Sum(x => x.Field1),
                        Field1_2 = g.Sum(x => x.Field1_2),
                        Field1_5 = g.Sum(x => x.Field1_5),
                        Field2 = g.Sum(x => x.Field2),
                        Field3 = g.Sum(x => x.Field3),
                        Field4 = g.Sum(x => x.Field4),
                        Field5 = g.Sum(x => x.Field5),
                        MoreThan6 = g.Sum(x => x.MoreThan6)
                    },
                    g.Select(v => new SewageWaterRequestSummaryByZoneIdGroupingDataOutputDto
                    {
                        ItemTitle = v.ZoneTitle,
                        CustomerCount = v.CustomerCount,
                        TotalUnit = v.TotalUnit,
                        UnSpecified = v.UnSpecified,
                        Field0_5 = v.Field0_5,
                        Field0_75 = v.Field0_75,
                        Field1 = v.Field1,
                        Field1_2 = v.Field1_2,
                        Field1_5 = v.Field1_5,
                        Field2 = v.Field2,
                        Field3 = v.Field3,
                        Field4 = v.Field4,
                        Field5 = v.Field5,
                        MoreThan6 = v.MoreThan6
                    })
                ))
                .ToList();
            ReportOutput<SewageWaterRequestHeaderOutputDto, ReportOutput<SewageWaterRequestSummaryByZoneIdGroupingDataOutputDto, SewageWaterRequestSummaryByZoneIdGroupingDataOutputDto>> finalData = new(result.Title, result.ReportHeader, dataGroup);

            return finalData;
        }

        public async Task<ReportOutput<SewageWaterRequestHeaderOutputDto, SewageWaterRequestSummaryByZoneIdGroupingDataOutputDto>> HandleFlat(SewageWaterRequestInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<SewageWaterRequestHeaderOutputDto, ReportOutput<SewageWaterRequestSummaryByZoneIdGroupingDataOutputDto, SewageWaterRequestSummaryByZoneIdGroupingDataOutputDto>> result = await Handle(input, cancellationToken);


            ICollection<SewageWaterRequestSummaryByZoneIdGroupingDataOutputDto> flatReportData = result
                 .ReportData
                 .SelectMany(f =>
                 {
                     f.ReportHeader.IsFirstRow = true;
                     f.ReportData.Select(data => data.IsFirstRow = false);

                     return new[] { f.ReportHeader }.Concat(f.ReportData);
                 })
                 .ToList();

            ReportOutput<SewageWaterRequestHeaderOutputDto, SewageWaterRequestSummaryByZoneIdGroupingDataOutputDto> flatResult = new
                (result.Title,
                result.ReportHeader,
                flatReportData);

            return flatResult;
        }
    }
}
