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
    internal sealed class SewageWaterRequestNonInstalledSummaryByZoneGroupedHandler : ISewageWaterRequestNonInstalledSummaryByZoneGroupedHandler
    {
        private readonly ISewageWaterRequestNonInstalledSummaryByZoneQueryService _sewageWaterRequestNonInstalledSummaryByZoneQuery;
        private readonly IValidator<SewageWaterRequestNonInstalledInputDto> _validator;
        public SewageWaterRequestNonInstalledSummaryByZoneGroupedHandler(
            ISewageWaterRequestNonInstalledSummaryByZoneQueryService sewageWaterRequestNonInstalledSummaryByZoneQuery,
            IValidator<SewageWaterRequestNonInstalledInputDto> validator)
        {
            _sewageWaterRequestNonInstalledSummaryByZoneQuery = sewageWaterRequestNonInstalledSummaryByZoneQuery;
            _sewageWaterRequestNonInstalledSummaryByZoneQuery.NotNull(nameof(sewageWaterRequestNonInstalledSummaryByZoneQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<SewageWaterRequestNonInstalledHeaderOutputDto, ReportOutput<SewageWaterRequestNonInstalledSummaryDataOutputDto, SewageWaterRequestNonInstalledSummaryDataOutputDto>>> Handle(SewageWaterRequestNonInstalledInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<SewageWaterRequestNonInstalledHeaderOutputDto, SewageWaterRequestNonInstalledSummaryByZoneDataOutputDto> result = await _sewageWaterRequestNonInstalledSummaryByZoneQuery.Get(input);
            var dataGroup = result.ReportData
                .GroupBy(m => m.RegionTitle)
                .Select(g =>
                {
                    var mapped = g.Select(MapToGroupe);

                    return new ReportOutput
                    <SewageWaterRequestNonInstalledSummaryDataOutputDto,
                    SewageWaterRequestNonInstalledSummaryDataOutputDto>
                    (
                        result.Title,
                        ReportAggregator.AggregateGroup(mapped, g.Key),
                        mapped.Select(v => ReportAggregator.AggregateGroup(new[] { v }, v.ItemTitle)
                    ));
                });

            ReportOutput<SewageWaterRequestNonInstalledHeaderOutputDto, ReportOutput<SewageWaterRequestNonInstalledSummaryDataOutputDto, SewageWaterRequestNonInstalledSummaryDataOutputDto>> finalData = new(result.Title, result.ReportHeader, dataGroup);

            return finalData;
        }
        private static SewageWaterRequestNonInstalledSummaryDataOutputDto MapToGroupe(SewageWaterRequestNonInstalledSummaryByZoneDataOutputDto input)
        {
            return new SewageWaterRequestNonInstalledSummaryDataOutputDto()
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
                MoreThan6 = input.MoreThan6,
            };
        }
    }
}
