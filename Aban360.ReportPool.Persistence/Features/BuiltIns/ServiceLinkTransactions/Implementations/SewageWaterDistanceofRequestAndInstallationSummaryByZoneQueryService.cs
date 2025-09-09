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
                distanceRequestInstallationQuery = GetWaterDistanceRequestInstallationQuery(input.IsInstallation);
            else
                distanceRequestInstallationQuery = GetSewageDistanceRequestInstallationQuery(input.IsInstallation);

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
                (GetTitle(input.IsWater, input.IsInstallation),
                RequestHeader,
                RequestData);

            return result;
        }
        private string GetWaterDistanceRequestInstallationQuery(bool isIntallation)
        {
            string baseDate = isIntallation ? "WaterInstallDate" : "WaterRequestDate";

            return @$"Select	
						MAX(t46.C2) AS RegionTitle,
                    	c.ZoneTitle AS ZoneTitle,
						ROUND(AVG(CONVERT(float, DATEDIFF(DAY,[CustomerWarehouse].dbo.PersianToMiladi(c.WaterRequestDate), [CustomerWarehouse].dbo.PersianToMiladi(c.WaterInstallDate)))), 2) AS DistanceAverage
                    From [CustomerWarehouse].dbo.Clients c	
					Join [Db70].dbo.T51 t51
						On t51.C0=c.ZoneId
					Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
                    Where	
					    c.{baseDate} IS NOT NULL AND
					    c.WaterRegisterDateJalali IS NOT NULL AND
					    TRIM(c.WaterRegisterDateJalali) != '' AND
					    TRIM(c.{baseDate}) != '' AND
                    	c.WaterRegisterDateJalali BETWEEN @fromDate AND @toDate AND
                    	c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber AND
                    	c.ZoneId IN @zoneIds AND
            			c.ToDayJalali IS NULL
                    Group BY
                    	c.ZoneTitle";
        }
        private string GetSewageDistanceRequestInstallationQuery(bool isIntallation)
        {
            string baseDate = isIntallation ? "SewageInstallDate" : "SewageRequestDate";

            return @$"Select	
						MAX(t46.C2) AS RegionTitle,
                    	c.ZoneTitle AS ZoneTitle,
						ROUND(AVG(CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(c.SewageRequestDate), [CustomerWarehouse].dbo.PersianToMiladi(c.SewageInstallDate)))), 2) AS DistanceAverage
                    From [CustomerWarehouse].dbo.Clients c	
					Join [Db70].dbo.T51 t51
						On t51.C0=c.ZoneId
					Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
                    Where	
					    c.{baseDate} IS NOT NULL AND
					    c.SewageRegisterDateJalali IS NOT NULL AND
					    TRIM(c.{baseDate}) != '' AND
					    TRIM(c.SewageRegisterDateJalali) != '' AND
                    	c.SewageRegisterDateJalali BETWEEN @fromDate AND @toDate AND
                    	c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber AND
                    	c.ZoneId IN @zoneIds AND
            			c.ToDayJalali IS NULL
                    Group BY
                    	c.ZoneTitle";
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
