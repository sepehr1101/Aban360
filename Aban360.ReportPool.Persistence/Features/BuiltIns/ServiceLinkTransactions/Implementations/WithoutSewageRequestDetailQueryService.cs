using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class WithoutSewageRequestDetailQueryService : WithoutSewageRequestBase, IWithoutSewageRequestDetailQueryService
    {
        public WithoutSewageRequestDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<WithoutSewageRequestHeaderOutputDto, WithoutSewageRequestDetailDataOutputDto>> Get(WithoutSewageRequestInputDto input)
        {
            string query = GetDetailQuery();

            IEnumerable<WithoutSewageRequestDetailDataOutputDto> withoutSewageRequestData = await _sqlReportConnection.QueryAsync<WithoutSewageRequestDetailDataOutputDto>(query, input);
            WithoutSewageRequestHeaderOutputDto withoutSewageRequestHeader = new WithoutSewageRequestHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (withoutSewageRequestData is not null && withoutSewageRequestData.Any()) ? withoutSewageRequestData.Count() : 0,
                Title= ReportLiterals.WithoutSewageRequestDetail,

                SumCommercialUnit = withoutSewageRequestData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = withoutSewageRequestData.Sum(i => i.DomesticUnit),
                SumOtherUnit = withoutSewageRequestData.Sum(i => i.OtherUnit),
                TotalUnit = withoutSewageRequestData.Sum(i => i.TotalUnit),
                CustomerCount = (withoutSewageRequestData is not null && withoutSewageRequestData.Any()) ? withoutSewageRequestData.Count() : 0,
            };
            var result = new ReportOutput<WithoutSewageRequestHeaderOutputDto, WithoutSewageRequestDetailDataOutputDto>
                (ReportLiterals.WithoutSewageRequestDetail, withoutSewageRequestHeader, withoutSewageRequestData);

            return result;
        }
    }
}