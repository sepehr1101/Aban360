using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Base;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class MalfunctionMeterByDurationSummaryByZoneGroupedHandler : IMalfunctionMeterByDurationSummaryByZoneGroupedHandler
    {
        private readonly IMalfunctionMeterByDurationSummaryByZoneQueryService _malfunctionMeterByDurationSummaryByZoneQueryService;
        private readonly IValidator<MalfunctionMeterByDurationInputDto> _validator;
        public MalfunctionMeterByDurationSummaryByZoneGroupedHandler(
            IMalfunctionMeterByDurationSummaryByZoneQueryService malfunctionMeterByDurationSummaryByZoneQueryService,
            IValidator<MalfunctionMeterByDurationInputDto> validator)
        {
            _malfunctionMeterByDurationSummaryByZoneQueryService = malfunctionMeterByDurationSummaryByZoneQueryService;
            _malfunctionMeterByDurationSummaryByZoneQueryService.NotNull(nameof(malfunctionMeterByDurationSummaryByZoneQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, ReportOutput<MalfunctionMeterByDurationSummaryDataOutputDto, MalfunctionMeterByDurationSummaryDataOutputDto>>> Handle(MalfunctionMeterByDurationInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationSummaryByZoneDataOutputDto> result = await _malfunctionMeterByDurationSummaryByZoneQueryService.Get(input);

            var dataGrouped = result.ReportData
                .GroupBy(m => m.RegionTitle)
                .Select(g =>
                {
                    var mapped = g.Select(MapToGrouped);
                    return new ReportOutput
                    <MalfunctionMeterByDurationSummaryDataOutputDto,
                    MalfunctionMeterByDurationSummaryDataOutputDto>
                    (
                        result.Title,
                        ReportAggregator.AggregateGroup(mapped, g.Key),
                        mapped.Select(v => ReportAggregator.AggregateGroup(new[] { v }, v.ItemTitle))
                    );
                });

            ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, ReportOutput<MalfunctionMeterByDurationSummaryDataOutputDto, MalfunctionMeterByDurationSummaryDataOutputDto>> finalData = new(result.Title, result.ReportHeader, dataGrouped);
            return finalData;
        }

        public async Task<ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationSummaryDataOutputDto>> HandleFlat(MalfunctionMeterByDurationInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, ReportOutput<MalfunctionMeterByDurationSummaryDataOutputDto, MalfunctionMeterByDurationSummaryDataOutputDto>> result = await Handle(input, cancellationToken);

            ICollection<MalfunctionMeterByDurationSummaryDataOutputDto> flatData = result
                .ReportData
                .SelectMany(f =>
                {
                    f.ReportHeader.IsFirstRow = true;
                    f.ReportData.Select(d => d.IsFirstRow = false);

                    return new[] { f.ReportHeader }.Concat(f.ReportData);
                }).ToList();

            ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationSummaryDataOutputDto> flatResult = new(result.Title, result.ReportHeader, flatData) { };
            return flatResult;
        }
        private static MalfunctionMeterByDurationSummaryDataOutputDto MapToGrouped(MalfunctionMeterByDurationSummaryByZoneDataOutputDto input)
        {
            return new MalfunctionMeterByDurationSummaryDataOutputDto()
            {
                ItemTitle = input.ItemTitle,
                CustomerCount = input.CustomerCount,
                CommercialUnit = input.CommercialUnit,
                DomesticUnit = input.DomesticUnit,
                OtherUnit = input.OtherUnit,
                TotalUnit = input.TotalUnit,
                Field0_5 = input.Field0_5,
                Field1_5 = input.Field1_5,
                Field0_75 = input.Field0_75,
                Field1 = input.Field1,
                Field2 = input.Field2,
                Field3 = input.Field3,
                Field4 = input.Field4,
                Field5 = input.Field5,
                Field1_2 = input.Field1_2,
                MoreThan6 = input.MoreThan6,
                UnSpecified = input.UnSpecified
            };
        }
    }
}
