using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class WithoutBillQueryService : WithoutBillBase, IWithoutBillQueryService
    {
        public WithoutBillQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<WithoutBillHeaderOutputDto, WithoutBillDataOutputDto>> GetInfo(WithoutBillInputDto input)
        {
            string query = GetDetailQuery(input.ZoneIds.HasValue(), input.UsageIds.HasValue());

            IEnumerable<WithoutBillDataOutputDto> withoutBillData = await _sqlReportConnection.QueryAsync<WithoutBillDataOutputDto>(query, input, null, 180);
            WithoutBillHeaderOutputDto withoutBillHeader = new WithoutBillHeaderOutputDto()
            {
                FromDateJalali=input.FromDateJalali,
                ToDateJalali=input.ToDateJalali,
                FromReadingNumber=input.FromReadingNumber,
                ToReadingNumber=input.ToReadingNumber,
                RecordCount= (withoutBillData is not null && withoutBillData.Any()) ? withoutBillData.Count() : 0,
                CustomerCount = (withoutBillData is not null && withoutBillData.Any()) ? withoutBillData.Count() : 0,
                ReportDateJalali =DateTime.Now.ToShortPersianDateString(),
                Title= ReportLiterals.WithoutBill,

                SumCommercialUnit =withoutBillData?.Sum(s=>s.CommercialUnit)??0,
                SumDomesticUnit=withoutBillData?.Sum(s=>s.DomesticUnit)??0,
                SumOtherUnit=withoutBillData?.Sum(s=>s.OtherUnit)??0,
                TotalUnit=withoutBillData?.Sum(s=>s.TotalUnit)??0,
            };

            var result = new ReportOutput<WithoutBillHeaderOutputDto, WithoutBillDataOutputDto>(ReportLiterals.WithoutBill, withoutBillHeader, withoutBillData);
            return result;
        }
    }
}