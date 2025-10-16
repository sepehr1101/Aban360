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
    internal sealed class BranchTypeChangeHistoryQueryService : ChangeHistoryBase, IBranchTypeChangeHistoryQueryService
    {
        public BranchTypeChangeHistoryQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ReportOutput<BranchTypeChangeHistoryHeaderOutputDto, ChangeHistoryDataOutputDto>> GetInfo(BranchTypeChangeHistoryInputDto input)
        {
            string query = GetDetailQuery(input.ZoneIds.HasValue(), GroupingFields.UsageStateId, GroupingFields.BranchType);

            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,

                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,

                zoneIds = input.ZoneIds,

                fromFieldIds = input.FromUseStateIds,
                toFieldIds = input.ToUseStateIds,
            };

            IEnumerable<ChangeHistoryDataOutputDto> BranchTypeChangeHistoryData = await _sqlReportConnection.QueryAsync<ChangeHistoryDataOutputDto>(query, @params);
            BranchTypeChangeHistoryHeaderOutputDto BranchTypeChangeHistoryHeader = new BranchTypeChangeHistoryHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,

                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,

                CustomerCount = BranchTypeChangeHistoryData is not null && BranchTypeChangeHistoryData.Any() ? BranchTypeChangeHistoryData.Count() : 0,
                RecordCount = BranchTypeChangeHistoryData is not null && BranchTypeChangeHistoryData.Any() ? BranchTypeChangeHistoryData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = ReportLiterals.BranchTypeChangeHistory,

                SumDomesticCount = BranchTypeChangeHistoryData is not null && BranchTypeChangeHistoryData.Any() ? BranchTypeChangeHistoryData.Sum(x => x.DomesticUnit) : 0,
                SumCommercialCount = BranchTypeChangeHistoryData is not null && BranchTypeChangeHistoryData.Any() ? BranchTypeChangeHistoryData.Sum(x => x.CommercialUnit) : 0,
                SumOtherCount = BranchTypeChangeHistoryData is not null && BranchTypeChangeHistoryData.Any() ? BranchTypeChangeHistoryData.Sum(x => x.OtherUnit) : 0,
                TotalUnit = BranchTypeChangeHistoryData is not null && BranchTypeChangeHistoryData.Any() ? BranchTypeChangeHistoryData.Sum(x => x.TotalUnit) : 0,
            };


            var result = new ReportOutput<BranchTypeChangeHistoryHeaderOutputDto, ChangeHistoryDataOutputDto>(ReportLiterals.BranchTypeChangeHistory, BranchTypeChangeHistoryHeader, BranchTypeChangeHistoryData);

            return result;
        }
    }
}