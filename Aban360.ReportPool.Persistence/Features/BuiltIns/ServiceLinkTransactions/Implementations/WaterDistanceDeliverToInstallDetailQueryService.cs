using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
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
    internal sealed class WaterDistanceDeliverToInstallDetailQueryService : AbstractBaseConnection, IWaterDistanceDeliverToInstallDetailQueryService
    {
        public WaterDistanceDeliverToInstallDetailQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ReportOutput<SewageWaterDistanceHeaderOutputDto, SewageWaterDistanceDetailDataOutputDto>> Get(WaterDistanceDeliverToInstallInputDto input)
        {
            string reportTitle = ReportLiterals.WaterDistanceDeliverToInstallDetail;
            string query = GetQuery();
            IEnumerable<SewageWaterDistanceDetailDataOutputDto> RequestData = await _sqlReportConnection.QueryAsync<SewageWaterDistanceDetailDataOutputDto>(query, input);
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
            var result = new ReportOutput<SewageWaterDistanceHeaderOutputDto, SewageWaterDistanceDetailDataOutputDto>(reportTitle, RequestHeader, RequestData);
            return result;
        }
        private string GetQuery()
        {
            return @";WITH CTE AS
                    (
                        SELECT 
                            RN= ROW_NUMBER() OVER (PARTITION by ZoneId , CustomerNumber ORDER BY RegisterDayJalali DESC, LocalId DESC),
                            *
                        From [CustomerWarehouse].dbo.Clients c
                        Where			
                            c.PhysicalWaterInstallDateJalali BETWEEN @FromDateJalali AND @ToDateJalali AND	                    
                            c.ZoneId IN @zoneIds AND
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
                        t46.C2 AS RegionTitle,
                    	c.ZoneId,
                    	c.ZoneTitle,
                    	c.CustomerNumber,
                    	c.ReadingNumber,
                    	c.BillId,
                    	c.PhysicalWaterInstallDateJalali,
                    	TRIM(c.FirstName) AS FirstName,
                    	TRIM(c.SureName) AS Surname,
                    	TRIM(c.Address) AS Address,
                    	c.UsageTitle2 AS UsageTitle,
                    	c.WaterDiameterTitle AS MeterDiameterTitle,
                    	c.MainSiphonTitle AS SiphonDiameterTitle,
                    	c.ZoneTitle,
                    	c.ZoneId,
                    	c.DomesticCount	AS DomesticUnit,
                    	c.CommercialCount AS CommercialUnit,
                    	c.OtherCount AS OtherUnit,
                    	IIF((c.DomesticCount+c.CommercialCount +c.OtherCount=0) ,1, (c.DomesticCount+c.CommercialCount +c.OtherCount)) AS TotalUnit,
                    	c.BillId,
                    	c.BranchType AS UseStateTitle,
                    	c.ContractCapacity AS ContractualCapacity,
                    	r.InsertDateJalali as RequestDate,
                    	c.PhysicalWaterInstallDateJalali as  InstallationDate,
						c.WaterRegisterDateJalali as RegisterDate
                    From [CustomerWarehouse].dbo.RequestFlows r
                    Join CTE c
                    	On r.BillId collate SQL_Latin1_General_CP1_CI_AS=c.BillId 
                    JOIN [Db70].dbo.T51 t51
                        On t51.C0=c.ZoneId
                    JOIN [Db70].dbo.T46 t46
                        On t51.C1=t46.C0
                    Where
                    	r.StatusId=97 AND
                    	c.RN=1";
        }
    }

}
