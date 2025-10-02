using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
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
    internal sealed class WaterMeterReplacementsSummaryByChangeCauseQueryService : WaterMeterReplacementsBase, IWaterMeterReplacementsSummaryByChangeCauseQueryService
    {
        public WaterMeterReplacementsSummaryByChangeCauseQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<WaterMeterReplacementsHeaderOutputDto, WaterMeterReplacementsSummaryByChangeCauseDataOutputDto>> Get(WaterMeterReplacementsInputDto input)
        {
            string query = GetGroupedQuery(input.IsChangeDate, "mc.ChangeCauseTitle");
            //string query = GetBranchWaterMeterReplacementsQuery();
            
            string reportTitle = input.IsChangeDate == true ? ReportLiterals.WaterMeterReplacements(ReportLiterals.ChangeDate) + ReportLiterals.ByChangeCause : ReportLiterals.WaterMeterReplacements(ReportLiterals.RegisterDate) + ReportLiterals.ByChangeCause;
            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,

                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,

                zoneIds = input.ZoneIds,
                usageIds = input.UsageIds,
                isChangeDate = input.IsChangeDate ? 1 : 0,
            };
            IEnumerable<WaterMeterReplacementsSummaryByChangeCauseDataOutputDto> waterMeterReplacementsData = await _sqlReportConnection.QueryAsync<WaterMeterReplacementsSummaryByChangeCauseDataOutputDto>(query, @params);
            WaterMeterReplacementsHeaderOutputDto waterMeterReplacementsHeader = new WaterMeterReplacementsHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber=input.FromReadingNumber,
                ToReadingNumber=input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = waterMeterReplacementsData is not null && waterMeterReplacementsData.Any() ? waterMeterReplacementsData.Count() : 0,
                Title = reportTitle,

                CustomerCount = waterMeterReplacementsData.Sum(i => i.CustomerCount),
                SumCommercialUnit = waterMeterReplacementsData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = waterMeterReplacementsData.Sum(i => i.DomesticUnit),
                SumOtherUnit = waterMeterReplacementsData.Sum(i => i.OtherUnit),
                TotalUnit = waterMeterReplacementsData.Sum(i => i.TotalUnit),
            };
            var result = new ReportOutput<WaterMeterReplacementsHeaderOutputDto, WaterMeterReplacementsSummaryByChangeCauseDataOutputDto>(
                   reportTitle,
                   waterMeterReplacementsHeader,
                   waterMeterReplacementsData);
            return result;
        }
        private string GetBranchWaterMeterReplacementsQuery()
        {
            return @"Select 
                        mc.ChangeCauseTitle AS ChangeCauseTitle,
                        COUNT(mc.ChangeCauseTitle) AS CustomerCount,
                        SUM(ISNULL(c.CommercialCount, 0) + ISNULL(c.DomesticCount, 0) + ISNULL(c.OtherCount, 0)) AS TotalUnit,
                        SUM(ISNULL(c.CommercialCount, 0)) AS CommercialUnit,
                        SUM(ISNULL(c.DomesticCount, 0)) AS DomesticUnit,
                        SUM(ISNULL(c.OtherCount, 0)) AS OtherUnit,
                        SUM(CASE WHEN c.WaterDiameterId = 0 THEN 1 ELSE 0 END) AS UnSpecified,
                        SUM(CASE WHEN c.WaterDiameterId = 1 THEN 1 ELSE 0 END) AS Field0_5,
                        SUM(CASE WHEN c.WaterDiameterId = 2 THEN 1 ELSE 0 END) AS Field0_75,
                        SUM(CASE WHEN c.WaterDiameterId = 3 THEN 1 ELSE 0 END) AS Field1,
                        SUM(CASE WHEN c.WaterDiameterId = 4 THEN 1 ELSE 0 END) AS Field1_2,
                        SUM(CASE WHEN c.WaterDiameterId = 5 THEN 1 ELSE 0 END) AS Field1_5,
                        SUM(CASE WHEN c.WaterDiameterId = 6 THEN 1 ELSE 0 END) AS Field2,
                        SUM(CASE WHEN c.WaterDiameterId = 7 THEN 1 ELSE 0 END) AS Field3,
                        SUM(CASE WHEN c.WaterDiameterId = 8 THEN 1 ELSE 0 END) AS Field4,
                        SUM(CASE WHEN c.WaterDiameterId = 9 THEN 1 ELSE 0 END) AS Field5,
                        SUM(CASE WHEN c.WaterDiameterId In (10,11,12,13,15) THEN 1 ELSE 0 END) AS MoreThan6
                    From [CustomerWarehouse].dbo.MeterChange mc
                    Join [CustomerWarehouse].dbo.Clients c 
						on mc.CustomerNumber=c.CustomerNumber AND mc.ZoneId=c.ZoneId
                    Where 
                    	(@isChangeDate=0 AND
                    	mc.RegisterDateJalali BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds AND
                    	c.UsageId IN @UsageIds AND
						(@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
						c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
						c.ToDayJalali IS NULL 
						)
                    	OR
                    	(@isChangeDate=1 AND
                    	mc.ChangeDateJalali BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds AND
                    	c.UsageId IN @UsageIds AND
						(@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
						c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
						c.ToDayJalali IS NULL
						)
                    Group By mc.ChangeCauseTitle";
        }
    }
}
