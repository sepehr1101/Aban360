using Aban360.Common.Excel;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class WaterUsageGroupedQueryService : AbstractBaseConnection, IWaterUsageGroupedQueryService
    {
        public WaterUsageGroupedQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WaterUsageGroupedHeaderOutputDto, WaterUsageGroupedDataOutputDto>> GetInfo(WaterUsageGroupedInputDto input)
        {
            string waterUsageGroupeds = GetWaterUsageGroupedQuery();
            var @params = new
            {
                FromDate = input.FromDateJalali,
                ToDate = input.ToDateJalali,
            };
            IEnumerable<WaterUsageGroupedDataOutputDto> waterUsageGroupedData = await _sqlReportConnection.QueryAsync<WaterUsageGroupedDataOutputDto>(waterUsageGroupeds, @params);
            WaterUsageGroupedHeaderOutputDto waterUsageGroupedHeader = new WaterUsageGroupedHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RecordCount = (waterUsageGroupedData is not null && waterUsageGroupedData.Any()) ? waterUsageGroupedData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                TotalAmount = waterUsageGroupedData.Sum(usage => usage.Amount)
            };

            var result = new ReportOutput<WaterUsageGroupedHeaderOutputDto, WaterUsageGroupedDataOutputDto>(ReportLiterals.WaterUsageGrouped, waterUsageGroupedHeader, waterUsageGroupedData);
            return result;
        }

        private string GetWaterUsageGroupedQuery()
        {
            return @"Select 
                    	SUM(p.Amount) AS Amount,
                    	c.UsageTitle AS UsageTitle
                    From [CustomerWarehouse].dbo.Payments p
                    JOIN [CustomerWarehouse].dbo.Clients c 
                    	ON p.BillId=c.BillId
                    WHERE
                        c.ToDayJalali IS NULL AND
                        (@FromDate IS NULL OR 
                        @ToDate IS NULL OR
                    	p.RegisterDay BETWEEN @FromDate and @ToDate)
                    GROUP BY c.UsageTitle";
            //todo
        }
    }
}
