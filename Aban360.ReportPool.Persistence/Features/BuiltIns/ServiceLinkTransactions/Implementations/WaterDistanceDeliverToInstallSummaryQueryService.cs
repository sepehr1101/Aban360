using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class WaterDistanceDeliverToInstallSummaryQueryService : AbstractBaseConnection, IWaterDistanceDeliverToInstallSummaryQueryService
    {
        public WaterDistanceDeliverToInstallSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ReportOutput<SewageWaterDistanceHeaderOutputDto, SewageWaterDistanceSummaryDataOutputDto>> Get(WaterDistanceDeliverToInstallInputDto input, string groupedParam)
        {
            string groupedTitle = groupedParam == ReportLiterals.ZoneTitle ? ReportLiterals.ByZone : ReportLiterals.ByUsage;
            string reportTitle = ReportLiterals.WaterDistanceDeliverToInstallSummary + groupedTitle;
            string query = GetQuery(groupedParam);
            IEnumerable<SewageWaterDistanceSummaryDataOutputDto> RequestData = await _sqlReportConnection.QueryAsync<SewageWaterDistanceSummaryDataOutputDto>(query, input);
            SewageWaterDistanceHeaderOutputDto RequestHeader = new SewageWaterDistanceHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = RequestData.CountValue(),
                CustomerCount = RequestData.CountValue(),
                Title = reportTitle,

                SumCommercialUnit = RequestData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = RequestData.Sum(i => i.DomesticUnit),
                SumOtherUnit = RequestData.Sum(i => i.OtherUnit),
                TotalUnit = RequestData.Sum(i => i.TotalUnit)
            };
            var result = new ReportOutput<SewageWaterDistanceHeaderOutputDto, SewageWaterDistanceSummaryDataOutputDto>(reportTitle, RequestHeader, RequestData);
            return result;
        }
        private string GetQuery(string groupedParam)
        {
            return @$";WITH CTE AS
                    (
                        SELECT 
                            RN= ROW_NUMBER() OVER (PARTITION by ZoneId , CustomerNumber ORDER BY RegisterDayJalali DESC, LocalId DESC),
                            *
                        From [CustomerWarehouse].dbo.Clients c
                        Where			
                            c.PhysicalWaterInstallDateJalali BETWEEN @FromDateJalali AND @ToDateJalali AND	                    
                            --c.ZoneId IN @zoneIds AND
                            c.UsageId IN @usageIds AND
                            (
                                @fromReadingNumber IS NULL OR 
                                @toReadingNumber IS NULL OR
                                c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber
                            ) AND
                            c.CustomerNumber<>0 AND
                            c.RegisterDayJalali <= @ToDateJalali
                    )
                    Select 
                        MAX(t46.C2) AS RegionTitle,
                    	c.{groupedParam} AS ItemTitle,
                    	c.{groupedParam} ,
						Count(c.{groupedParam}) as CustomerCount,
						Count(c.{groupedParam}) as CustomerCount,
						SUM(c.DomesticCount) as DomesticUnit,
						SUM(c.CommercialCount) as CommercialUnit,
						SUM(c.OtherCount) as OtherUnit,
						SUM(IIF((c.DomesticCount+c.CommercialCount +c.OtherCount=0) ,1, (c.DomesticCount+c.CommercialCount +c.OtherCount))) AS TotalUnit,
						ROUND(AVG(CONVERT(float,ABS( DATEDIFF(DAY,
                            Case When LEN(r.InsertDateJalali)=10 Then [CustomerWarehouse].dbo.PersianToMiladi(r.InsertDateJalali) END,
                            Case When LEN(c.PhysicalWaterInstallDateJalali)=10 Then [CustomerWarehouse].dbo.PersianToMiladi(c.PhysicalWaterInstallDateJalali) END)))), 2) AS DistanceAverage
                   From [CustomerWarehouse].dbo.RequestFlows r
                    Join CTE c
                    	On r.BillId collate SQL_Latin1_General_CP1_CI_AS=c.BillId 
                    JOIN [Db70].dbo.T51 t51
                        On t51.C0=c.ZoneId
                    JOIN [Db70].dbo.T46 t46
                        On t51.C1=t46.C0
                    Where
                    	r.StatusId=97 AND
                    	c.RN=1
					Group By c.{groupedParam}";
        }
    }
}
