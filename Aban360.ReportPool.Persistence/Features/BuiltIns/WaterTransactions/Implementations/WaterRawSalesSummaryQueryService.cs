using Aban360.Common.Db.Dapper;
using Aban360.Common.Excel;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class WaterRawSalesSummaryQueryService : AbstractBaseConnection, IWaterRawSalesSummaryQueryService
    {
        public WaterRawSalesSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WaterSalesHeaderOutputDto, WaterRawSalesSummaryDataOutputDto>> GetInfo(WaterSalesInputDto input)
        {
            string waterRawSalesSummarys = GetWaterRawSalesSummaryQuery();
            var @params = new
            {   
                fromDate=input.FromDateJalali,
                toDate=input.ToDateJalali,
                zoneIds=input.ZoneIds,
            };
            IEnumerable<WaterRawSalesSummaryDataOutputDto> waterRawSalesData = await _sqlReportConnection.QueryAsync<WaterRawSalesSummaryDataOutputDto>(waterRawSalesSummarys,@params);
            WaterSalesHeaderOutputDto waterRawSalesHeader = new WaterSalesHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount =( waterRawSalesData is not null && waterRawSalesData.Any() ? waterRawSalesData.Count() : 0),
                SumPayable = (waterRawSalesData is not null && waterRawSalesData.Any() ? waterRawSalesData.Sum(x=>x.Payable): 0)
            };

            var result = new ReportOutput<WaterSalesHeaderOutputDto, WaterRawSalesSummaryDataOutputDto>(ReportLiterals.WaterRawSalesSummary, waterRawSalesHeader, waterRawSalesData);
            return result;
        }

        private string GetWaterRawSalesSummaryQuery()
        {
            return @"Select 
                    	b.UsageTitle,
                    	b.ZoneId,
                        b.ZoneTitle,
                    	COUNT(1) AS Count,
                    	SUM(b.Payable) AS Payable
                    From [CustomerWarehouse].dbo.Bills b
                    Where 
                    	b.TypeCode=1 AND
                    	b.RegisterDay BETWEEN @fromDate AND @toDate AND
                    	b.ZoneId IN @zoneIds
                    Group By
                    	b.UsageTitle ,
                    	b.ZoneId,
                        b.ZoneTitle";
        }

     
    }
}
