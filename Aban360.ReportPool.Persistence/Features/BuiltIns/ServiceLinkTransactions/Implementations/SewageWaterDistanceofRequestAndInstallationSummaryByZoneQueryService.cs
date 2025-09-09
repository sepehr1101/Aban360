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
    internal sealed class SewageWaterDistanceofRequestAndInstallationSummaryByZoneQueryService : AbstractBaseConnection, ISewageWaterDistanceofRequestAndInstallationSummaryByZoneQueryService
    {
        public SewageWaterDistanceofRequestAndInstallationSummaryByZoneQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, SewageWaterDistanceofRequestAndInstallationSummaryByZoneDataOutputDto>> Get(SewageWaterDistanceofRequestAndInstallationByZoneInputDto input)
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
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,
                zoneIds = input.ZoneIds
            };
            IEnumerable<SewageWaterDistanceofRequestAndInstallationSummaryByZoneDataOutputDto> RequestData = await _sqlReportConnection.QueryAsync<SewageWaterDistanceofRequestAndInstallationSummaryByZoneDataOutputDto>(distanceRequestInstallationQuery, @params);
            SewageWaterDistanceofRequestAndInstallationHeaderOutputDto RequestHeader = new SewageWaterDistanceofRequestAndInstallationHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = RequestData is not null && RequestData.Any() ? RequestData.Count() : 0
            };
            var result = new ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, SewageWaterDistanceofRequestAndInstallationSummaryByZoneDataOutputDto>
                (input.IsWater ? ReportLiterals.WaterDistanceRequestInstallationSummaryByZone : ReportLiterals.SewageDistanceRequesteInstallationSummaryByZone,
                RequestHeader,
                RequestData);

            return result;
        }
        private string GetWaterDistanceRequestInstallationQuery()
        {
            return @"Select	
						MAX(t46.C2) AS RegionTitle,
                    	c.ZoneTitle AS ZoneTitle,
						ROUND(AVG(CONVERT(float, DATEDIFF(DAY,[CustomerWarehouse].dbo.PersianToMiladi(c.WaterRequestDate), [CustomerWarehouse].dbo.PersianToMiladi(c.WaterInstallDate)))), 2) AS DistanceAverage
                    From [CustomerWarehouse].dbo.Clients c	
					Join [Db70].dbo.T51 t51
						On t51.C0=c.ZoneId
					Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
                    Where	
					    c.WaterRequestDate IS NOT NULL AND
					    c.WaterInstallDate IS NOT NULL AND
					    TRIM(c.WaterRequestDate) != '' AND
					    TRIM(c.WaterInstallDate) != '' AND
                    	c.WaterInstallDate BETWEEN @fromDate AND @toDate AND
                    	c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber AND
                    	c.ZoneId IN @zoneIds AND
            			c.ToDayJalali IS NULL
                    Group BY
                    	c.ZoneTitle";
        }
        private string GetSewageDistanceRequestInstallationQuery()
        {
            return @"Select	
						MAX(t46.C2) AS RegionTitle,
                    	c.ZoneTitle AS ZoneTitle,
						ROUND(AVG(CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(c.SewageRequestDate), [CustomerWarehouse].dbo.PersianToMiladi(c.SewageInstallDate)))), 2) AS DistanceAverage
                    From [CustomerWarehouse].dbo.Clients c	
					Join [Db70].dbo.T51 t51
						On t51.C0=c.ZoneId
					Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
                    Where	
					    c.SewageRequestDate IS NOT NULL AND
					    c.SewageInstallDate IS NOT NULL AND
					    TRIM(c.SewageRequestDate) != '' AND
					    TRIM(c.SewageInstallDate) != '' AND
                    	c.SewageInstallDate BETWEEN @fromDate AND @toDate AND
                    	c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber AND
                    	c.ZoneId IN @zoneIds AND
            			c.ToDayJalali IS NULL
                    Group BY
                    	c.ZoneTitle";
        }
    }
}
