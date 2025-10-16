using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class NonPermanentBranchSummaryQueryService : NonPermanentBranchBase, INonPermanentBranchSummaryQueryService
    {
        public NonPermanentBranchSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchSummaryDataOutputDto>> GetInfo(NonPermanentBranchInputDto input)
        {
            string query = GetSummaryQuer();

            IEnumerable<NonPermanentBranchSummaryDataOutputDto> nonPremanentBranchData = await _sqlReportConnection.QueryAsync<NonPermanentBranchSummaryDataOutputDto>(query, input);
            NonPermanentBranchHeaderOutputDto nonPremanentBranchHeader = new NonPermanentBranchHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                RecordCount = (nonPremanentBranchData is not null && nonPremanentBranchData.Any()) ? nonPremanentBranchData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = ReportLiterals.NonPermanentBranchSummary + ReportLiterals.ByUsageAndZoneAndDiameter,
                CustomerCount =nonPremanentBranchData?.Sum(x=>x.Count)??0
            };

            var result = new ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchSummaryDataOutputDto>(ReportLiterals.NonPermanentBranchSummary+ReportLiterals.ByUsageAndZoneAndDiameter, nonPremanentBranchHeader, nonPremanentBranchData);

            return result;
        }

        private string GetNonPermanentBranchQuery()
        {
            return @"SELECT 
                        c.ZoneTitle,
						c.UsageTitle,
						c.WaterDiameterTitle AS MeterDiameterTitle,
						Count(1) AS Count
                    FROM [CustomerWarehouse].dbo.Clients c
                    WHERE 
            			c.ToDayJalali IS NULL AND
						(@fromReadingNumber IS NULL OR
						 @toReadingNumber IS NULL OR
						 c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
						c.ZoneId in @zoneIds AND
						c.IsNonPermanent=1
					Group By 
						c.ZoneTitle ,
						c.UsageTitle,
						c.WaterDiameterTitle";
        }
    }
}
