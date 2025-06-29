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
    internal sealed class SewageWaterDistanceofRequestAndInstallationDetailQueryService : AbstractBaseConnection, ISewageWaterDistanceofRequestAndInstallationDetailQueryService
    {
        public SewageWaterDistanceofRequestAndInstallationDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, SewageWaterDistanceofRequestAndInstallationDetailDataOutputDto>> Get(SewageWaterDistanceofRequestAndInstallationInputDto input)
        {
            string distanceRequestInstallationQuery;
            if (input.IsWater)
                distanceRequestInstallationQuery = GetWaterDistanceRequestInstallationQuery();
            else
                distanceRequestInstallationQuery = GetSewageDistanceRequestInstallationQuery();

            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds
            };
            IEnumerable<SewageWaterDistanceofRequestAndInstallationDetailDataOutputDto> RequestData = await _sqlReportConnection.QueryAsync<SewageWaterDistanceofRequestAndInstallationDetailDataOutputDto>(distanceRequestInstallationQuery, @params);

            RequestData.ForEach(data =>
            {
                data.DistanceOfRequestAndInstallation = CalcDistance(data.InstallationDate, data.RequestDate);
            });

            SewageWaterDistanceofRequestAndInstallationHeaderOutputDto RequestHeader = new SewageWaterDistanceofRequestAndInstallationHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDate = DateTime.Now.ToShortPersianDateString(),
                RecordCount = RequestData.Count()
            };
            var result = new ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, SewageWaterDistanceofRequestAndInstallationDetailDataOutputDto>
                (input.IsWater ? ReportLiterals.WaterDistanceRequestInstallationDetail : ReportLiterals.SewageDistanceRequesteInstallationDetail,
                RequestHeader,
                RequestData);

            return result;
        }
        private int CalcDistance(string fromDate, string toDate)
        {
            int distance = Convert.ToInt32((Convert.ToDateTime(fromDate) - Convert.ToDateTime(toDate)).Days);
            return distance;
        }
        private string GetWaterDistanceRequestInstallationQuery()
        {
            return @"Select
                    	c.CustomerNumber, 
                    	c.ReadingNumber,
                    	c.FirstName,
                    	c.SureName AS Surname,
                    	c.UsageTitle2 AS UsageTitle,
                    	c.WaterDiameterTitle AS MeterDiameterTitle,
                    	c.Address,
                    	c.ZoneTitle,
                    	c.ZoneId,
                    	c.DomesticCount	AS DomesticUnit,
                    	c.CommercialCount AS CommercialUnit,
                    	c.OtherCount AS OtherUnit,
                    	c.BillId,
                    	c.BranchType AS UseStateTitle,
                    	c.ContractCapacity,
                    	c.WaterRequestDate AS RequestDate,
						c.WaterInstallDate AS InstallationDate
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                        c.WaterRequestDate IS NOT NULL AND
                        TRIM(c.WaterRequestDate) != '' AND
                        c.WaterInstallDate IS NOT NULL AND
                        TRIM(c.WaterInstallDate) != '' AND
                    	c.WaterInstallDate BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds";
        }
        private string GetSewageDistanceRequestInstallationQuery()
        {
            return @"Select
                    	c.CustomerNumber, 
                    	c.ReadingNumber,
                    	c.FirstName,
                    	c.SureName AS Surname,
                    	c.UsageTitle2 AS UsageTitle,
                    	c.WaterDiameterTitle AS MeterDiameterTitle,
                    	c.Address,
                    	c.ZoneTitle,
                    	c.ZoneId,
                    	c.DomesticCount	AS DomesticUnit,
                    	c.CommercialCount AS CommercialUnit,
                    	c.OtherCount AS OtherUnit,
                    	c.BillId,
                    	c.BranchType AS UseStateTitle,
                    	c.ContractCapacity,
                    	c.SewageRequestDate AS RequestDate,
						c.SewageInstallDate AS InstallationDate
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                        c.SewageRequestDate IS NOT NULL AND
                        TRIM(c.SewageRequestDate) != '' AND
                        c.SewageInstallDate IS NOT NULL AND
                        TRIM(c.SewageInstallDate) != '' AND
                    	c.SewageInstallDate BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds";
        }
    }
}
