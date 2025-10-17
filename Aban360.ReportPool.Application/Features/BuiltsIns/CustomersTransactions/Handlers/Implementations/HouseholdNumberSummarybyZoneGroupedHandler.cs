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
using System.Runtime.InteropServices;

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
                throw new CustomValidationException(message);
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

                    return data;
                });
            ReportOutput<HouseholdNumberHeaderOutputDto, ReportOutput<HouseholdNumberSummaryDataOutputDto, HouseholdNumberSummaryDataOutputDto>> finalData = new(result.Title, result.ReportHeader, dataGroup);

            return finalData;
        }
        public async Task<ReportOutput<HouseholdNumberHeaderOutputDto, HouseholdNumberSummaryDataOutputDto>> HandleFlat(HouseholdNumberInputDto input, [Optional] CancellationToken cancellationToken)
        {
            ReportOutput<HouseholdNumberHeaderOutputDto, ReportOutput<HouseholdNumberSummaryDataOutputDto, HouseholdNumberSummaryDataOutputDto>> result = await Handle(input, cancellationToken);

            ICollection<HouseholdNumberSummaryDataOutputDto> flatData = result
                .ReportData
                .SelectMany(f =>
                {
                    f.ReportHeader.IsFirstRow = true;
                    f.ReportData.Select(d => d.IsFirstRow = false);

                    return new[] { f.ReportHeader }.Concat(f.ReportData);
                }).ToList();

            ReportOutput<HouseholdNumberHeaderOutputDto, HouseholdNumberSummaryDataOutputDto> flatResult = new(result.Title, result.ReportHeader, flatData) { };
            return flatResult;
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
                SumHousehold = input.SumHousehold,
                Field1 = input.Field1,
                Field2 = input.Field2,
                Field3 = input.Field3,
                Field4 = input.Field4,
                FieldMore5=input.FieldMore5,
            };
        }
    }
}
