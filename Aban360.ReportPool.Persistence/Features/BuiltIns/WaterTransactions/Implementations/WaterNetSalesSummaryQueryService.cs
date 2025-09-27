using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class WaterNetSalesSummaryQueryService : AbstractBaseConnection, IWaterNetSalesSummaryQueryService
    {
        public WaterNetSalesSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WaterSalesHeaderOutputDto, WaterNetSalesSummaryDataOutputDto>> GetInfo(WaterSalesInputDto input)
        {
            string waterNetSalesSummarys = GetWaterNetSalesSummaryQuery();
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds,
            };
            IEnumerable<WaterNetSalesSummaryDataOutputDto> waterNetSalesData = await _sqlReportConnection.QueryAsync<WaterNetSalesSummaryDataOutputDto>(waterNetSalesSummarys,@params);
            WaterSalesHeaderOutputDto waterNetSalesHeader = new WaterSalesHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (waterNetSalesData is not null && waterNetSalesData.Any() ? waterNetSalesData.Count() : 0),
                SumPayable = (waterNetSalesData is not null && waterNetSalesData.Any() ? waterNetSalesData.Sum(x => x.Payable) : 0),
                CustomerCount = (waterNetSalesData is not null && waterNetSalesData.Any() ? waterNetSalesData.Sum(x => x.Count) : 0)
            };

            var result = new ReportOutput<WaterSalesHeaderOutputDto, WaterNetSalesSummaryDataOutputDto>(ReportLiterals.WaterNetSalesSummary, waterNetSalesHeader, waterNetSalesData);
            return result;
        }

        private string GetWaterNetSalesSummaryQuery()
        {
            return @"Select 
                    	b.UsageTitle,
                    	b.ZoneTitle,
                    	SUM(b.Payable) AS Payable,
                    	COUNT(1) AS Count
                    From [CustomerWarehouse].dbo.Bills b
                    Where 
                    	b.TypeCode IN (3,4,5,9) AND
                    	b.RegisterDay BETWEEN @fromDate AND @toDate AND
                    	b.ZoneId IN @zoneIds 
                    Group By
                    	b.ZoneTitle,
                    	b.UsageTitle";
        }
    }
}
