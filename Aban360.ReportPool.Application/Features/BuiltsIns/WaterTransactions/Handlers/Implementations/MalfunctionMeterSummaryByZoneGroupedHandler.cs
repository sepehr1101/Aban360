using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Base;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class MalfunctionMeterSummaryByZoneGroupedHandler : IMalfunctionMeterSummaryByZoneGroupedHandler
    {
        private readonly IMalfunctionMeterSummaryByZoneQueryService _malfunctionMeterSummaryByZoneQueryService;
        private readonly IValidator<MalfunctionMeterInputDto> _validator;
        public MalfunctionMeterSummaryByZoneGroupedHandler(
            IMalfunctionMeterSummaryByZoneQueryService malfunctionMeterSummaryByZoneQueryService,
            IValidator<MalfunctionMeterInputDto> validator)
        {
            _malfunctionMeterSummaryByZoneQueryService = malfunctionMeterSummaryByZoneQueryService;
            _malfunctionMeterSummaryByZoneQueryService.NotNull(nameof(malfunctionMeterSummaryByZoneQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<MalfunctionMeterSummaryHeaderOutputDto, ReportOutput<MalfunctionReportZoneGroupedData, MalfunctionReportZoneGroupedData>>> Handle(MalfunctionMeterInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<MalfunctionMeterSummaryHeaderOutputDto, MalfunctionMeterSummaryDataOutputDto> result = await _malfunctionMeterSummaryByZoneQueryService.Get(input);

            var dataGroup = result.ReportData
            .GroupBy(m => m.RegionTitle)
            .Select(g =>
            {
                // Map all raw DTOs to grouped DTOs
                var mapped = g.Select(MapToGrouped);

                return new ReportOutput<
                    MalfunctionReportZoneGroupedData,
                    MalfunctionReportZoneGroupedData>
                (
                    result.Title,
                    ReportAggregator.AggregateGroup(mapped, g.Key),
                    mapped.Select(v => ReportAggregator.AggregateGroup(new[] { v }, v.ItemTitle))
                );
            })
            .ToList();

            result.ReportHeader.ConsumptionAverage=(float)Math.Round(result.ReportHeader.ConsumptionAverage, 3);

            ReportOutput<MalfunctionMeterSummaryHeaderOutputDto, ReportOutput<MalfunctionReportZoneGroupedData, MalfunctionReportZoneGroupedData>> finalData = new(result.Title, result.ReportHeader, dataGroup);

            return finalData;
        }
        public async Task<ReportOutput<MalfunctionMeterSummaryHeaderOutputDto, MalfunctionReportZoneGroupedData>> HandleFlat(MalfunctionMeterInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<MalfunctionMeterSummaryHeaderOutputDto, ReportOutput<MalfunctionReportZoneGroupedData, MalfunctionReportZoneGroupedData>> result = await Handle(input, cancellationToken);

            ICollection<MalfunctionReportZoneGroupedData> flatData = result
                .ReportData
                .SelectMany(f =>
                {
                    f.ReportHeader.IsFirstRow = true;
                    f.ReportData.Select(d => d.IsFirstRow = false);

                    return new[] { f.ReportHeader }.Concat(f.ReportData);
                }).ToList();

            ReportOutput<MalfunctionMeterSummaryHeaderOutputDto, MalfunctionReportZoneGroupedData> flatResult = new(result.Title, result.ReportHeader, flatData) { };
            return flatResult;
        }

        private MalfunctionReportZoneGroupedData MapToGrouped(MalfunctionMeterSummaryDataOutputDto input)
        {
            return new MalfunctionReportZoneGroupedData
            {
                ItemTitle = input.ItemTitle,
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
