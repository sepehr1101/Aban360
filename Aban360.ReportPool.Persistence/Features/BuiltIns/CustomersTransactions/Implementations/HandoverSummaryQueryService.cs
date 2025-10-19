using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class HandoverSummaryQueryService : AbstractBaseConnection, IHandoverSummaryQueryService
    {
        public HandoverSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<HandoverHeaderOutputDto, HandoverSummaryDataOutputDto>> Get(HandoverInputDto input)
        {
            string query = GetQuery();
            //string query = GetHandoverSummaryQuery();

            IEnumerable<HandoverSummaryDataOutputDto> data = await _sqlReportConnection.QueryAsync<HandoverSummaryDataOutputDto>(query, input);
            if (!data.Any())
            {
                throw new BaseException(ExceptionLiterals.NotFoundAnyData);
            }
            HandoverHeaderOutputDto header = new HandoverHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                RecordCount = (data is not null && data.Any()) ? data.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                CustomerCount = data?.Sum(r => r.Count) ?? 0,
                Title = ReportLiterals.HandoverSummary,
            };

            var result = new ReportOutput<HandoverHeaderOutputDto, HandoverSummaryDataOutputDto>(ReportLiterals.HandoverSummary, header, data);
            return result;
        }

        private string GetQuery()
        {
            return @";WITH CTE AS
                    (
                    	SELECT 
                    	    RN= ROW_NUMBER() OVER (PARTITION by ZoneId , CustomerNumber ORDER BY RegisterDayJalali DESC, LocalId DESC),
                    	    BranchType,
                    		DeletionStateId
                    	From [CustomerWarehouse].dbo.Clients c
                    	Where				
                    	    c.RegisterDayJalali BETWEEN @FromDateJalali AND @ToDateJalali AND
                    	    c.ZoneId IN @zoneIds AND
                    	    c.UsageStateId IN @branchTypeIds AND
                    	    (
                    	        @fromReadingNumber IS NULL OR 
                    	        @toReadingNumber IS NULL OR
                    	        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber
                    	    ) AND
                    	    c.CustomerNumber<>0 AND
                    	    c.RegisterDayJalali <= @ToDateJalali 
                    )
                    Select	
                    	c.BranchType AS UseStateTitle,
                    	Count(c.BranchType) AS Count
                     FROM CTE c
                     WHERE	  
                         c.RN=1 AND
                         c.DeletionStateId NOT IN(1,2)
                    GROUP BY c.BranchType";
        }
        private string GetHandoverSummaryQuery()
        {
            return @"Select
                    	c.BranchType AS UseStateTitle,
                    	Count(c.BranchType) AS Count
                    From [CustomerWarehouse].dbo.Clients c
                    Where 
                    	c.ToDayJalali IS NULL AND
                    	(@fromReadingNumber IS NULL OR
                    	 @toReadingNumber IS NULL OR
                    	 c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                    	 c.ZoneId IN @zoneIds  AND
						 c.UsageStateId IN @branchTypeIds
                    Group By 
                    	c.BranchType";
        }
    }
}
