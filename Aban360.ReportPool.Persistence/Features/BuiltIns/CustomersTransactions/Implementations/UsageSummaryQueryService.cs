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
        public async Task<ReportOutput<UsageSummaryHeaderOutputDto, UsageSummaryDataOutputDto>> GetInfo(UsageSummaryInputDto input)
        {
            string usageSummaryQuery = GetUsageSummaryQuery();
            var @params = new
            {
                input.FromReadingNumber,
                input.ToReadingNumber,
                UsageIds = input.UsageSellIds,
                input.ZoneIds
            };

            IEnumerable<UsageSummaryDataOutputDto> usageSummaryData = await _sqlReportConnection.QueryAsync<UsageSummaryDataOutputDto>(usageSummaryQuery, @params);
            UsageSummaryHeaderOutputDto usageSummaryHeader = new UsageSummaryHeaderOutputDto()
            {               
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                RecordCount = (usageSummaryData is not null && usageSummaryData.Any()) ? usageSummaryData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString()
            };

            var result = new ReportOutput<UsageSummaryHeaderOutputDto, UsageSummaryDataOutputDto>(ReportLiterals.UsageSummary, usageSummaryHeader, usageSummaryData);

            return result;
        }

        private string GetUsageSummaryQuery()
        {
            return @"SELECT 
                        c.ZoneTitle,
						c.UsageTitle,
						SUM( c.DomesticCount +c.CommercialCount +c.OtherCount) AS TotalUnit
						FROM [CustomerWarehouse].dbo.Clients c
                    WHERE 
            			c.ToDayJalali IS NULL AND
            			c.UsageId in @UsageIds AND
                        c.ReadingNumber BETWEEN @FromReadingNumber AND @ToReadingNumber AND
                        c.ZoneId in @ZoneIds 
						Group by c.UsageTitle,c.ZoneTitle";
        }
    }
}
