using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class SewageWaterInstallationSummaryByZoneIdQueryService : AbstractBaseConnection, ISewageWaterInstallationSummaryByZoneIdQueryService
    {
        public SewageWaterInstallationSummaryByZoneIdQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationSummaryByZoneIdDataOutputDto>> Get(SewageWaterInstallationInputDto input)
        {
            string query = GetQuery(input.IsWater);

            string reportTitle = input.IsWater ? ReportLiterals.WaterInstallationSummaryByZoneId : ReportLiterals.SewageInstallationSummaryByZoneId;
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds,
                usageIds=input.UsageIds,
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,
            };
            IEnumerable<SewageWaterInstallationSummaryByZoneIdDataOutputDto> installationData = await _sqlReportConnection.QueryAsync<SewageWaterInstallationSummaryByZoneIdDataOutputDto>(query, @params);
            SewageWaterInstallationHeaderOutputDto installationHeader = new SewageWaterInstallationHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = installationData is not null && installationData.Any() ? installationData.Count() : 0,
                Title = reportTitle,

                CustomerCount = installationData.Sum(i => i.CustomerCount),
                SumCommercialUnit = installationData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = installationData.Sum(i => i.DomesticUnit),
                SumOtherUnit = installationData.Sum(i => i.OtherUnit),
                TotalUnit = installationData.Sum(i => i.TotalUnit),
            };
            var result = new ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationSummaryByZoneIdDataOutputDto>
                (reportTitle,
                installationHeader,
                installationData);

            return result;
        }
        private string GetQuery(bool isWater)
        {
            string WaterRegisterDateJalali = nameof(WaterRegisterDateJalali),
                  SewageRegisterDateJalali = nameof(SewageRegisterDateJalali);
            string dateField = isWater ? WaterRegisterDateJalali : SewageRegisterDateJalali;
            return $@";WITH CTE AS
                    (
	                    SELECT 
		                    MaxRegisterDayJalali = MAX(RegisterDayJalali) OVER ( partition by ZoneId , CustomerNumber) ,
                            MaxId = MAX(LocalId) over( PARTITION by ZoneId , CustomerNumber) ,
		                    *
                        From [CustomerWarehouse].dbo.Clients c
	                    Where				
		                    c.{dateField} BETWEEN @fromDate AND @toDate AND
		                    c.ZoneId IN @zoneIds AND
		                    c.UsageId IN @usageIds AND
		                    (
			                    @fromReadingNumber IS NULL OR 
			                    @toReadingNumber IS NULL OR
			                    c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber
		                    ) AND
		                    c.CustomerNumber<>0 AND
		                    c.RegisterDayJalali <= @toDate
                    )
                    Select	
	                    MAX(t46.C2) AS RegionTitle,
                        c.ZoneTitle AS ZoneTitle,
                        COUNT(c.UsageTitle) AS CustomerCount,
	                    SUM(ISNULL(c.CommercialCount, 0) + ISNULL(c.DomesticCount, 0) + ISNULL(c.OtherCount, 0)) AS TotalUnit,
	                    SUM(ISNULL(c.CommercialCount, 0)) AS CommercialUnit,
                        SUM(ISNULL(c.DomesticCount, 0)) AS DomesticUnit,
                        SUM(ISNULL(c.OtherCount, 0)) AS OtherUnit,
                        SUM(CASE WHEN t5.C0 = 0 THEN 1 ELSE 0 END) AS UnSpecified,
	                    SUM(CASE WHEN t5.C0 = 1 THEN 1 ELSE 0 END) AS Field0_5,
	                    SUM(CASE WHEN t5.C0 = 2 THEN 1 ELSE 0 END) AS Field0_75,
	                    SUM(CASE WHEN t5.C0 = 3 THEN 1 ELSE 0 END) AS Field1,
	                    SUM(CASE WHEN t5.C0 = 4 THEN 1 ELSE 0 END) AS Field1_2,
	                    SUM(CASE WHEN t5.C0 = 5 THEN 1 ELSE 0 END) AS Field1_5,
	                    SUM(CASE WHEN t5.C0 = 6 THEN 1 ELSE 0 END) AS Field2,
	                    SUM(CASE WHEN t5.C0 = 7 THEN 1 ELSE 0 END) AS Field3,
	                    SUM(CASE WHEN t5.C0 = 8 THEN 1 ELSE 0 END) AS Field4,
	                    SUM(CASE WHEN t5.C0 = 9 THEN 1 ELSE 0 END) AS Field5,
	                    SUM(CASE WHEN t5.C0 In (10,11,12,13,15) THEN 1 ELSE 0 END) AS MoreThan6
                    FROM CTE c
                    JOIN [Db70].dbo.T5 t5
	                    On t5.C0=c.WaterDiameterId
                    JOIN [Db70].dbo.T51 t51
	                    On t51.C0=c.ZoneId
                    JOIN [Db70].dbo.T46 t46
	                    On t51.C1=t46.C0
                    WHERE	   
	                    c.DeletionStateId NOT IN(1,2) AND
	                    c.LocalId=MaxId AND
	                    c.RegisterDayJalali = MaxRegisterDayJalali
                    GROUP BY
                        c.ZoneId,c.ZoneTitle";
        }
    }
}
