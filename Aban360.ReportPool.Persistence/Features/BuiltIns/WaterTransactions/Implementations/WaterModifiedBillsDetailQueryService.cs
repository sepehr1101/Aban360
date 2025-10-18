using Aban360.Common.BaseEntities;
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
    internal sealed class WaterModifiedBillsDetailQueryService : AbstractBaseConnection, IWaterModifiedBillsDetailQueryService
    {
        public WaterModifiedBillsDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<WaterModifiedBillsHeaderOutputDto, WaterModifiedBillsDetailDataOutputDto>> GetInfo(WaterModifiedBillsInputDto input)
        {
            string modifiedBills = GetWaterModifiedBillsQuery();
            
            IEnumerable<WaterModifiedBillsDetailDataOutputDto> modifiedBillsData = await _sqlReportConnection.QueryAsync<WaterModifiedBillsDetailDataOutputDto>(modifiedBills, input);
            WaterModifiedBillsHeaderOutputDto modifiedBillsHeader = new WaterModifiedBillsHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (modifiedBillsData is not null && modifiedBillsData.Any()) ? modifiedBillsData.Count() : 0,
                CustomerCount = (modifiedBillsData is not null && modifiedBillsData.Any()) ? modifiedBillsData.Count() : 0,
                Payable = modifiedBillsData.Sum(x => x.Payable),
                SumItems = modifiedBillsData.Sum(x => x.SumItems),
                Title= ReportLiterals.WaterModifiedBillsDetail,
            };

            var result = new ReportOutput<WaterModifiedBillsHeaderOutputDto, WaterModifiedBillsDetailDataOutputDto>(ReportLiterals.WaterModifiedBillsDetail, modifiedBillsHeader, modifiedBillsData);
            return result;
        }

        private string GetWaterModifiedBillsQuery()
        {
            return @"Select
                    	b.ZoneTitle,
	                    b.CustomerNumber,
	                    b.UsageTitle AS UsageSellTitle,
	                    b.RegisterDay AS RegisterDateJalali,
	                    b.Payable,
	                    b.SumItems
                    From [CustomerWarehouse].dbo.Bills b
                    Where	
                    	b.RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali AND
                    	b.TypeCode IN @TypeIds AND
                        b.ZoneId IN @zoneIds";
        }
    }
}
