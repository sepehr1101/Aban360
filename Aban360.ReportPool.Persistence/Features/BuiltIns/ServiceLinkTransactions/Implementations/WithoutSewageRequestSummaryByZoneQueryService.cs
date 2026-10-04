using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Constants;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class WithoutSewageRequestSummaryByZoneQueryService : WithoutSewageRequestBase, IWithoutSewageRequestSummaryByZoneQueryService
    {
        public WithoutSewageRequestSummaryByZoneQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<WithoutSewageRequestSummaryHeaderOutputDto, WithoutSewageRequestSummaryDataOutputDto>> Get(WithoutSewageRequestInputDto input)
        {
            string query = GetGroupedQuery(GroupingFields.ZoneTitle);

            IEnumerable<WithoutSewageRequestSummaryDataOutputDto> withoutSewageRequestData = await _sqlReportConnection.QueryAsync<WithoutSewageRequestSummaryDataOutputDto>(query, input);
            WithoutSewageRequestSummaryHeaderOutputDto withoutSewageRequestHeader = new WithoutSewageRequestSummaryHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = withoutSewageRequestData is not null && withoutSewageRequestData.Any() ? withoutSewageRequestData.Count() : 0,
                Title = ReportLiterals.WithoutSewageRequestSummaryByZone,

                SumCommercialUnit = withoutSewageRequestData?.Sum(i => i.CommercialUnit) ?? 0,
                SumDomesticUnit = withoutSewageRequestData?.Sum(i => i.DomesticUnit) ?? 0,
                SumOtherUnit = withoutSewageRequestData?.Sum(i => i.OtherUnit) ?? 0,
                TotalUnit = withoutSewageRequestData?.Sum(i => i.TotalUnit) ?? 0,
                CustomerCount = withoutSewageRequestData?.Sum(i => i.CustomerCount) ?? 0,

                UnSpecified = withoutSewageRequestData?.Sum(i => i.UnSpecified) ?? 0,
                Field0_5 = withoutSewageRequestData?.Sum(i => i.Field0_5) ?? 0,
                Field0_75 = withoutSewageRequestData?.Sum(i => i.Field0_75) ?? 0,
                Field1 = withoutSewageRequestData?.Sum(i => i.Field1) ?? 0,
                Field1_2 = withoutSewageRequestData?.Sum(i => i.Field1_2) ?? 0,
                Field1_5 = withoutSewageRequestData?.Sum(i => i.Field1_5) ?? 0,
                Field2 = withoutSewageRequestData?.Sum(i => i.Field2) ?? 0,
                Field3 = withoutSewageRequestData?.Sum(i => i.Field3) ?? 0,
                Field4 = withoutSewageRequestData?.Sum(i => i.Field4) ?? 0,
                Field5 = withoutSewageRequestData?.Sum(i => i.Field5) ?? 0,
                MoreThan6 = withoutSewageRequestData?.Sum(i => i.MoreThan6) ?? 0,
            };
            var result = new ReportOutput<WithoutSewageRequestSummaryHeaderOutputDto, WithoutSewageRequestSummaryDataOutputDto>
                (ReportLiterals.WithoutSewageRequestSummaryByZone, withoutSewageRequestHeader, withoutSewageRequestData);

            return result;
        }
    }
}
