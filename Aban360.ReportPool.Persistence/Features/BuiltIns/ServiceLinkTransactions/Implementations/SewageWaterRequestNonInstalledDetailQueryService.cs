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
    internal sealed class SewageWaterRequestNonInstalledDetailQueryService : NonInstalledBase, ISewageWaterRequestNonInstalledDetailQueryService
    {
        public SewageWaterRequestNonInstalledDetailQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<SewageWaterRequestNonInstalledHeaderOutputDto, SewageWaterRequestNonInstalledDetailDataOutputDto>> Get(SewageWaterRequestNonInstalledInputDto input)
        {
            string query = GetDetailQuery(input.IsWater);
            //string query = GetQuery(input.IsWater);
            
            string reportTitle = input.IsWater ? ReportLiterals.WaterRequestNonInstalledDetail : ReportLiterals.SewageRequestNonInstalledDetail;
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,
                zoneIds = input.ZoneIds
            };
            IEnumerable<SewageWaterRequestNonInstalledDetailDataOutputDto> requestNonInstalledData = await _sqlReportConnection.QueryAsync<SewageWaterRequestNonInstalledDetailDataOutputDto>(query, @params);
            SewageWaterRequestNonInstalledHeaderOutputDto requestNonInstalledHeader = new SewageWaterRequestNonInstalledHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (requestNonInstalledData is not null && requestNonInstalledData.Any()) ? requestNonInstalledData.Count() : 0,
                Title = reportTitle,

                SumCommercialUnit = requestNonInstalledData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = requestNonInstalledData.Sum(i => i.DomesticUnit),
                SumOtherUnit = requestNonInstalledData.Sum(i => i.OtherUnit),
                TotalUnit = requestNonInstalledData.Sum(i => i.TotalUnit),
                CustomerCount = (requestNonInstalledData is not null && requestNonInstalledData.Any()) ? requestNonInstalledData.Count() : 0,
            };
            var result = new ReportOutput<SewageWaterRequestNonInstalledHeaderOutputDto, SewageWaterRequestNonInstalledDetailDataOutputDto>
                (reportTitle,
                requestNonInstalledHeader,
                requestNonInstalledData);

            return result;
        }
        private string GetQuery(bool isWater)
        {
            string requestDate=isWater? "WaterRequestDate" : "SewageRequestDate";
            string registerDate=isWater? "WaterRegisterDateJalali" : "SewageRegisterDateJalali";
            string installDate = isWater ? "WaterInstallDate" : "SewageInstallDate";
            string query = $@"
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
                    	c.{requestDate} AS RequestDate,
						c.{registerDate} AS RegisterDateJalali,
                        c.{installDate} AS InstallationDateJalali
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                    	{requestDate} BETWEEN @fromDate AND @toDate AND
						(TRIM(c.{installDate})<'1330/01/01' OR c.{installDate} IS NULL) AND
                    	c.ZoneId IN @zoneIds  AND
						c.ToDayJalali IS NULL AND
						(@fromReadingNumber IS NULL OR
						@toReadingNumber IS NULL OR
						c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber)";
            return query;
        }
        private string GetWaterRequestNonInstalledDetailQuery()
        {
            return @"Select
                        c.CustomerNumber, 
                    	c.ReadingNumber,
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
                        (c.DomesticCount+c.CommercialCount +c.OtherCount) AS TotalUnit ,
                    	c.BillId,
                    	c.BranchType AS UseStateTitle,
                    	c.ContractCapacity AS ContractualCapacity,
                    	c.WaterRequestDate AS RequestDate,
						c.WaterRegisterDateJalali AS RegisterDateJalali,
                        c.WaterInstallDate AS InstallationDateJalali
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                    	c.WaterRequestDate BETWEEN @fromDate AND @toDate AND
						(TRIM(c.WaterInstallDate)='' OR c.WaterInstallDate IS NULL) AND
                    	c.ZoneId IN @zoneIds  AND
						c.ToDayJalali IS NULL AND
						(@fromReadingNumber IS NULL OR
						@toReadingNumber IS NULL OR
						c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber)";
        }
        private string GetSewageRequestNonInstalledDetailQuery()
        {
            return @"Select
                    	c.CustomerNumber, 
                    	c.ReadingNumber,
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
                        (c.DomesticCount+c.CommercialCount +c.OtherCount) AS TotalUnit ,
                    	c.BillId,
                    	c.BranchType AS UseStateTitle,
                    	c.ContractCapacity AS ContractualCapacity,
                    	c.SewageRequestDate AS RequestDate,
						c.SewageRegisterDateJalali AS RegisterDateJalali,
                        c.SewageInstallDate AS InstallationDateJalali
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                    	c.SewageRequestDate BETWEEN @fromDate AND @toDate AND
						(TRIM(c.SewageRegisterDateJalali)='' OR c.SewageRegisterDateJalali IS NULL) AND
                    	c.ZoneId IN @zoneIds AND
						c.ToDayJalali IS NULL AND
						(@fromReadingNumber IS NULL OR
						@toReadingNumber IS NULL OR
						c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber)";
            
        }
    }
}
