using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Constants;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class UsageChangeHistoryQueryService : ChangeHistoryBase, IUsageChangeHistoryQueryService
    {
        public UsageChangeHistoryQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ReportOutput<UsageChangeHistoryHeaderOutputDto, ChangeHistoryDataOutputDto>> GetInfo(UsageChangeHistoryInputDto input)
        {
            string query = GetDetailQuery(input.ZoneIds.HasValue(), GroupingFields.UsageId, GroupingFields.UsageTitle);

            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,

                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,

                zoneIds = input.ZoneIds,

                fromFieldIds = input.FromUsageIds,
                toFieldIds = input.ToUsageIds,
            };

            IEnumerable<ChangeHistoryDataOutputDto> usageChangeHistoryData = await _sqlReportConnection.QueryAsync<ChangeHistoryDataOutputDto>(query, @params);
            UsageChangeHistoryHeaderOutputDto usageChangeHistoryHeader = new UsageChangeHistoryHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,

                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,

                CustomerCount = usageChangeHistoryData is not null && usageChangeHistoryData.Any() ? usageChangeHistoryData.Count() : 0,
                RecordCount = usageChangeHistoryData is not null && usageChangeHistoryData.Any() ? usageChangeHistoryData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title= ReportLiterals.UsageChangeHistory,

                SumDomesticCount = usageChangeHistoryData is not null && usageChangeHistoryData.Any() ? usageChangeHistoryData.Sum(x => x.DomesticUnit) : 0,
                SumCommercialCount = usageChangeHistoryData is not null && usageChangeHistoryData.Any() ? usageChangeHistoryData.Sum(x => x.CommercialUnit) : 0,
                SumOtherCount = usageChangeHistoryData is not null && usageChangeHistoryData.Any() ? usageChangeHistoryData.Sum(x => x.OtherUnit) : 0,
                TotalUnit = usageChangeHistoryData is not null && usageChangeHistoryData.Any() ? usageChangeHistoryData.Sum(x => x.TotalUnit) : 0,
            };


            var result = new ReportOutput<UsageChangeHistoryHeaderOutputDto, ChangeHistoryDataOutputDto>(ReportLiterals.UsageChangeHistory, usageChangeHistoryHeader, usageChangeHistoryData);

            return result;
        }
    }
}
