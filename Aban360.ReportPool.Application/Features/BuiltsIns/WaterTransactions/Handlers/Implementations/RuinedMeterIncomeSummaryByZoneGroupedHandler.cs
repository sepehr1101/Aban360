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
    internal sealed class RuinedMeterIncomeSummaryByZoneGroupedHandler : IRuinedMeterIncomeSummaryByZoneGroupedHandler
    {
        private readonly IRuinedMeterIncomeSummaryByZoneQueryService _ruinedMeterIncomeQueryService;
        private readonly IValidator<RuinedMeterIncomeInputDto> _validator;
        public RuinedMeterIncomeSummaryByZoneGroupedHandler(
            IRuinedMeterIncomeSummaryByZoneQueryService ruinedMeterIncomeQueryService,
            IValidator<RuinedMeterIncomeInputDto> validator)
        {
            _ruinedMeterIncomeQueryService = ruinedMeterIncomeQueryService;
            _ruinedMeterIncomeQueryService.NotNull(nameof(ruinedMeterIncomeQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<RuinedMeterIncomeHeaderOutputDto, ReportOutput<RuinedMeterIncomeSummaryDataOutputDto, RuinedMeterIncomeSummaryDataOutputDto>>> Handle(RuinedMeterIncomeInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<RuinedMeterIncomeHeaderOutputDto, RuinedMeterIncomeSummaryDataOutputDto> result = await _ruinedMeterIncomeQueryService.GetInfo(input);
            var dataGroup = result.ReportData
                .GroupBy(m => m.RegionTitle)
                .Select(g =>
                {
                    var mapped = g.Select(MapToGrouped);
                    return new ReportOutput<
                        RuinedMeterIncomeSummaryDataOutputDto,
                        RuinedMeterIncomeSummaryDataOutputDto>
                    (
                        result.Title,
                        ReportAggregator.AggregateGroup(mapped, g.Key),
                        mapped.Select(v => ReportAggregator.AggregateGroup(new[] { v }, v.ItemTitle))

                    );
                });

            ReportOutput<RuinedMeterIncomeHeaderOutputDto, ReportOutput<RuinedMeterIncomeSummaryDataOutputDto, RuinedMeterIncomeSummaryDataOutputDto>> finalData = new(result.Title, result.ReportHeader, dataGroup);


            return finalData;
        }
        private static RuinedMeterIncomeSummaryDataOutputDto MapToGrouped(RuinedMeterIncomeSummaryDataOutputDto input)
        {
            return new RuinedMeterIncomeSummaryDataOutputDto()
            {
                ItemTitle = input.ZoneTitle,
                Payable = input.Payable,
                SumItems = input.SumItems,
                CustomerCount = input.CustomerCount,
                CommercialUnit = input.CommercialUnit,
                DomesticUnit = input.DomesticUnit,
                TotalUnit = input.TotalUnit,
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
