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
    internal sealed class DeletionStateChangeHistoryQueryService : ChangeHistoryBase, IDeletionStateChangeHistoryQueryService
    {
        public DeletionStateChangeHistoryQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ReportOutput<DeletionStateChangeHistoryHeaderOutputDto, ChangeHistoryDataOutputDto>> GetInfo(DeletionStateChangeHistoryInputDto input)
        {
            string query = GetDetailQuery(input.ZoneIds.HasValue(), GroupingFields.DeletionStateId, GroupingFields.DeletionStateTitle);
            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,

                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,

                zoneIds = input.ZoneIds,

                fromFieldIds = input.FromDeletionStateIds,
                toFieldIds = input.ToDeletionStateIds,
            };

            IEnumerable<ChangeHistoryDataOutputDto> deletionStateChangeHistoryData = await _sqlReportConnection.QueryAsync<ChangeHistoryDataOutputDto>(query, @params, null, 180);
            DeletionStateChangeHistoryHeaderOutputDto deletionStateChangeHistoryHeader = new DeletionStateChangeHistoryHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,

                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,

                CustomerCount = deletionStateChangeHistoryData is not null && deletionStateChangeHistoryData.Any() ? deletionStateChangeHistoryData.Count() : 0,
                RecordCount = deletionStateChangeHistoryData is not null && deletionStateChangeHistoryData.Any() ? deletionStateChangeHistoryData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = ReportLiterals.DeletionStateChangeHistory,

                SumDomesticUnit= deletionStateChangeHistoryData is not null && deletionStateChangeHistoryData.Any() ? deletionStateChangeHistoryData.Sum(x => x.DomesticUnit) : 0,
                SumCommercialUnit = deletionStateChangeHistoryData is not null && deletionStateChangeHistoryData.Any() ? deletionStateChangeHistoryData.Sum(x => x.CommercialUnit) : 0,
                SumOtherUnit = deletionStateChangeHistoryData is not null && deletionStateChangeHistoryData.Any() ? deletionStateChangeHistoryData.Sum(x => x.OtherUnit) : 0,
                TotalUnit = deletionStateChangeHistoryData is not null && deletionStateChangeHistoryData.Any() ? deletionStateChangeHistoryData.Sum(x => x.TotalUnit) : 0,
            };


            var result = new ReportOutput<DeletionStateChangeHistoryHeaderOutputDto, ChangeHistoryDataOutputDto>(ReportLiterals.DeletionStateChangeHistory, deletionStateChangeHistoryHeader, deletionStateChangeHistoryData);

            return result;
        }
    }
}
