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
    internal sealed class SewageWaterInstallationSummaryQueryService : AbstractBaseConnection, ISewageWaterInstallationSummaryQueryService
    {
        public SewageWaterInstallationSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationSummaryDataOutputDto>> Get(SewageWaterInstallationInputDto input)
        {
            string installationSummaryQuery;
            if (input.IsWater)
                installationSummaryQuery = GetWaterInstallationSummaryQuery();
            else
                installationSummaryQuery = GetSewageInstallationSummaryQuery();

            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds,
                usageIds=input.UsageIds,
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,
            };
            IEnumerable<SewageWaterInstallationSummaryDataOutputDto> installationData = await _sqlReportConnection.QueryAsync<SewageWaterInstallationSummaryDataOutputDto>(installationSummaryQuery, @params);
            SewageWaterInstallationHeaderOutputDto installationHeader = new SewageWaterInstallationHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (installationData is not null && installationData.Any()) ? installationData.Count() : 0,

                SumCommercialUnit = installationData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = installationData.Sum(i => i.DomesticUnit),
                SumOtherUnit = installationData.Sum(i => i.OtherUnit),
                TotalUnit = installationData.Sum(i => i.TotalUnit),
            };
            var result = new ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationSummaryDataOutputDto>
                (input.IsWater ? ReportLiterals.WaterInstallationSummary : ReportLiterals.SewageInstallationSummary,
                installationHeader,
                installationData);
            
            return result;
        }
        private string GetWaterInstallationSummaryQuery()
        {
            return @"Select	
                    	c.UsageTitle AS UsageTitle,
                    	COUNT(c.UsageTitle) AS CustomerCount,
                        SUM(ISNULL(c.CommercialCount, 0) + ISNULL(c.DomesticCount, 0) + ISNULL(c.OtherCount, 0)) AS TotalUnit,
                        SUM(ISNULL(c.CommercialCount, 0)) AS CommercialUnit,
                        SUM(ISNULL(c.DomesticCount, 0)) AS DomesticUnit,
                        SUM(ISNULL(c.OtherCount, 0)) AS OtherUnit,
						SUM(CASE WHEN t5.C0 = 0 THEN 1 ELSE 0 END) AS UnSpecified,
				        SUM(CASE WHEN t5.C0 = 1 THEN 1 ELSE 0 END) AS Field0_5,
				        SUM(CASE WHEN t5.C0 = 2 THEN 1 ELSE 0 END) AS Field0_75,
				        SUM(CASE WHEN t5.C0 = 3 THEN 1 ELSE 0 END) AS Field1,
				        SUM(CASE WHEN t5.C0 = 4 THEN 1 ELSE 0 END) AS Field1_2,
				        SUM(CASE WHEN t5.C0 = 5 THEN 1 ELSE 0 END) AS Field1_5,
				        SUM(CASE WHEN t5.C0 = 6 THEN 1 ELSE 0 END) AS Field2,
				        SUM(CASE WHEN t5.C0 = 7 THEN 1 ELSE 0 END) AS Field3,
				        SUM(CASE WHEN t5.C0 = 8 THEN 1 ELSE 0 END) AS Field4,
				        SUM(CASE WHEN t5.C0 = 9 THEN 1 ELSE 0 END) AS Field5,
				        SUM(CASE WHEN t5.C0 In (10,11,12,13,15) THEN 1 ELSE 0 END) AS MoreThan6
                    From [CustomerWarehouse].dbo.Clients c
					Join [Db70].dbo.T5 t5
						On t5.C0=c.WaterDiameterId
                    Where	
                    	c.WaterInstallDate BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds AND
                        c.UsageId IN @usageIds AND
                        (@fromReadingNumber IS NULL OR
					    @toReadingNumber IS NULL OR
					    c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
						c.ToDayJalali IS NULL
                    Group BY
                    	c.UsageTitle";
            return @"DECLARE @columns NVARCHAR(MAX), @sql NVARCHAR(MAX);

                    SELECT @columns = STUFF((
                        SELECT DISTINCT ',[' + CAST(C2 AS NVARCHAR(10)) + ']'
                        FROM [Db70].dbo.T5
                        FOR XML PATH(''), TYPE
                    ).value('.', 'NVARCHAR(MAX)'), 1, 1, '');
                    
                    SET @sql = N'
                        SELECT
                            UsageTitle,
                            ' + @columns + '
                        FROM
                            (
                                SELECT
                                    c.UsageTitle,
                                    t5.C2 AS DiameterId
                                FROM
                                    [CustomerWarehouse].dbo.Clients c
                                JOIN
                                    [Db70].dbo.T5 t5 ON c.WaterDiameterId = t5.C0
                                WHERE
                                    c.WaterInstallDate BETWEEN @fromDate AND @toDate
                                    AND c.ZoneId IN @zoneIds
                                    AND c.ToDayJalali IS NULL
                                    AND (
                                        @fromReadingNumber IS NULL OR
                                        @toReadingNumber IS NULL OR
                                        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber
                                    )
                            ) AS SourceData
                        PIVOT
                            (
                                COUNT(DiameterId)
                                FOR DiameterId IN (' + @columns + ')
                            ) AS PivotTable
                        ORDER BY
                            UsageTitle;
                    ';
                    
                    EXEC sp_executesql @sql, N'@fromDate NVARCHAR(10), @toDate NVARCHAR(10), @fromReadingNumber NVARCHAR(10), @toReadingNumber NVARCHAR(10)',
                        @fromDate = @fromDate,
                        @toDate = @toDate,
                        @fromReadingNumber = @fromReadingNumber,
                        @toReadingNumber = @toReadingNumber;";
        }
        private string GetSewageInstallationSummaryQuery()
        {
            return @"Select	
                    	c.UsageTitle AS UsageTitle,
                    	COUNT(c.UsageTitle) AS CustomerCount,
					    SUM(ISNULL(c.CommercialCount, 0) + ISNULL(c.DomesticCount, 0) + ISNULL(c.OtherCount, 0)) AS TotalUnit,
						SUM(CASE WHEN t5.C0 = 0 THEN 1 ELSE 0 END) AS UnSpecified,
				        SUM(CASE WHEN t5.C0 = 1 THEN 1 ELSE 0 END) AS Field0_5,
				        SUM(CASE WHEN t5.C0 = 2 THEN 1 ELSE 0 END) AS Field0_75,
				        SUM(CASE WHEN t5.C0 = 3 THEN 1 ELSE 0 END) AS Field1,
				        SUM(CASE WHEN t5.C0 = 4 THEN 1 ELSE 0 END) AS Field1_2,
				        SUM(CASE WHEN t5.C0 = 5 THEN 1 ELSE 0 END) AS Field1_5,
				        SUM(CASE WHEN t5.C0 = 6 THEN 1 ELSE 0 END) AS Field2,
				        SUM(CASE WHEN t5.C0 = 7 THEN 1 ELSE 0 END) AS Field3,
				        SUM(CASE WHEN t5.C0 = 8 THEN 1 ELSE 0 END) AS Field4,
				        SUM(CASE WHEN t5.C0 = 9 THEN 1 ELSE 0 END) AS Field5,
				        SUM(CASE WHEN t5.C0 In (10,11,12,13,15) THEN 1 ELSE 0 END) AS MoreThan6
                    From [CustomerWarehouse].dbo.Clients c
					Join [Db70].dbo.T5 t5
						On t5.C0=c.WaterDiameterId
                    Where	
                    	c.SewageInstallDate BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds AND
                        c.UsageId IN @usageIds AND
                        (@fromReadingNumber IS NULL OR
					    @toReadingNumber IS NULL OR
					    c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
						c.ToDayJalali IS NULL
                    Group BY
                    	c.UsageTitle";
        }
    }
}
