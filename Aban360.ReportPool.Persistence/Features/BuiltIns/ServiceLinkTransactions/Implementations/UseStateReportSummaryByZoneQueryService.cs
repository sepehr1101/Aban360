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

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class UseStateReportSummaryByZoneQueryService : UseStateBase, IUseStateReportSummaryByZoneQueryService
    {
        public UseStateReportSummaryByZoneQueryService(IConfiguration configuration)
            : base(configuration) { }

        public async Task<ReportOutput<UseStateReportHeaderSummaryOutputDto, UseStateReportSummaryDataOutputDto>> Get(UseStateReportInputDto input)
        {
            string query = GetGroupedQuery(GroupingFields.ZoneTitle);
            //string query = GetUseStateReportQuery();
           
            var @params = new
            {
                useStateId = input.UseStateId,
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds,
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber
            };

            IEnumerable<UseStateReportSummaryDataOutputDto> data = await _sqlReportConnection.QueryAsync<UseStateReportSummaryDataOutputDto>(query, @params);
            UseStateReportHeaderSummaryOutputDto header = new UseStateReportHeaderSummaryOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = data is not null && data.Any() ? data.Count() : 0,

                SumCommercialUnit = data.Sum(i => i.CommercialUnit),
                SumDomesticUnit = data.Sum(i => i.DomesticUnit),
                SumOtherUnit = data.Sum(i => i.OtherUnit),
                TotalUnit = data.Sum(i => i.TotalUnit),
                CustomerCount = data.Sum(i => i.CustomerCount),
            };
            string useStateQuery = GetUseStateTitle();
            string useStateTitle = await _sqlConnection.QueryFirstOrDefaultAsync<string>(useStateQuery, new { useStateId = input.UseStateId });
            var result = new ReportOutput<UseStateReportHeaderSummaryOutputDto, UseStateReportSummaryDataOutputDto>(ReportLiterals.Report + " " + ReportLiterals.ByZone + useStateTitle, header, data);
            return result;
        }

        private string GetUseStateReportQuery()
        {
            return @";WITH CTE AS 
                     (SELECT 
						c.ZoneTitle,
                        c.ZoneId,
						c.WaterDiameterId,
						c.CommercialCount,
						c.DomesticCount,
						c.OtherCount,
						c.DeletionStateId,
	                    RN=ROW_NUMBER() OVER (PARTITION BY ZoneId, CustomerNumber ORDER BY RegisterDayJalali DESC)
                    FROM [CustomerWarehouse].dbo.Clients c 
                    WHERE 
                       c.FromDayJalali>=@fromDate and
                       c.ToDayJalali<=@toDate and
                       c.DeletionStateId=@useStateId and
                        (@fromReadingNumber IS NULL OR
						@toReadingNumber IS NULL OR
						c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) and  
                        c.ZoneId in @zoneIds
					)
                    SELECT 
						MAX(t46.C2) AS RegionTitle,
						c.ZoneTitle,
                    	COUNT(c.ZoneTitle) AS CustomerCount,
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
					FROM CTE c
					Join [Db70].dbo.T5 t5
						On t5.C0=c.WaterDiameterId	
					Join [Db70].dbo.T51 t51
						On t51.C0=c.ZoneId
					Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
                    WHERE 
						c.RN=1 AND 
						c.DeletionStateId=@useStateId 
					Group By 
						c.ZoneTitle";
        }
        private string GetUseStateTitle()
        {
            return @"select Title
                     from [Aban360].ClaimPool.UseState 
                     where Id=@useStateId";
        }
    }
}