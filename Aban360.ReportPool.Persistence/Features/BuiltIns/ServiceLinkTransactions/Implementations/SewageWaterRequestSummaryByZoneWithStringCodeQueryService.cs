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
    internal sealed class SewageWaterRequestSummaryByZoneWithStringCodeQueryService : RequestOrInstallBase, ISewageWaterRequestSummaryByZoneWithStringCodeQueryService
    {
        public SewageWaterRequestSummaryByZoneWithStringCodeQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<SewageWaterRequestHeaderOutputDto, SewageWaterRequestSummaryWithStringCodeDataOutputDto>> Get(SewageWaterRequestWithStringCodeInputDto input)
        {
            string ZoneTitle = nameof(ZoneTitle);
            string query = GetQuery(input.IsWater, input.IsZone);
            string reportTitle = input.IsWater ? ReportLiterals.WaterRequestSummary + ReportLiterals.ByZone : ReportLiterals.SewageRequestSummary + ReportLiterals.ByZone;

            IEnumerable<SewageWaterRequestSummaryWithStringCodeDataOutputDto> data = await _sqlReportConnection.QueryAsync<SewageWaterRequestSummaryWithStringCodeDataOutputDto>(query, input);
            SewageWaterRequestHeaderOutputDto header = new SewageWaterRequestHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = data is not null && data.Any() ? data.Count() : 0,
                Title = reportTitle,

                SumCommercialUnit = data.Sum(i => i.CommercialUnit),
                SumDomesticUnit = data.Sum(i => i.DomesticUnit),
                SumOtherUnit = data.Sum(i => i.OtherUnit),
                TotalUnit = data.Sum(i => i.TotalUnit),
                CustomerCount = data.Sum(i => i.CustomerCount),
            };
            var result = new ReportOutput<SewageWaterRequestHeaderOutputDto, SewageWaterRequestSummaryWithStringCodeDataOutputDto>(reportTitle, header, data);
            return result;
        }
        private string GetQuery(bool isWater, bool isZone)
        {
            string dateConditionField = isWater ? " c.WaterRequestDate " : " c.SewageRequestDate ";
            string villageJionQuery = isZone ? string.Empty : " JOIN [Db70].dbo.Village v On t51.C0=v.ZoneId ";
            string villageField = isZone ? string.Empty : "MAX(v.VillageId) VillageId, IIF( TRIM(v.VillageName)='',N'نامخشص',TRIM(v.VillageName)) VillageTitle,";
            string groupingFields = isZone ? " t51.C0,t51.C2,t51.StringCode " : " t51.C0,t51.C2,v.VillageName,v.StringCode ";
            string stringCodeField = isZone ? " t51.StringCode " : "v.StringCode";

            return $@";WITH CTE AS
                    (
	                    SELECT 
		                    RN= ROW_NUMBER() OVER (PARTITION by ZoneId , CustomerNumber ORDER BY RegisterDayJalali DESC, LocalId DESC),
		                    *
                        From [CustomerWarehouse].dbo.Clients c
	                    Where				
		                    c.ZoneId IN @ZoneIds AND
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
                        t51.C0 ZoneId,
                        t51.C2 ZoneTitle,
                        {villageField}
						{stringCodeField} StringCode ,
                        COUNT(1) AS CustomerCount,
	                    SUM(IIF((c.DomesticCount+c.CommercialCount +c.OtherCount=0) ,1, (c.DomesticCount+c.CommercialCount +c.OtherCount))) AS TotalUnit,
	                    SUM(ISNULL(c.CommercialCount, 0)) AS CommercialUnit,
                        SUM(ISNULL(c.DomesticCount, 0)) AS DomesticUnit,
                        SUM(ISNULL(c.OtherCount, 0)) AS OtherUnit,
						SUM(c.ContractCapacity) ContractualCapacity,
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
                    FROM CTE c
                    JOIN [Db70].dbo.T51 t51
	                    On t51.C0=c.ZoneId
                    JOIN [Db70].dbo.T46 t46
	                    On t51.C1=t46.C0
                    {villageJionQuery}
                    WHERE	  
                        c.RN=1 AND
		                c.UsageId IN @UsageIds AND
	                    c.DeletionStateId NOT IN(1,2) AND
		                {dateConditionField} BETWEEN @FromDateJalali AND @ToDateJalali
                    GROUP BY
                        {groupingFields}";
        }
    }
}
