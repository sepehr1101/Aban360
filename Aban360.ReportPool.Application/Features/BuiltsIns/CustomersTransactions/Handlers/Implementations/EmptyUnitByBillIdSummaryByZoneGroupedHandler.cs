using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using FluentValidation;
using System.Runtime.InteropServices;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class EmptyUnitByBillIdSummaryByZoneGroupedHandler : IEmptyUnitByBillIdSummaryByZoneGroupedHandler
    {
        private readonly IEmptyUnitByBillIdZoneGroupingQueryService _emptyUnitByBillIdZoneGroupingQueryService;
        private readonly IValidator<EmptyUnitInputDto> _validator;
        public EmptyUnitByBillIdSummaryByZoneGroupedHandler(
            IEmptyUnitByBillIdZoneGroupingQueryService emptyUnitByBillIdZoneGroupingQueryService,
            IValidator<EmptyUnitInputDto> validator)
        {
            _emptyUnitByBillIdZoneGroupingQueryService = emptyUnitByBillIdZoneGroupingQueryService;
            _emptyUnitByBillIdZoneGroupingQueryService.NotNull(nameof(emptyUnitByBillIdZoneGroupingQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<EmptyUnitByBillIdSummaryHeaderOutputDto, ReportOutput<EmptyUnitByBillIdByZoneGroupedDataOutputDto, EmptyUnitByBillIdByZoneGroupedDataOutputDto>>> Handle(EmptyUnitInputDto input, [Optional] CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<EmptyUnitByBillIdSummaryHeaderOutputDto, EmptyUnitByBillIdSummaryDataOutputDto> result = await _emptyUnitByBillIdZoneGroupingQueryService.Get(input);

            IEnumerable<ReportOutput<EmptyUnitByBillIdByZoneGroupedDataOutputDto, EmptyUnitByBillIdByZoneGroupedDataOutputDto>> dataGroup = result
              .ReportData
              .GroupBy(m => m.RegionTitle) // فقط بر اساس RegionId گروه‌بندی
              .Select(g => new ReportOutput<EmptyUnitByBillIdByZoneGroupedDataOutputDto, EmptyUnitByBillIdByZoneGroupedDataOutputDto>
              (
                  result.Title,
                  new EmptyUnitByBillIdByZoneGroupedDataOutputDto
                  {
                      ItemTitle = g.First().RegionTitle,
                      CustomerCount = g.Sum(x => x.CustomerCount),
                      TotalUnit = g.Sum(x => x.TotalUnit),
                      CommercialUnit = g.Sum(x => x.CommercialUnit),
                      DomesticUnit = g.Sum(x => x.DomesticUnit),
                      OtherUnit = g.Sum(x => x.OtherUnit),
                      EmptyUnit = g.Sum(x => x.EmptyUnit),
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
                  g.Select(v => new EmptyUnitByBillIdByZoneGroupedDataOutputDto
                  {
                      ItemTitle = v.ZoneTitle,
                      CustomerCount = v.CustomerCount,
                      TotalUnit = v.TotalUnit,
                      CommercialUnit = v.CommercialUnit,
                      DomesticUnit = v.DomesticUnit,
                      OtherUnit = v.OtherUnit,
                      EmptyUnit = v.EmptyUnit,
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


            ReportOutput<EmptyUnitByBillIdSummaryHeaderOutputDto, ReportOutput<EmptyUnitByBillIdByZoneGroupedDataOutputDto, EmptyUnitByBillIdByZoneGroupedDataOutputDto>> finalData = new(result.Title, result.ReportHeader, dataGroup);
            return finalData;
        }

        public async Task<ReportOutput<EmptyUnitByBillIdSummaryHeaderOutputDto, EmptyUnitByBillIdByZoneGroupedDataOutputDto>> HandleFlat(EmptyUnitInputDto input, [Optional] CancellationToken cancellationToken)
        {
            ReportOutput<EmptyUnitByBillIdSummaryHeaderOutputDto, ReportOutput<EmptyUnitByBillIdByZoneGroupedDataOutputDto, EmptyUnitByBillIdByZoneGroupedDataOutputDto>> result = await Handle(input, cancellationToken);

            ICollection<EmptyUnitByBillIdByZoneGroupedDataOutputDto> flatData = result
                .ReportData
                .SelectMany(f =>
                {
                    f.ReportHeader.IsFirstRow = true;
                    f.ReportData.Select(d => d.IsFirstRow = false);

                    return new[] { f.ReportHeader }.Concat(f.ReportData);
                }).ToList();

            ReportOutput<EmptyUnitByBillIdSummaryHeaderOutputDto, EmptyUnitByBillIdByZoneGroupedDataOutputDto> flatResult = new(result.Title, result.ReportHeader, flatData) { };
            return flatResult;
        }
    }
}
