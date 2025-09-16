using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Base;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class HouseholdNumberSummarybyZoneGroupedHandler : IHouseholdNumberSummarybyZoneGroupedHandler
    {
        private readonly IHouseholdNumberSummaryByZoneQueryService _householdNumberSummarybyZoneQueryService;
        private readonly IValidator<HouseholdNumberInputDto> _validator;
        public HouseholdNumberSummarybyZoneGroupedHandler(
            IHouseholdNumberSummaryByZoneQueryService householdNumberSummarybyZoneQueryService,
            IValidator<HouseholdNumberInputDto> validator)
        {
            _householdNumberSummarybyZoneQueryService = householdNumberSummarybyZoneQueryService;
            _householdNumberSummarybyZoneQueryService.NotNull(nameof(householdNumberSummarybyZoneQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<HouseholdNumberHeaderOutputDto, ReportOutput<HouseholdNumberSummaryDataOutputDto, HouseholdNumberSummaryDataOutputDto>>> Handle(HouseholdNumberInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            input.ToHouseholdDateJalali = ReduceYear(input.ToHouseholdDateJalali);
            string lastDateJalai = ReduceYear(DateTime.Now.ToShortPersianDateString());
            ReportOutput<HouseholdNumberHeaderOutputDto, HouseholdNumberSummaryByZoneDataOutputDto> result = await _householdNumberSummarybyZoneQueryService.GetInfo(input, lastDateJalai);

            var dataGroup = result.ReportData
                .GroupBy(m => m.RegionTitle)
                .Select(g =>
                {
                    var mapped = g.Select(MapToGroupe);

                    var data = new ReportOutput
                    <HouseholdNumberSummaryDataOutputDto,
                    HouseholdNumberSummaryDataOutputDto>
                    (
                        result.Title,
                        ReportAggregator.AggregateGroup(mapped, g.Key),
                        mapped.Select(v => ReportAggregator.AggregateGroup(new[] { v }, v.ItemTitle))
                    );
                    data.ReportHeader.HouseholdAverage = data.ReportData.Max(r => r.HouseholdAverage);

                    return data;
                });
            ReportOutput<HouseholdNumberHeaderOutputDto, ReportOutput<HouseholdNumberSummaryDataOutputDto, HouseholdNumberSummaryDataOutputDto>> finalData = new(result.Title, result.ReportHeader, dataGroup);

            return finalData;
        }
        private string ReduceYear(string jalaliDate)
        {
            DateOnly? lastDateJalali = jalaliDate.ToGregorianDateOnly();
            if (!lastDateJalali.HasValue)
            {
                throw new BaseException(ExceptionLiterals.InvalidDate);
            }
            return lastDateJalali
                              .Value
                              .AddYears(-1)
                              .ToShortPersianDateString();
        }
        private static HouseholdNumberSummaryDataOutputDto MapToGroupe(HouseholdNumberSummaryByZoneDataOutputDto input)
        {
            return new HouseholdNumberSummaryDataOutputDto()
            {
                ItemTitle = input.ZoneTitle,
                CustomerCount = input.CustomerCount,
                TotalUnit = input.TotalUnit,
                CommercialUnit = input.CommercialUnit,
                DomesticUnit = input.DomesticUnit,
                OtherUnit = input.OtherUnit,
                HouseholdAverage = input.HouseholdAverage,
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
                MoreThan6 = input.MoreThan6,
            };
        }
    }
}
