using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Constants;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class RemovedBillSummaryByZoneQueryService : RemovedBillBase, IRemovedBillSummaryByZoneQueryService
    {
        public RemovedBillSummaryByZoneQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<RemovedBillHeaderOutputDto, RemovedBillSummaryDataOutputDto>> GetInfo(RemovedBillRawInputDto input)
        {
            string reportTitle = ReportLiterals.RemovedBillSummary + ReportLiterals.ByZone;
            string query = GetGroupedQuery(input.ZoneIds.HasValue(), GroupingFields.ZoneTitle);
           
            IEnumerable<RemovedBillSummaryDataOutputDto> RemovedBillData = await _sqlReportConnection.QueryAsync<RemovedBillSummaryDataOutputDto>(query, input);
            RemovedBillHeaderOutputDto RemovedBillHeader = new RemovedBillHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                RecordCount = RemovedBillData is not null && RemovedBillData.Any() ? RemovedBillData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title=reportTitle,

                SumAmount = RemovedBillData.Sum(x => x.Amount),
                CustomerCount = RemovedBillData.Sum(x => x.CustomerCount)
            };

            var result = new ReportOutput<RemovedBillHeaderOutputDto, RemovedBillSummaryDataOutputDto>(reportTitle, RemovedBillHeader, RemovedBillData);

            return result;
        }
    }
}
