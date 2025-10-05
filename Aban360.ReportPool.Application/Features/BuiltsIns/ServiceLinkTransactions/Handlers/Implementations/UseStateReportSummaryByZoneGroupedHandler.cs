using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Base;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Implementations
{
    internal sealed class UseStateReportSummaryByZoneGroupedHandler : IUseStateReportSummaryByZoneGroupedHandler
    {
        private readonly IUseStateReportSummaryByZoneQueryService _userStateReportQueryService;
        private readonly IValidator<UseStateReportInputDto> _validator;
        public UseStateReportSummaryByZoneGroupedHandler(
            IUseStateReportSummaryByZoneQueryService userStateReportQueryService,
            IValidator<UseStateReportInputDto> validator)
        {
            _userStateReportQueryService = userStateReportQueryService;
            _userStateReportQueryService.NotNull(nameof(userStateReportQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<UseStateReportHeaderSummaryOutputDto, ReportOutput<UseStateReportSummaryByZoneGroupedDataOutputDto, UseStateReportSummaryByZoneGroupedDataOutputDto>>> Handle(UseStateReportInputDto input, CancellationToken cancellationToken)
        {
            var validationReuslt = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationReuslt.IsValid)
            {
                var message = string.Join(", ", validationReuslt.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }
            ReportOutput<UseStateReportHeaderSummaryOutputDto, UseStateReportSummaryDataOutputDto> result = await _userStateReportQueryService.Get(input);

            #region hard coded
            /* IEnumerable<ReportOutput<UseStateReportSummaryByZoneGroupedDataOutputDto, UseStateReportSummaryByZoneGroupedDataOutputDto>> dataGroup = result
                .ReportData
                .GroupBy(m => m.RegionTitle) // فقط بر اساس RegionId گروه‌بندی
                .Select(g => new ReportOutput<UseStateReportSummaryByZoneGroupedDataOutputDto, UseStateReportSummaryByZoneGroupedDataOutputDto>
                (
                    result.Title,
                    new UseStateReportSummaryByZoneGroupedDataOutputDto
                    {
                        ItemTitle = g.First().RegionTitle,
                        CustomerCount = g.Sum(x => x.CustomerCount),
                        TotalUnit = g.Sum(x => x.TotalUnit),
                        CommercialUnit = g.Sum(x => x.CommercialUnit),
                        DomesticUnit = g.Sum(x => x.DomesticUnit),
                        OtherUnit = g.Sum(x => x.OtherUnit),
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
                    g.Select(v => new UseStateReportSummaryByZoneGroupedDataOutputDto
                    {
                        ItemTitle = v.ZoneTitle,
                        CustomerCount = v.CustomerCount,
                        TotalUnit = v.TotalUnit,
                        CommercialUnit = v.CommercialUnit,
                        DomesticUnit = v.DomesticUnit,
                        OtherUnit = v.OtherUnit,
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
                .ToList();*/
            #endregion

             var dataGroup = result.ReportData
             .GroupBy(m => m.RegionTitle)
             .Select(g =>
             {
                 // Map all raw DTOs to grouped DTOs
                 var mapped = g.Select(MapToGrouped);

                 return new ReportOutput<
                     UseStateReportSummaryByZoneGroupedDataOutputDto,
                     UseStateReportSummaryByZoneGroupedDataOutputDto>
                 (
                     result.Title,
                     ReportAggregator.AggregateGroup(mapped, g.Key),
                     mapped.Select(v => ReportAggregator.AggregateGroup(new[] { v }, v.ItemTitle))
                 );
             })
             .ToList();

            ReportOutput<UseStateReportHeaderSummaryOutputDto, ReportOutput<UseStateReportSummaryByZoneGroupedDataOutputDto, UseStateReportSummaryByZoneGroupedDataOutputDto>> finalData = new(result.Title, result.ReportHeader, dataGroup);

            return finalData;
        }
        private static UseStateReportSummaryByZoneGroupedDataOutputDto MapToGrouped(
            UseStateReportSummaryDataOutputDto input)
        {
            return new UseStateReportSummaryByZoneGroupedDataOutputDto
            {
                ItemTitle = input.ZoneTitle,
                CustomerCount = input.CustomerCount,
                TotalUnit = input.TotalUnit,
                CommercialUnit = input.CommercialUnit,
                DomesticUnit = input.DomesticUnit,
                OtherUnit = input.OtherUnit,
                UnSpecified = input.UnSpecified,
                Field0_5 = input.Field0_5,
                Field0_75 = input.Field0_75,
                Field1 = input.Field1,
                Field1_2 = input.Field1_2,
                Field1_5 = input.Field1_5,
                Field2 = input.Field2,
                Field3 = input.Field3,
                Field4 = input.Field4,
                Field5 = input.Field5,
                MoreThan6 = input.MoreThan6
            };
        }
    }
}
