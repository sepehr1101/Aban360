using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class NonPermanentBranchSummaryByZoneGroupedHandler : INonPermanentBranchSummaryByZoneGroupedHandler
    {
        private readonly INonPermanentBranchSummaryByZoneQueryService _nonPermanentBranchSummaryByZoneQueryService;
        private readonly IValidator<NonPermanentBranchByUsageAndZoneInputDto> _validator;
        public NonPermanentBranchSummaryByZoneGroupedHandler(
            INonPermanentBranchSummaryByZoneQueryService nonPremanentBranchSummaryByZoneQueryService,
            IValidator<NonPermanentBranchByUsageAndZoneInputDto> validator)
        {
            _nonPermanentBranchSummaryByZoneQueryService = nonPremanentBranchSummaryByZoneQueryService;
            _nonPermanentBranchSummaryByZoneQueryService.NotNull(nameof(nonPremanentBranchSummaryByZoneQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<NonPermanentBranchHeaderOutputDto, ReportOutput<NonPermanentBranchSummaryByZoneGropedDataOutputDto, NonPermanentBranchSummaryByZoneGropedDataOutputDto>>> Handle(NonPermanentBranchByUsageAndZoneInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchSummaryByZoneDataOutputDto> result = await _nonPermanentBranchSummaryByZoneQueryService.GetInfo(input);

            IEnumerable<ReportOutput<NonPermanentBranchSummaryByZoneGropedDataOutputDto, NonPermanentBranchSummaryByZoneGropedDataOutputDto>> dataGroup = result
               .ReportData
               .GroupBy(m => m.RegionTitle) // فقط بر اساس RegionId گروه‌بندی
               .Select(g => new ReportOutput<NonPermanentBranchSummaryByZoneGropedDataOutputDto, NonPermanentBranchSummaryByZoneGropedDataOutputDto>
               (
                   result.Title,
                   new NonPermanentBranchSummaryByZoneGropedDataOutputDto
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
                   g.Select(v => new NonPermanentBranchSummaryByZoneGropedDataOutputDto
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
               .ToList();


            ReportOutput<NonPermanentBranchHeaderOutputDto, ReportOutput<NonPermanentBranchSummaryByZoneGropedDataOutputDto, NonPermanentBranchSummaryByZoneGropedDataOutputDto>> finalData = new(result.Title, result.ReportHeader, dataGroup);

            return finalData;
        }
    }
}
