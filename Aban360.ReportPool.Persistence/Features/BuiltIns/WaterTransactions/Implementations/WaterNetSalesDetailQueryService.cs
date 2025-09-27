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

    internal sealed class WaterNetSalesDetailQueryService : AbstractBaseConnection, IWaterNetSalesDetailQueryService
    {
        public WaterNetSalesDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WaterSalesHeaderOutputDto, WaterNetSalesDetailDataOutputDto>> GetInfo(WaterSalesInputDto input)
        {
            string waterNetSalesDetails = GetWaterNetSalesDetailQuery();
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds,
            };
            IEnumerable<WaterNetSalesDetailDataOutputDto> waterNetSalesData = await _sqlReportConnection.QueryAsync<WaterNetSalesDetailDataOutputDto>(waterNetSalesDetails,@params);
            WaterSalesHeaderOutputDto waterNetSalesHeader = new WaterSalesHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                CustomerCount = (waterNetSalesData is not null && waterNetSalesData.Any() ? waterNetSalesData.Count() : 0),
                RecordCount = (waterNetSalesData is not null && waterNetSalesData.Any() ? waterNetSalesData.Count() : 0),
                SumPayable = (waterNetSalesData is not null && waterNetSalesData.Any() ? waterNetSalesData.Sum(x => x.Payable) : 0)
            };

            var result = new ReportOutput<WaterSalesHeaderOutputDto, WaterNetSalesDetailDataOutputDto>(ReportLiterals.WaterNetSalesDetail, waterNetSalesHeader, waterNetSalesData);
            return result;
        }

        private string GetWaterNetSalesDetailQuery()
        {
            return @"Select 
                    	b.UsageTitle,
                    	b.ZoneTitle,
                    	b.BillId,
                    	b.Payable,
                    	b.CustomerNumber,
                    	b.ReadingNumber
                    From [CustomerWarehouse].dbo.Bills b
                    Where 
                    	b.TypeCode IN (3,4,5,9) AND
                    	b.RegisterDay BETWEEN @fromDate AND @toDate AND
                    	b.ZoneId IN @zoneIds ";
        }
    }
}
