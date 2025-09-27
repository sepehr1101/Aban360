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
    internal sealed class SewageWaterDistanceofRequestAndInstallationDetailQueryService : AbstractBaseConnection, ISewageWaterDistanceofRequestAndInstallationDetailQueryService
    {
        public SewageWaterDistanceofRequestAndInstallationDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, SewageWaterDistanceofRequestAndInstallationDetailDataOutputDto>> Get(SewageWaterDistanceofRequestAndInstallationInputDto input)
        {
            string distanceRequestInstallationQuery;
            if (input.IsWater)
                distanceRequestInstallationQuery = GetWaterDistanceRequestInstallationQuery(input.IsInstallation);
            else
                distanceRequestInstallationQuery = GetSewageDistanceRequestInstallationQuery(input.IsInstallation);

            string reportTitle = GetTitle(input.IsWater, input.IsInstallation);

            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds,
                fromReadingNumber=input.FromReadingNumber,
                toReadingNumber=input.ToReadingNumber,
            };
            IEnumerable<SewageWaterDistanceofRequestAndInstallationDetailDataOutputDto> RequestData = await _sqlReportConnection.QueryAsync<SewageWaterDistanceofRequestAndInstallationDetailDataOutputDto>(distanceRequestInstallationQuery, @params);
            SewageWaterDistanceofRequestAndInstallationHeaderOutputDto RequestHeader = new SewageWaterDistanceofRequestAndInstallationHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (RequestData is not null && RequestData.Any()) ? RequestData.Count() : 0,
                Title= reportTitle, 

                SumCommercialUnit = RequestData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = RequestData.Sum(i => i.DomesticUnit),
                SumOtherUnit = RequestData.Sum(i => i.OtherUnit),
                TotalUnit = RequestData.Sum(i => i.TotalUnit)
            };


            var result = new ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, SewageWaterDistanceofRequestAndInstallationDetailDataOutputDto>(reportTitle, RequestHeader, RequestData);

            return result;
        }
       
        private string GetWaterDistanceRequestInstallationQuery(bool isIntallation)
        {
            string baseDate = isIntallation ? "WaterInstallDate" : "WaterRequestDate";
            return @$"Select
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
						c.WaterInstallDate AS InstallationDate
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                        c.{baseDate} IS NOT NULL AND
                        TRIM(c.{baseDate}) != '' AND
                        c.WaterRegisterDateJalali IS NOT NULL AND
                        TRIM(c.WaterRegisterDateJalali) != '' AND
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                    	c.WaterRegisterDateJalali BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds AND
            			c.ToDayJalali IS NULL";
        }
        private string GetSewageDistanceRequestInstallationQuery(bool isIntallation)
        {
            string baseDate = isIntallation ? "SewageInstallDate" : "SewageRequestDate";

            return @$"Select
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
						c.SewageInstallDate AS InstallationDate
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                        c.{baseDate} IS NOT NULL AND
                        TRIM(c.{baseDate}) != '' AND
                        c.SewageRegisterDateJalali IS NOT NULL AND
                        TRIM(c.SewageRegisterDateJalali) != '' AND
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                    	c.SewageRegisterDateJalali BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds AND
            			c.ToDayJalali IS NULL";
        }
        private string GetTitle(bool IsWater, bool IsInstallation)
        {
            if (IsWater)
            {
                return IsInstallation ? ReportLiterals.WaterDistanceInstallationRegisterDetail : ReportLiterals.WaterDistanceRequestRegisterDetail;
            }
            else
            {
                return IsInstallation ? ReportLiterals.SewageDistanceInstallationeRegisterDetail : ReportLiterals.SewageDistanceRequesteRegisterDetail;
            }
        }
    }
}
