using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class UsageSummaryQueryService : AbstractBaseConnection, IUsageSummaryQueryService
    {
        public UsageSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }
        public async Task<ReportOutput<UsageSummaryHeaderOutputDto, UsageSummaryDataOutputDto>> GetInfo(UsageDetailInputDto input)
        {
            string query = GetQuery();
            //string query = GetUsageSummaryQuery();

            IEnumerable<UsageSummaryDataOutputDto> usageSummaryData = await _sqlReportConnection.QueryAsync<UsageSummaryDataOutputDto>(query, input);
            UsageSummaryHeaderOutputDto usageSummaryHeader = new UsageSummaryHeaderOutputDto()
            {               
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromDateJalali=input.FromDateJalali,
                ToDateJalali=input.ToDateJalali,
                RecordCount = (usageSummaryData is not null && usageSummaryData.Any()) ? usageSummaryData.Count() : 0,
                CustomerCount = (usageSummaryData is not null && usageSummaryData.Any()) ? usageSummaryData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title= ReportLiterals.UsageSummary,
            };

            var result = new ReportOutput<UsageSummaryHeaderOutputDto, UsageSummaryDataOutputDto>(ReportLiterals.UsageSummary, usageSummaryHeader, usageSummaryData);

            return result;
        }

        private string GetQuery()
        {
            return @";WITH CTE AS
                    (
                    	SELECT 
                    	    RN= ROW_NUMBER() OVER (PARTITION by ZoneId , CustomerNumber ORDER BY RegisterDayJalali DESC, LocalId DESC),
                    	    *
                    	From [CustomerWarehouse].dbo.Clients c
                    	Where				
                    	    c.RegisterDayJalali BETWEEN @FromDateJalali AND @ToDateJalali AND
                    	    c.ZoneId IN @ZoneIds AND
                    	    (
                    	        @fromReadingNumber IS NULL OR 
                    	        @toReadingNumber IS NULL OR
                    	        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber
                    	    ) AND
                    	    c.CustomerNumber<>0
                    )
                    Select	
                    	c.ZoneTitle,
                    	c.UsageTitle,
                    	SUM( c.DomesticCount +c.CommercialCount +c.OtherCount) AS TotalUnit
                     FROM CTE c
                     JOIN [Db70].dbo.T51 t51
                         On t51.C0=c.ZoneId
                     JOIN [Db70].dbo.T46 t46
                         On t51.C1=t46.C0
                     WHERE	  
                         c.RN=1 AND
                         c.DeletionStateId NOT IN(1,2) AND
                    	 c.UsageId IN @UsageSellIds
                     GROUP BY 
                    	c.ZoneTitle,
                    	c.UsageTitle
                     ORDER BY 
                    	c.ZoneTitle,
                    	c.UsageTitle";
        }
    }
}
