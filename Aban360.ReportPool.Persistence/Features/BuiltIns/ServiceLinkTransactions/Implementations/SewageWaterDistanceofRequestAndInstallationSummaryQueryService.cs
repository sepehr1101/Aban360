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
    internal sealed class SewageWaterDistanceofRequestAndInstallationSummaryQueryService : AbstractBaseConnection, ISewageWaterDistanceofRequestAndInstallationSummaryQueryService
    {
        public SewageWaterDistanceofRequestAndInstallationSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, SewageWaterDistanceofRequestAndInstallationSummaryDataOutputDto>> Get(SewageWaterDistanceofRequestAndInstallationInputDto input)
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
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber
            };
            IEnumerable<SewageWaterDistanceofRequestAndInstallationSummaryDataOutputDto> RequestData = await _sqlReportConnection.QueryAsync<SewageWaterDistanceofRequestAndInstallationSummaryDataOutputDto>(distanceRequestInstallationQuery, @params);
            SewageWaterDistanceofRequestAndInstallationHeaderOutputDto RequestHeader = new SewageWaterDistanceofRequestAndInstallationHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (RequestData is not null && RequestData.Any()) ? RequestData.Count() : 0,
                Title = reportTitle
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
                    	c.UsageTitle AS UsageTitle,
						ROUND(AVG(CONVERT(float, DATEDIFF(DAY,[CustomerWarehouse].dbo.PersianToMiladi(c.WaterRequestDate), [CustomerWarehouse].dbo.PersianToMiladi(c.WaterInstallDate)))), 2) AS DistanceAverage
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
					    c.{baseDate} IS NOT NULL AND
					    c.WaterRegisterDateJalali IS NOT NULL AND
					    TRIM(c.{baseDate}) != '' AND
					    TRIM(c.WaterRegisterDateJalali) != '' AND
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                    	c.WaterRegisterDateJalali BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds AND
            			c.ToDayJalali IS NULL
                    Group BY
                    	c.UsageTitle";
        }
        private string GetSewageDistanceRequestInstallationQuery(bool isIntallation)
        {
            string baseDate = isIntallation ? "SewageInstallDate" : "SewageRequestDate";

            return @$"Select	
                    	c.UsageTitle AS UsageTitle,
						ROUND(AVG(CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(c.SewageRequestDate), [CustomerWarehouse].dbo.PersianToMiladi(c.SewageInstallDate)))), 2) AS DistanceAverage
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
					    c.SewageRegisterDateJalali IS NOT NULL AND
					    c.{baseDate} IS NOT NULL AND
					    TRIM(c.SewageRegisterDateJalali) != '' AND
					    TRIM(c.{baseDate}) != '' AND
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                    	c.SewageRegisterDateJalali BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds AND
            			c.ToDayJalali IS NULL
                    Group BY
                    	c.UsageTitle";
        }
        private string GetTitle(bool IsWater, bool IsInstallation)
        {
            if (IsWater)
            {
                return IsInstallation ? ReportLiterals.WaterDistanceInstallationRegisterSummary + ReportLiterals.ByUsage : ReportLiterals.WaterDistanceRequestRegisterSummary + ReportLiterals.ByUsage;
            }
            else
            {
                return IsInstallation ? ReportLiterals.SewageDistanceInstallationeRegisterSummary + ReportLiterals.ByUsage : ReportLiterals.SewageDistanceRequesteRegisterSummary + ReportLiterals.ByUsage;
            }
        }
    }
}
