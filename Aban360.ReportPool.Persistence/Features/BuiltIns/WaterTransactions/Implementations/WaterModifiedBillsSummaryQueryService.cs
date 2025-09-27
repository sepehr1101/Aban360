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
    internal sealed class WaterModifiedBillsSummaryQueryService : AbstractBaseConnection, IWaterModifiedBillsSummaryQueryService
    {
        public WaterModifiedBillsSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WaterModifiedBillsHeaderOutputDto, WaterModifiedBillsSummaryDataOutputDto>> GetInfo(WaterModifiedBillsInputDto input)
        {
            string modifiedBills = GetWaterModifiedBillsQuery();
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                typeCode = input.TypeIds,
                zoneIds=input.ZoneIds,
            };
            IEnumerable<WaterModifiedBillsSummaryDataOutputDto> modifiedBillsData = await _sqlReportConnection.QueryAsync<WaterModifiedBillsSummaryDataOutputDto>(modifiedBills, @params);
            WaterModifiedBillsHeaderOutputDto modifiedBillsHeader = new WaterModifiedBillsHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (modifiedBillsData is not null && modifiedBillsData.Any()) ? modifiedBillsData.Count() : 0,
                CustomerCount = modifiedBillsData.Sum(x => x.Count),
                Payable = modifiedBillsData.Sum(x => x.Payable),
                SumItems = modifiedBillsData.Sum(x => x.SumItems),
            };

            var result = new ReportOutput<WaterModifiedBillsHeaderOutputDto, WaterModifiedBillsSummaryDataOutputDto>(ReportLiterals.WaterModifiedBillsSummary, modifiedBillsHeader, modifiedBillsData);
            return result;
        }

        private string GetWaterModifiedBillsQuery()
        {
            return @"Select
                    	b.ZoneTitle,
                    	b.UsageTitle,
                    	b.TypeId AS TypeTitle,
                    	COUNT(1) AS Count,
                    	SUM(b.Payable) AS Payable,
                    	SUM(b.SumItems) AS SumItems
                    From [CustomerWarehouse].dbo.Bills b
                    Where	
                    	b.RegisterDay BETWEEN @fromDate AND @toDate AND
                    	b.TypeCode IN @typeCode AND
                        b.ZoneId IN @zoneIds
                    Group By	
                    	b.UsageTitle,
                    	b.TypeId,
                    	b.ZoneTitle,
                        b.ZoneId";
        }
    }
}
