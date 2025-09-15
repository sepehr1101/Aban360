using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Timing;
using Aban360.ReportPool.Application.Features.Base;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Implementations
{
    internal sealed class ReadingIssueDistanceBillSummaryByZoneGroupedHandler : IReadingIssueDistanceBillSummaryByZoneGroupedHandler
    {
        private readonly IReadingIssueDistanceBillSummaryByZoneQueryService _readingIssueDistanceBillSummaryByZoneQuery;
        private readonly IValidator<ReadingIssueDistanceBillInputDto> _validator;
        public ReadingIssueDistanceBillSummaryByZoneGroupedHandler(
            IReadingIssueDistanceBillSummaryByZoneQueryService readingIssueDistanceBillSummaryByZoneQuery,
            IValidator<ReadingIssueDistanceBillInputDto> validator)
        {
            _readingIssueDistanceBillSummaryByZoneQuery = readingIssueDistanceBillSummaryByZoneQuery;
            _readingIssueDistanceBillSummaryByZoneQuery.NotNull(nameof(readingIssueDistanceBillSummaryByZoneQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ReadingIssueDistanceBillHeaderOutputDto, ReportOutput<ReadingIssueDistanceBillSummryDataOutputDto, ReadingIssueDistanceBillSummryDataOutputDto>>> Handle(ReadingIssueDistanceBillInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<ReadingIssueDistanceBillHeaderOutputDto, ReadingIssueDistanceBillSummryByZoneDataOutputDto> result = await _readingIssueDistanceBillSummaryByZoneQuery.GetInfo(input);
            result.ReportData.ForEach(r =>
            {
                r.UnSpecifiedText = CalculationDistanceDate.ConvertDaysToDate(r.UnSpecified);
                r.Field0_5Text = CalculationDistanceDate.ConvertDaysToDate(r.Field0_5);
                r.Field0_75Text = CalculationDistanceDate.ConvertDaysToDate(r.Field0_75);
                r.Field1Text = CalculationDistanceDate.ConvertDaysToDate(r.Field1);
                r.Field1_2Text = CalculationDistanceDate.ConvertDaysToDate(r.Field1_2);
                r.Field1_5Text = CalculationDistanceDate.ConvertDaysToDate(r.Field1_5);
                r.Field2Text = CalculationDistanceDate.ConvertDaysToDate(r.Field2);
                r.Field3Text = CalculationDistanceDate.ConvertDaysToDate(r.Field3);
                r.Field4Text = CalculationDistanceDate.ConvertDaysToDate(r.Field4);
                r.Field5Text = CalculationDistanceDate.ConvertDaysToDate(r.Field5);
                r.MoreThan6Text = CalculationDistanceDate.ConvertDaysToDate(r.MoreThan6);
            });

            //var dataGroup = result.ReportData
            //    .GroupBy(m => m.RegionTitle)
            //    .Select(g =>
            //    {
            //        var mapped = g.Select(MapToGroupe);

            //        return new ReportOutput
            //        <ReadingIssueDistanceBillSummryDataOutputDto,
            //        ReadingIssueDistanceBillSummryDataOutputDto>
            //        (
            //            result.Title,
            //            ReportAggregator.AggregateGroup(mapped, g.Key),
            //            mapped.Select(v => ReportAggregator.AggregateGroup(new[] { v }, v.ItemTitle)
            //        ));
            //    });
            IEnumerable<ReportOutput<ReadingIssueDistanceBillSummryDataOutputDto, ReadingIssueDistanceBillSummryDataOutputDto>> dataGroup = result
                  .ReportData
                  .GroupBy(m => m.RegionTitle) 
                  .Select(g => new ReportOutput<ReadingIssueDistanceBillSummryDataOutputDto, ReadingIssueDistanceBillSummryDataOutputDto>
                  (
                      result.Title,
                      new ReadingIssueDistanceBillSummryDataOutputDto
                      {
                          ItemTitle = g.First().RegionTitle,
                          CommercialUnit = g.Sum(s => s.CommercialUnit),
                          DomesticUnit = g.Sum(s => s.DomesticUnit),
                          OtherUnit = g.Sum(s => s.OtherUnit),
                          TotalUnit = g.Sum(s => s.TotalUnit),
                          CustomerCount = g.Sum(s => s.CustomerCount),
                          UnSpecified = g.Sum(s => s.UnSpecified),
                          Field0_5 = g.Sum(s => s.Field0_5),
                          Field0_75 = g.Sum(s => s.Field0_75),
                          Field1 = g.Sum(s => s.Field1),
                          Field1_2 = g.Sum(s => s.Field1_2),
                          Field1_5 = g.Sum(s => s.Field1_5),
                          Field2 = g.Sum(s => s.Field2),
                          Field3 = g.Sum(s => s.Field3),
                          Field4 = g.Sum(s => s.Field4),
                          Field5 = g.Sum(s => s.Field5),
                          MoreThan6 = g.Sum(s => s.MoreThan6),
                      },
                      g.Select(v => new ReadingIssueDistanceBillSummryDataOutputDto
                      {
                          ItemTitle = v.ZoneTitle,
                      })
                  ))
                  .ToList();
            ReportOutput<ReadingIssueDistanceBillHeaderOutputDto, ReportOutput<ReadingIssueDistanceBillSummryDataOutputDto, ReadingIssueDistanceBillSummryDataOutputDto>> finalData = new(result.Title, result.ReportHeader, dataGroup);

            return finalData;
        }
        private static ReadingIssueDistanceBillSummryDataOutputDto MapToGroupe(ReadingIssueDistanceBillSummryByZoneDataOutputDto input)
        {
            return new ReadingIssueDistanceBillSummryDataOutputDto()
            {
                ItemTitle = input.ZoneTitle,
                CustomerCount = input.CustomerCount,
                TotalUnit = input.TotalUnit,
                CommercialUnit = input.CommercialUnit,
                DomesticUnit = input.DomesticUnit,
                OtherUnit = input.OtherUnit,

                UnSpecified = input.UnSpecified,
                UnSpecifiedText = input.UnSpecifiedText,

                Field0_5 = input.Field0_5,
                Field0_5Text = input.Field0_5Text,

                Field0_75 = input.Field0_75,
                Field0_75Text = input.Field0_75Text,

                Field1 = input.Field1,
                Field1Text = input.Field1Text,

                Field1_2 = input.Field1_2,
                Field1_2Text = input.Field1_2Text,

                Field1_5 = input.Field1_5,
                Field1_5Text = input.Field1_5Text,

                Field2 = input.Field2,
                Field2Text = input.Field2Text,

                Field3 = input.Field3,
                Field3Text = input.Field3Text,

                Field4 = input.Field4,
                Field4Text = input.Field4Text,

                Field5 = input.Field5,
                Field5Text = input.Field5Text,

                MoreThan6 = input.MoreThan6,
                MoreThan6Text = input.MoreThan6Text
            };
        }

    }
}
