using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Constants;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class SewageWaterDistanceofRequestAndInstallationSummaryByZoneQueryService : SewageWaterDistanceofRequestAndInstallationBase, ISewageWaterDistanceofRequestAndInstallationSummaryByZoneQueryService
    {
        public SewageWaterDistanceofRequestAndInstallationSummaryByZoneQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, SewageWaterDistanceofRequestAndInstallationSummaryDataOutputDto>> Get(SewageWaterDistanceofRequestAndInstallationByZoneInputDto input)
        {
            string query = GetGroupedQuery(input.IsWater, input.IsInstallation, GroupingFields.ZoneTitle);

            //string query;
            //if (input.IsWater)
            //    query = GetWaterDistanceRequestInstallationQuery(input.IsInstallation);
            //else
            //    query = GetSewageDistanceRequestInstallationQuery(input.IsInstallation);

            string reportTitle = GetTitle(input.IsWater, input.IsInstallation);

            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,
                zoneIds = input.ZoneIds
            };
            IEnumerable<SewageWaterDistanceofRequestAndInstallationSummaryDataOutputDto> RequestData = await _sqlReportConnection.QueryAsync<SewageWaterDistanceofRequestAndInstallationSummaryDataOutputDto>(query, @params);
            SewageWaterDistanceofRequestAndInstallationHeaderOutputDto RequestHeader = new SewageWaterDistanceofRequestAndInstallationHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = RequestData is not null && RequestData.Any() ? RequestData.Count() : 0,
                CustomerCount = RequestData is not null && RequestData.Any() ? RequestData.Count() : 0,
                Title = reportTitle,
            };
            var result = new ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, SewageWaterDistanceofRequestAndInstallationSummaryDataOutputDto>
                (reportTitle,
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
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
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
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                    	c.ZoneId IN @zoneIds AND
            			c.ToDayJalali IS NULL
                    Group BY
                    	c.ZoneTitle";
        }
        private string GetTitle(bool IsWater, bool IsInstallation)
        {
            if (IsWater)
            {
                return IsInstallation ? ReportLiterals.WaterDistanceInstallationRegisterSummary + ReportLiterals.ByZone : ReportLiterals.WaterDistanceRequestRegisterSummary + ReportLiterals.ByZone;
            }
            else
            {
                return IsInstallation ? ReportLiterals.SewageDistanceInstallationeRegisterSummary + ReportLiterals.ByZone : ReportLiterals.SewageDistanceRequesteRegisterSummary + ReportLiterals.ByZone;
            }
        }
    }
}
