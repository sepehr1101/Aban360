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
    internal sealed class SewageWaterDistanceofRequestAndInstallationSummaryQueryService : AbstractBaseConnection, ISewageWaterDistanceofRequestAndInstallationSummaryQueryService
    {
        public SewageWaterDistanceofRequestAndInstallationSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, SewageWaterDistanceofRequestAndInstallationSummaryDataOutputDto>> Get(SewageWaterDistanceofRequestAndInstallationInputDto input)
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
            IEnumerable<SewageWaterDistanceofRequestAndInstallationSummaryDataOutputDto> RequestData = await _sqlReportConnection.QueryAsync<SewageWaterDistanceofRequestAndInstallationSummaryDataOutputDto>(distanceRequestInstallationQuery, @params);
            SewageWaterDistanceofRequestAndInstallationHeaderOutputDto RequestHeader = new SewageWaterDistanceofRequestAndInstallationHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (RequestData is not null && RequestData.Any()) ? RequestData.Count() : 0
            };
            var result = new ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, SewageWaterDistanceofRequestAndInstallationSummaryDataOutputDto>
                (input.IsWater ? ReportLiterals.WaterDistanceRequestInstallationSummary : ReportLiterals.SewageDistanceRequesteInstallationSummary,
                RequestHeader,
                RequestData);

            return result;
        }
        private string GetWaterDistanceRequestInstallationQuery()
        {
            return @"Select	
                    	c.UsageTitle AS UsageTitle,
						ROUND(AVG(CONVERT(float, DATEDIFF(DAY,[CustomerWarehouse].dbo.PersianToMiladi(c.WaterRequestDate), [CustomerWarehouse].dbo.PersianToMiladi(c.WaterInstallDate)))), 2) AS DistanceAverage
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
					    c.WaterRequestDate IS NOT NULL AND
					    c.WaterInstallDate IS NOT NULL AND
					    TRIM(c.WaterRequestDate) != '' AND
					    TRIM(c.WaterInstallDate) != '' AND
                    	c.WaterInstallDate BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds
                    Group BY
                    	c.UsageTitle";
        }
        private string GetSewageDistanceRequestInstallationQuery()
        {
            return @"Select	
                    	c.UsageTitle AS UsageTitle,
						ROUND(AVG(CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(c.SewageRequestDate), [CustomerWarehouse].dbo.PersianToMiladi(c.SewageInstallDate)))), 2) AS DistanceAverage
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
					    c.SewageRequestDate IS NOT NULL AND
					    c.SewageInstallDate IS NOT NULL AND
					    TRIM(c.SewageRequestDate) != '' AND
					    TRIM(c.SewageInstallDate) != '' AND
                    	c.SewageInstallDate BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds
                    Group BY
                    	c.UsageTitle";
        }
    }
}
