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
    internal sealed class SewageWaterInstallationDetailQueryService : AbstractBaseConnection, ISewageWaterInstallationDetailQueryService
    {
        public SewageWaterInstallationDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationDetailDataOutputDto>> Get(SewageWaterInstallationInputDto input)
        {//Todo: second Boolean Field
            string reportTitle = input.IsWater ? ReportLiterals.WaterInstallationDetail : ReportLiterals.SewageInstallationDetail;
            string query = GetQuery(input.IsWater);

            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,
                zoneIds = input.ZoneIds,
                usageIds=input.UsageIds,
            };
            IEnumerable<SewageWaterInstallationDetailDataOutputDto> installationData = await _sqlReportConnection.QueryAsync<SewageWaterInstallationDetailDataOutputDto>(query, @params);
            SewageWaterInstallationHeaderOutputDto installationHeader = new SewageWaterInstallationHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                CustomerCount = (installationData is not null && installationData.Any()) ? installationData.Count() : 0,
                RecordCount = (installationData is not null && installationData.Any()) ? installationData.Count() : 0,
                Title = reportTitle,

                SumCommercialUnit = installationData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = installationData.Sum(i => i.DomesticUnit),
                SumOtherUnit = installationData.Sum(i => i.OtherUnit),
                TotalUnit = installationData.Sum(i => i.TotalUnit)
            };

            ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationDetailDataOutputDto> result = new(reportTitle, installationHeader, installationData);

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
	                    c.CustomerNumber, 
                    	c.ReadingNumber,
                    	TRIM(c.FirstName) AS FirstName,
                    	TRIM(c.SureName) AS Surname,
                    	TRIM(c.Address) AS Address,
                    	c.UsageTitle AS UsageTitle,
                    	c.WaterDiameterTitle AS MeterDiameterTitle,
                        c.MainSiphonTitle AS SiphonDiameterTitle,
                    	c.ZoneTitle,
                    	c.ZoneId,
                    	c.DomesticCount	AS DomesticUnit,
                    	c.CommercialCount AS CommercialUnit,
                    	c.OtherCount AS OtherUnit,
                        (c.DomesticCount+c.CommercialCount +c.OtherCount) AS TotalUnit ,
                    	c.BillId,
                    	c.BranchType AS UseStateTitle,
                    	c.ContractCapacity AS ContractualCapacity,
                    	c.WaterRequestDate AS RequestDate,
                    	c.WaterRegisterDateJalali AS InstallationDate
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
	                    c.RegisterDayJalali = MaxRegisterDayJalali";
        }
    }
}